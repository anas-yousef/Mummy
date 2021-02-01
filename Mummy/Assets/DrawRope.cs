using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawRope : MonoBehaviour
{
    [SerializeField] GameObject[] jointNodes = new GameObject[10];
    LineRenderer line;
    // Start is called before the first frame update
    void Start()
    {
        //jointNodes
        line = GetComponent<LineRenderer>();
    }
    private void Update()
    {
        line.positionCount = 12;

        line.SetPosition(0, jointNodes[0].GetComponent<DistanceJoint2D>().connectedAnchor);
        for (int i = 0; i < 10; i++)
        {
            line.SetPosition(i+1, jointNodes[i].transform.position);
        }
        line.SetPosition(11, jointNodes[9].GetComponents<DistanceJoint2D>()[0].connectedAnchor);

        for (int i = 0; i < jointNodes.Length; i++)
        {
            if (!line.GetPosition(i).Equals(jointNodes[i].transform.position))
            {
                Debug.Log("Bad");
            }
        }
    }
}
