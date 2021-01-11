using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DraggableBoxScript : MonoBehaviour
{
    [SerializeField] float step;
    [SerializeField] Transform player;
    [SerializeField] Transform toiletPaper;
    private bool moving;
    private float distance;


    private void Update()
    {
        /*if (moving)
        {
            distance+= Time.deltaTime * 3f;
            transform.position = Vector3.MoveTowards(transform.position, player.position, step);
            toiletPaper.position = transform.position;
        }*/
    }
    public void StartMoving()
    {
       // moving = true;
    }

    public void StopMoving()
    {
        //moving = false;
        //return distance;
    }
}
