using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DraggableBoxScript : MonoBehaviour
{
    private bool onRope;

    private void Start()
    {
        onRope = false;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag.Equals("NodeRope"))
        {
            onRope = true;
            //Debug.Log("Good");
        }
        
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag.Equals("NodeRope"))
        {
            onRope = false;
            //Debug.Log("Good");
        }
    }

    public bool GetOnRope()
    {
        return onRope;
    }



    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.gameObject.tag == "Player")
    //    {
    //        GetComponent<Rigidbody2D>().mass = 40;
    //    }

    //}
    //private void OnTriggerExit2D(Collider2D collision)
    //{
    //    if (collision.gameObject.tag == "Player")
    //    {
    //        GetComponent<Rigidbody2D>().mass = 5;
    //    }
    //}
    //private void OnTriggerStay2D(Collider2D collision)
    //{
    //    if (collision.gameObject.tag == "Player")
    //    {
    //        GetComponent<Rigidbody2D>().mass = 40;
    //    }
    //}
}
