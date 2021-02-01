using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpeinigObjectScript : MonoBehaviour
{
    private Vector3 firstPos=new Vector3(-3.775977f,-4.437042f,0);
    private Vector3 secondPos=new Vector3(4.247139f,-4.438741f,0);
    [SerializeField] private Transform firstBlockPosition;
    [SerializeField] private Transform secondBlockPosition;
    public GameObject openingObject;
    void Update()
    {
        if (checkplace())
        {
            openingObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 2, gameObject.transform.position.z);
        }
    }

    private bool checkplace()
    {
        if (Vector3.Distance(firstBlockPosition.transform.position,firstPos)<=0.3)
        {
            if(Vector3.Distance(secondBlockPosition.transform.position, secondPos) <= 0.3)
            {
                return true;
            }
        }
        else if (Vector3.Distance(secondBlockPosition.transform.position, firstPos) <= 0.3)
        {
            if (Vector3.Distance(firstBlockPosition.transform.position, secondPos) <= 0.3)
            {
                return true;
            }
        }
        return false;
    }
}
