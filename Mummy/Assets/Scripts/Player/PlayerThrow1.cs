using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerThrow1 : MonoBehaviour
{
    [Header("Settings")]
    public ToiletPaper1 toiletPaper;
    [SerializeField] private LineRenderer toiletLine;
    [SerializeField] private SpringJoint2D jointLine;
    [SerializeField] private DistanceJoint2D distanceJoint;
    [SerializeField] private float seconds;
    [SerializeField] private PlayerController playerController;
    private Vector3 positionBeforeSwing;
    private GameObject target;
    private bool isPaperMoving;
    private bool isSwingnig;
    



    enum CollidedObject{
        DraggableBox,
        Floor,
        SwingableBox
    }


    private void Start()
    {
        toiletPaper.gameObject.SetActive(false);
        toiletLine.enabled = false;
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire1") && !toiletPaper.gameObject.activeSelf && !jointLine.enabled && !playerController.InAir())  
        {
            toiletPaper.Throw();
            toiletPaper.gameObject.SetActive(true);
        }
        if (target != null)
        {
            toiletLine.SetPosition(0,transform.position);
            toiletLine.SetPosition(1, target.transform.position);
        }
        if(distanceJoint.enabled && distanceJoint.distance > Vector3.Distance(positionBeforeSwing, target.transform.position) - 2f)
        {
            distanceJoint.distance -= 0.008f;
        }
        if(Input.GetButtonDown("Fire1") && isSwingnig)
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
        toiletPaper.GetComponent<ToiletPaper1>().SetDistance(Vector3.Distance(transform.position, target.transform.position));
        toiletPaper.gameObject.SetActive(true);
        target.GetComponent<Rigidbody2D>().mass =10000;
        jointLine.enabled = false;
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
        target = null;
    }
    public void DraggableBoxHit(GameObject hit)
    {
        target = hit;
        toiletLine.enabled = true;
        jointLine.enabled = true;
        jointLine.connectedBody = target.GetComponent<Rigidbody2D>();
        toiletPaper.gameObject.SetActive(false);
        StartCoroutine(ReleaseAfterSeconds(CollidedObject.DraggableBox));

    }
    public void SwingBoxHit(GameObject hit)
    {
        positionBeforeSwing = transform.position;
        isSwingnig = true;
        target = hit;
        toiletLine.enabled = true;
        distanceJoint.enabled = true;
        distanceJoint.connectedBody = target.GetComponent<Rigidbody2D>();
        distanceJoint.distance = Vector3.Distance(transform.position, target.transform.position);
        toiletPaper.gameObject.SetActive(false);
        toiletLine.SetPosition(0, transform.position);
    }
    public void WallHit(GameObject hit)
    {
        toiletPaper.gameObject.SetActive(false);
        ReleaseFloor();
    }
    public void SetPaperMoving(bool isPaperMoving)
    {
        this.isPaperMoving = isPaperMoving;
        playerController.SetCanMove(!isPaperMoving);
    }

}
