using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeController : MonoBehaviour
{
    //GameObject[] Boxes = new GameObject[];
    [SerializeField] private List<GameObject> boxesOnRope;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        checkBoxedOnRope();
    }

    private void checkBoxedOnRope()
    {
        int counter = 0;
        for (int i = 0; i < boxesOnRope.Count; i++)
        {
            if (boxesOnRope[i].GetComponent<DraggableBoxScript>().GetOnRope())
            {
                counter += 1;
            }
        }
        if (counter == boxesOnRope.Count)
        {
            for (int i = 0; i < boxesOnRope.Count; i++)
            {
                boxesOnRope[i].GetComponent<Rigidbody2D>().mass = 52;
            }
        }
    }
}
