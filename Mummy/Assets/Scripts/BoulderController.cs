using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoulderController : MonoBehaviour
{
    [SerializeField] HingeJoint2D hingeJoint2D;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag.Equals("DraggableBox"))
        {
            JointMotor2D motor = hingeJoint2D.motor;
            motor.motorSpeed = -motor.motorSpeed;
            hingeJoint2D.motor = motor;
        }
    }
}
