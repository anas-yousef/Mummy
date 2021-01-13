using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    [Header("Settings")]
    public MummyPaper toiletPaper;
    [SerializeField] private LineRenderer toiletLine;
    [SerializeField] private SpringJoint2D springJoint;
    [SerializeField] private DistanceJoint2D distanceJoint;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private float seconds;
    [SerializeField] private float distanceFromLastNode = 0.2f;
    [SerializeField] private float pullingPlayerSpeed = 5f;
    [SerializeField] private float jumpOnSwingForce = 60;
    [SerializeField] private float NodesFactor=4;
    private int numberOfNodes;
    private Vector3 positionBeforeSwing;
    private GameObject target;
    GameObject[] jointNodes;
    private bool isSwingnig;




    enum CollidedObject
    {
        DraggableBox,
        Floor,
        SwingableBox
    }


    private void Start()
    {
        springJoint.enabled = false;
        distanceJoint.enabled = false;
        jointNodes = new GameObject[numberOfNodes];
        toiletPaper.gameObject.SetActive(false);
        toiletLine.enabled = false;
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire1") && !toiletPaper.gameObject.activeSelf && !springJoint.enabled && !playerController.GetIsJumpingFalling())
        {
            toiletPaper.Throw();
            toiletPaper.gameObject.SetActive(true);
        }
        if (target != null && isSwingnig)
        {
            toiletLine.positionCount = numberOfNodes;
            for(int i = 0; i < numberOfNodes; i++)
            {
                toiletLine.SetPosition(i, jointNodes[i].transform.position);
            }
        }
        if(target != null && !isSwingnig)
        {
            toiletLine.positionCount = 2;
            toiletLine.SetPosition(0, transform.position);
            toiletLine.SetPosition(1, target.transform.position);
        }
        if (distanceJoint.enabled && distanceJoint.distance > distanceFromLastNode)
        {
            distanceJoint.distance -= pullingPlayerSpeed * Time.deltaTime;
        }
        if (Input.GetButtonDown("Fire1") && isSwingnig)
        {
            SwingBoxRelease();
        }
    }
    private IEnumerator ReleaseAfterSeconds(CollidedObject collidedObject)
    {
        yield return new WaitForSeconds(seconds);
        if (collidedObject.Equals(CollidedObject.DraggableBox))
        {
            DraggableBoxRelease();
        }
        if (collidedObject.Equals(CollidedObject.Floor))
        {
            ReleaseFloor();
        }

    }
    public void DraggableBoxRelease()
    {
        toiletPaper.transform.position = target.transform.position;
        toiletPaper.GetComponent<MummyPaper>().SetDistance(Vector3.Distance(transform.position, target.transform.position));
        toiletPaper.gameObject.SetActive(true);
        target.GetComponent<Rigidbody2D>().mass = 10000;
        springJoint.enabled = false;
        toiletLine.enabled = false;
        target = null;
    }
    public void ReleaseFloor()
    {
        toiletPaper.gameObject.SetActive(true);
    }
    public void SwingBoxRelease()
    {
        toiletPaper.transform.position = target.transform.position;
        isSwingnig = false;
        toiletPaper.gameObject.SetActive(true);
        distanceJoint.enabled = false;
        toiletLine.enabled = false;
        RemoveCollider();
        target = null;
        springJoint.enabled = false;
    }
    public void DraggableBoxHit(GameObject hit)
    {
        target = hit;
        toiletLine.enabled = true;
        springJoint.enabled = true;
        springJoint.connectedBody = target.GetComponent<Rigidbody2D>();
        toiletPaper.gameObject.SetActive(false);
        StartCoroutine(ReleaseAfterSeconds(CollidedObject.DraggableBox));

    }
    public void SwingBoxHit(GameObject hit)
    {
        positionBeforeSwing = transform.position;
        isSwingnig = true;
        target = hit;
        numberOfNodes = (int)(Vector3.Distance(target.transform.position, transform.position)* NodesFactor)-1;
        jointNodes = new GameObject[numberOfNodes];
        toiletLine.enabled = true;
        distanceJoint.distance = Vector3.Distance(transform.position, target.transform.position);
        AddColliderToLine();
        distanceJoint.enabled = true;
        toiletPaper.gameObject.SetActive(false);
    }
    private void AddColliderToLine()
    {
        gameObject.GetComponent<Rigidbody2D>().AddForce(transform.up * jumpOnSwingForce,ForceMode2D.Impulse);
        for (int i=0; i < numberOfNodes; i++)
        {
            jointNodes[i] = Instantiate(Resources.Load("Nodes/NodeJoint")) as GameObject;
            jointNodes[i].transform.position =target.transform.position;
        }
        jointNodes[0].GetComponent<DistanceJoint2D>().connectedBody = target.GetComponent<Rigidbody2D>();
        for (int i = 1; i < numberOfNodes; i++)
        {
                jointNodes[i].GetComponent<DistanceJoint2D>().connectedBody = jointNodes[i - 1].GetComponent<Rigidbody2D>();
        }
        gameObject.GetComponent<DistanceJoint2D>().connectedBody = jointNodes[numberOfNodes-1].GetComponent<Rigidbody2D>();
    }
    public void RemoveCollider()
    {
        for(int i = 0; i < numberOfNodes; i++)
        {
            Destroy(jointNodes[i]);
        }
    }
    public void WallHit(GameObject hit)
    {
        toiletPaper.gameObject.SetActive(false);
        ReleaseFloor();
    }
    public void SetPaperMoving(bool isPaperMoving)
    {
        playerController.SetCanMove(!isPaperMoving);
    }
}
