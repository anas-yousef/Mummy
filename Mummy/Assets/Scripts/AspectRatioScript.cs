using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AspectRatioScript : MonoBehaviour
{

    public float targetWidth = 1920;
    public float targetHeight = 1080;
    public Camera camera;
    public Transform bg;
    public Transform main;
    public Transform[] pannels = new Transform[5];
    
    void Start()
    {
        float screenH = Screen.height;
        float windowAspect = (float) Screen.width / (float) Screen.height;
        float targetAspect = targetWidth / targetHeight;
        float scaleHeigth = windowAspect / targetAspect;
        
        if (windowAspect < targetAspect)
        {
            camera.orthographicSize = (targetHeight / 200f) / scaleHeigth;

        }
        else
        {
            camera.orthographicSize = targetHeight / 200f;
            pannels[0].localScale = new Vector3((float) targetHeight / screenH, targetHeight / screenH, 1) ;
            pannels[1].localScale = new Vector3((float) targetHeight / screenH, targetHeight / screenH, 1) ;
            pannels[2].localScale = new Vector3((float) targetHeight / screenH, targetHeight / screenH, 1) ;
            pannels[3].localScale = new Vector3((float) targetHeight / screenH, targetHeight / screenH, 1) ;
            pannels[4].localScale = new Vector3((float) targetHeight / screenH, targetHeight / screenH, 1) ;

        }
        
        bg.localScale = new Vector3(1, 1, 1);

    }
}
