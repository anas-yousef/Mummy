using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpeinigObjectScript : MonoBehaviour
{
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
        if (firstBlockPosition.position.x > 7 && firstBlockPosition.position.x < 7.2)
        {
            if(secondBlockPosition.position.y<-4.4 && secondBlockPosition.position.y > -4.43)
            {
                return true;
            }
        }
        else if (secondBlockPosition.position.x > 7 && secondBlockPosition.position.x < 7.2)
        {
            if (firstBlockPosition.position.y > -4.4 && firstBlockPosition.position.y < -4.3)
            {
                return true;
            }
        }
        return false;
    }
}
