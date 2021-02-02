using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoulderController : MonoBehaviour
{
    [SerializeField] HingeJoint2D hingeJoint2D;
    private int hitCounter = 0;
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
            hitCounter += 40;
            JointMotor2D motor = hingeJoint2D.motor;
            if (motor.motorSpeed < 0f)
            {
                motor.motorSpeed = -motor.motorSpeed - hitCounter;
            }
            if(motor.motorSpeed > 0f)
            {
                motor.motorSpeed = -motor.motorSpeed + hitCounter;
            }
            hingeJoint2D.motor = motor;
        }
    }
}
