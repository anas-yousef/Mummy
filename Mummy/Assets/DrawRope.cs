using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawRope : MonoBehaviour
{
    //[SerializeField] GameObject[] jointNodes = new GameObject[10];
    [SerializeField] private List<GameObject> jointNodes;
    LineRenderer line;
    // Start is called before the first frame update
    void Start()
    {
        //jointNodes
        line = GetComponent<LineRenderer>();
    }
    private void Update()
    {
        //line.positionCount = 12;
        line.positionCount = jointNodes.Count + 2;

        line.SetPosition(0, jointNodes[0].GetComponent<DistanceJoint2D>().connectedAnchor);
        for (int i = 0; i < jointNodes.Count; i++)
        {
            line.SetPosition(i+1, jointNodes[i].transform.position);
        }
        line.SetPosition(line.positionCount - 1, jointNodes[jointNodes.Count - 1].GetComponents<DistanceJoint2D>()[0].connectedAnchor);
    }
}
