using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawStraps : MonoBehaviour
{
    [SerializeField] private List<GameObject> jointNodes;
    private int length;
    LineRenderer line;
    // Start is called before the first frame update
    void Start()
    {
        line = GetComponent<LineRenderer>();
        length = jointNodes.Count;
    }

    // Update is called once per frame
    void Update()
    {
        //line.positionCount = jointNodesSide1.Count;
        line.positionCount = length;

        for (int i = 0; i < length; i++)
        {
            line.SetPosition(i, jointNodes[i].transform.position);
        }

    }
}
