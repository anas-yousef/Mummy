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


    private GameObject target;
    
    private void Start()
    {
        toiletPaper.gameObject.SetActive(false);
        toiletLine.enabled = false;
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire1") && !toiletPaper.gameObject.activeSelf && !jointLine.enabled) // if the toilet paper is not away 
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

    public void TargetHit(GameObject hit)
    {
        target = hit;
        target.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        toiletLine.enabled = true;
        jointLine.enabled = true;
        jointLine.connectedBody = target.GetComponent<Rigidbody2D>();
        toiletPaper.gameObject.SetActive(false);
        StartCoroutine(ReleaseAfterSeconds());

    }
    private IEnumerator ReleaseAfterSeconds()
    {
        yield return new WaitForSeconds(seconds);
        Release();
    }
    public void Release()
    {
        jointLine.enabled = false;
        toiletLine.enabled = false;
        toiletPaper.transform.position = target.transform.position;
        toiletPaper.gameObject.SetActive(true);
        toiletPaper.GetComponent<ToiletPaper1>().SetDistance(Vector3.Distance(transform.position, target.transform.position));
        target.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        target = null;
    }
}
