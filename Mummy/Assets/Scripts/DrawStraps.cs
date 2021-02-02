using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawStraps : MonoBehaviour
{
    [SerializeField] private List<GameObject> jointNodesSide1;
    [SerializeField] private List<GameObject> jointNodesSide2;
    LineRenderer line;
    // Start is called before the first frame update
    void Start()
    {
        line = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        line.positionCount = jointNodesSide1.Count + 1;
        line.SetPosition(0, jointNodesSide1[0].GetComponent<DistanceJoint2D>().connectedAnchor);
        for (int i = 0; i < jointNodesSide1.Count; i++)
        {
            line.SetPosition(i + 1, jointNodesSide1[i].transform.position);
        }
        //line.SetPosition(line.positionCount - 1, jointNodesSide1[jointNodesSide1.Count - 1].GetComponents<DistanceJoint2D>()[0].connectedAnchor);

    }
}
