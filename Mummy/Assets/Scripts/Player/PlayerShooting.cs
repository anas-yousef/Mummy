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
    [SerializeField] private HingeJoint2D hingeJoint;
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private float seconds;
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
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire1") && !toiletPaper.gameObject.activeSelf && !springJoint.enabled && !playerMovement.InAir())
        {
            toiletPaper.Throw();
            toiletPaper.gameObject.SetActive(true);
        }
        if (target != null && isSwingnig)
        {
            toiletLine.positionCount = numberOfNodes+1;
            for(int i = 0; i < numberOfNodes; i++)
            {
                toiletLine.SetPosition(i, jointNodes[i].transform.position);
            }
            toiletLine.SetPosition(numberOfNodes, transform.position);
        }
        else if(target != null && !isSwingnig)
        {
            toiletLine.positionCount = 2;
            toiletLine.SetPosition(0, transform.position);
            toiletLine.SetPosition(1, target.transform.position);
        }
        else
        {
            toiletLine.positionCount = 2;
            toiletLine.SetPosition(0, transform.position);
            toiletLine.SetPosition(1, toiletPaper.transform.position);
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
        playerMovement.SetIsSwinging(isSwingnig);
        toiletPaper.gameObject.SetActive(true);
        //distanceJoint.enabled = false;
        hingeJoint.enabled = false;
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
        playerMovement.SetIsSwinging(isSwingnig);
        target = hit;
        numberOfNodes = (int)(Vector3.Distance(target.transform.position, transform.position)* NodesFactor);
        jointNodes = new GameObject[numberOfNodes];
        toiletLine.enabled = true;
        AddColliderToLine();
        //distanceJoint.enabled = true;
        hingeJoint.enabled = true;
        toiletPaper.gameObject.SetActive(false);
    }
    private void AddColliderToLine()
    {
        toiletLine.positionCount = numberOfNodes;
        gameObject.GetComponent<Rigidbody2D>().AddForce(transform.up * jumpOnSwingForce,ForceMode2D.Impulse);
        GameObject hook = new GameObject();
        hook.transform.position = target.transform.position;

        for (int i=0; i < numberOfNodes; i++)
        {
            jointNodes[i] = Instantiate(Resources.Load("Nodes/NodeJoint")) as GameObject;
            //jointNodes[i].transform.position = (target.transform.position + transform.position) * (i + 1) / numberOfNodes;
            jointNodes[i].transform.SetParent(hook.transform); 
            toiletLine.SetPosition(i, jointNodes[i].transform.position);
        }
        jointNodes[0].GetComponent<HingeJoint2D>().connectedBody = target.GetComponent<Rigidbody2D>();
        for (int i = 1; i < numberOfNodes; i++)
        {
                jointNodes[i].GetComponent<HingeJoint2D>().connectedBody = jointNodes[i - 1].GetComponent<Rigidbody2D>();
        }
        gameObject.GetComponent<HingeJoint2D>().connectedBody = jointNodes[numberOfNodes - 1].GetComponent<Rigidbody2D>();

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
        playerMovement.SetCanMove(!isPaperMoving);
    }
}
