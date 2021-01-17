using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MummyPaper : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] PlayerShooting playerScript; //player script
    [SerializeField] private Transform playerPos;
    public float speed = 3f; // default speed 1 unit / second
    public float distance = 8f; // default distance 5 units

    private float _distance; // the distance it moves
    private bool _back; // is it coming back
    private bool _hit; //can get hit
    private Vector3 positionTrack;


    public void Throw()
    {
        _distance = 0;
        _back = false;
        enabled = true;
    }

    private void Update()
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
            if (transform.localPosition == Vector3.zero)
            {
                transform.position = playerPos.position;
                _distance = 0;
            }
            else
            {
                _distance += 1;
            }
            enabled = _distance > 0;
            _hit = _distance > 0;
        }
        if (_distance > 0)
        {
            playerScript.SetPaperMoving(true);
        }
        else
        {

            playerScript.SetPaperMoving(false);
        }
    }
    private void OnEnable()
    {
        gameObject.SetActive(true);
    }
    private void OnDisable()
    {
        gameObject.SetActive(false);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "DraggableBox" && !_hit)
        {
            _hit = true;
            //collision.gameObject.GetComponent<Rigidbody2D>().mass = 1;
            playerScript.DraggableBoxHit(collision.gameObject);
        }
        if (collision.gameObject.layer == 10 && !_hit)
        {
            _hit = true;
            playerScript.WallHit(collision.gameObject);
        }
        if (collision.gameObject.tag == "SwingBox" && !_hit)
        {
            _hit = true;
            playerScript.SwingBoxHit(collision.gameObject);
        }
    }
    public void SetDistance(float distance)
    {
        _distance = distance;
    }
}
