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
    [SerializeField] private float seconds;
    [SerializeField] private PlayerController playerController;
    private GameObject target;
    private bool isPaperMoving;
    



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
        if (Input.GetButtonDown("Fire1") && !toiletPaper.gameObject.activeSelf && !jointLine.enabled && !playerController.GetIsJumpingFalling()) // if the toilet paper is not away 
        {
            toiletPaper.Throw();
            toiletPaper.gameObject.SetActive(true);
        }
        if (target != null)
        {
            toiletLine.SetPosition(0,transform.position);
            toiletLine.SetPosition(1, target.transform.position);
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
        jointLine.enabled = false;
        toiletLine.enabled = false;
        toiletPaper.transform.position = target.transform.position;
        toiletPaper.gameObject.SetActive(true);
        toiletPaper.GetComponent<ToiletPaper1>().SetDistance(Vector3.Distance(transform.position, target.transform.position));
        target.GetComponent<Rigidbody2D>().mass =1000;
        target = null;
    }
    public void ReleaseFloor()
    {
        toiletPaper.gameObject.SetActive(true);
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
