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
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private float seconds;
    [SerializeField] private float jumpOnSwingForce = 60;
    [SerializeField] private float NodesFactor = 4;
    [SerializeField] private float DraggingNodesFactor = 5;
    private int numberOfNodes;
    private Vector3 positionBeforeSwing;
    private GameObject target;
    private Vector3 swingPoint=Vector3.zero;
    GameObject[] jointNodes;
    private bool isSwingnig;
    private bool isDragging;
    private Vector3 hitPoint;




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
        if (Input.GetButtonDown("Fire1") && !toiletPaper.gameObject.activeSelf && !distanceJoint.enabled && !playerMovement.InAir())
        {
            toiletPaper.Throw();
            toiletPaper.gameObject.SetActive(true);
        }
        else if(Input.GetButtonDown("Fire1") && toiletPaper.gameObject.activeSelf)
        {
            toiletPaper.SendBack();
        }
        if (Input.GetButtonDown("Fire1") && isSwingnig)
        {
            SwingBoxReleasePoint();
        }
        if (Input.GetButtonDown("Fire1") && isDragging)
        {
            DraggableBoxReleaseWithCollider();
        }
        if(toiletPaper.getDistination()>0 && !isDragging)
        {
            playerMovement.SetCanMove(false);
        }
        else
        {
            playerMovement.SetCanMove(true);
        }
        AssignLineToPoint();
        AssignLineToTarget();
    }

    private void AssignLineToTarget()
    {
        if (target != null && isDragging)
        {
            toiletLine.positionCount = numberOfNodes + 1;
            toiletLine.SetPosition(0, jointNodes[0].transform.position);
            for (int i = 1; i < numberOfNodes; i++)
            {
                toiletLine.SetPosition(i, jointNodes[i].transform.position);
            }
            toiletLine.SetPosition(numberOfNodes, transform.position);
        }
    }
    private void AssignLineToPoint()
    {
        if (swingPoint != Vector3.zero && isSwingnig)
        {
            toiletLine.positionCount = numberOfNodes + 1;
            toiletLine.SetPosition(0, swingPoint);
            for (int i = 0; i < numberOfNodes; i++)
            {
                toiletLine.SetPosition(i+1, jointNodes[i].transform.position);
            }
            toiletLine.SetPosition(numberOfNodes, transform.position);
        }
        else
        {
            toiletLine.positionCount = 2;
            toiletLine.SetPosition(0, transform.position);
            toiletLine.SetPosition(1, toiletPaper.transform.position);
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
            Release();
        }

    }
    public void DraggableBoxRelease()
    {
        toiletPaper.transform.position = target.transform.position;
        toiletPaper.GetComponent<MummyPaper>().SetDistance(Vector3.Distance(transform.position, target.transform.position));
        toiletPaper.gameObject.SetActive(true);
        springJoint.enabled = false;
        target = null;
    }
    public void DraggableBoxReleaseWithCollider()
    {
        toiletPaper.transform.position = target.transform.position;
        isDragging = false;
        toiletPaper.GetComponent<MummyPaper>().SetDistance(Vector3.Distance(transform.position, target.transform.position));
        toiletPaper.gameObject.SetActive(true);
        distanceJoint.enabled = false;
        RemoveCollider();
        target = null;
    }

    public void Release()
    {
        toiletPaper.gameObject.SetActive(true);
    }
    public void SwingBoxRelease()
    {
        toiletPaper.transform.position = target.transform.position;
        isSwingnig = false;
        playerMovement.SetIsSwinging(isSwingnig);
        toiletPaper.gameObject.SetActive(true);
        distanceJoint.enabled = false;
        RemoveCollider();
        target = null;
        springJoint.enabled = false;
    }
    public void SwingBoxReleasePoint()
    {
        toiletPaper.transform.position = swingPoint;
        isSwingnig = false;
        playerMovement.SetIsSwinging(isSwingnig);
        toiletPaper.gameObject.SetActive(true);
        distanceJoint.enabled = false;
        RemoveCollider();
        swingPoint = Vector3.zero;
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
    public void DraggableBoxHitWithCollider(GameObject hit,Vector3 DraghitPoint)
    {
        target = hit;
        hitPoint = DraghitPoint;
        toiletLine.enabled = true;
        distanceJoint.enabled = true;
        numberOfNodes = (int)(Vector3.Distance(target.transform.position, transform.position)*DraggingNodesFactor);
        toiletPaper.gameObject.SetActive(false);
        jointNodes = new GameObject[numberOfNodes];
        AddNodesToObject();
        isDragging = true;

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
        AddNodesToObject();
        distanceJoint.enabled = true;
        toiletPaper.gameObject.SetActive(false);
    }
    public void SwingBoxHit(Vector3 hitPoint)
    {
        numberOfNodes = (int)(Vector3.Distance(hitPoint, transform.position) * NodesFactor);
        if (numberOfNodes > 0)
        {
            positionBeforeSwing = transform.position;
            isSwingnig = true;
            playerMovement.SetIsSwinging(isSwingnig);
            swingPoint = hitPoint;
            jointNodes = new GameObject[numberOfNodes];
            toiletLine.enabled = true;
            AddNodesToPoint();
            distanceJoint.enabled = true;
            toiletPaper.gameObject.SetActive(false);
        }
        else
        {
            toiletPaper.gameObject.SetActive(false);
            Release();
        }

    }
    private void AddNodesToObject()
    {
        toiletLine.positionCount = numberOfNodes;
        for (int i=0; i < numberOfNodes; i++)
        {
            jointNodes[i] = Instantiate(Resources.Load("Nodes/DragNodeJoint")) as GameObject;
            jointNodes[i].transform.position = hitPoint;
            toiletLine.SetPosition(i, jointNodes[i].transform.position);
        }
        jointNodes[0].GetComponent<DistanceJoint2D>().connectedBody = target.GetComponent<Rigidbody2D>();
        for (int i = 1; i < numberOfNodes; i++)
        {
            jointNodes[i].GetComponent<DistanceJoint2D>().connectedBody = jointNodes[i - 1].GetComponent<Rigidbody2D>();
            jointNodes[i].GetComponent<DistanceJoint2D>().autoConfigureDistance = false;
        }
        gameObject.GetComponent<DistanceJoint2D>().connectedBody = jointNodes[numberOfNodes - 1].GetComponent<Rigidbody2D>();
        jointNodes[numberOfNodes - 1].GetComponent<DistanceJoint2D>().autoConfigureDistance = false;

    }

    private void AddNodesToPoint()
    {
        toiletLine.positionCount = numberOfNodes;
        gameObject.GetComponent<Rigidbody2D>().AddForce(transform.up * jumpOnSwingForce, ForceMode2D.Impulse);
        for (int i = 0; i < numberOfNodes; i++)
        {
            jointNodes[i] = Instantiate(Resources.Load("Nodes/NodeJoint")) as GameObject;
            jointNodes[i].transform.position = swingPoint;
            toiletLine.SetPosition(i, jointNodes[i].transform.position);
        }
        jointNodes[0].GetComponent<DistanceJoint2D>().connectedAnchor = swingPoint;
        for (int i = 1; i < numberOfNodes; i++)
        {
            jointNodes[i].GetComponent<DistanceJoint2D>().connectedBody = jointNodes[i - 1].GetComponent<Rigidbody2D>();
        }
        gameObject.GetComponent<DistanceJoint2D>().connectedBody = jointNodes[numberOfNodes - 1].GetComponent<Rigidbody2D>();
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
        Release();
    }
    public void SetPaperMoving(bool isPaperMoving)
    {
            playerMovement.SetCanMove(!isPaperMoving);
    }
    public void RestartPlayer()
    {
        springJoint.enabled = false;
        distanceJoint.enabled = false;
        isSwingnig = false;
        playerMovement.SetIsSwinging(isSwingnig);
        playerMovement.SetCanMove(true);
        playerMovement.StopMovement();
        target = null;
        toiletPaper.gameObject.SetActive(false);
        RemoveCollider();
    }
}
