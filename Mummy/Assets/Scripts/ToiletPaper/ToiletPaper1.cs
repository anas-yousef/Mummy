using System;
using System.Collections;
using UnityEngine;

public class ToiletPaper1 : MonoBehaviour
{
    [Header("Settings")]
    public float speed = 3f; // default speed 1 unit / second
    public float distance = 8f; // default distance 5 units
    private float _distance; // the distance it moves
    private bool _back; // is it coming back
    [SerializeField] PlayerThrow1 playerScript; //player script
    [SerializeField] private Transform playerPos;
    private bool _hit; //can get hit
    private Vector3 positionTrack;
    private bool ToiletPaperMoving;
    

    public void Throw ()
    {
        _distance = 0; 
        _back = false; 
        enabled = true;
    }
 
    private void Update ()
    {
        float travel = Time.deltaTime * speed;
        if (!_back)
        {
            positionTrack = transform.position;
            transform.Translate(new Vector3(1, 1, 0) * travel);
            _distance += Vector3.Distance(transform.position, positionTrack);
            _back = _distance >= distance || _hit;
        }
        else
        {
            positionTrack = transform.position;
            transform.position = Vector3.MoveTowards(transform.position, playerPos.position, travel);
            _distance -= Vector3.Distance(transform.position, positionTrack);
            enabled = _distance > 0.05;
            _hit = _distance > 0.05;
        }
        if (_distance > 0.05)
        {
            playerScript.SetPaperMoving(true);
            Debug.Log("Moving");
        }
        else
        {
            playerScript.SetPaperMoving(false);
            Debug.Log("NotMoving");
        }
    }
    private void OnEnable ()
    {
        gameObject.SetActive(true);
    }
    private void OnDisable ()
    {
        gameObject.SetActive (false);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "DraggableBox" && !_hit)
        {
            _hit = true;
            collision.gameObject.GetComponent<Rigidbody2D>().mass =1;
            playerScript.DraggableBoxHit(collision.gameObject);
        }
        if (collision.gameObject.layer == 10 && !_hit)
        {
            _hit = true;
            playerScript.WallHit(collision.gameObject);
        }
    }
    public void SetDistance(float distance)
    {
        _distance = distance;
    }
   
}
