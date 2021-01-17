using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DraggableCheckPlayer : MonoBehaviour
{
    [SerializeField] Rigidbody2D parentRB;
    [SerializeField] float parentMass;
    // Start is called before the first frame update
    void Start()
    {
        parentRB = GetComponentInParent<Rigidbody2D>();
    }

    // Update is called once per frame
    //void Update()
    //{

    //}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            parentRB.mass = parentMass;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            parentRB.mass = 5f;
        }
    }
}
