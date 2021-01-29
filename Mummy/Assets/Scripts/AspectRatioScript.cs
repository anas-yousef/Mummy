using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AspectRatioScript : MonoBehaviour
{

    public float targetWidth = 1920;
    public float targetHeight = 1080;
    public Camera camera;
    public Transform bg; 
    
    void Start()
    {
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
        }
        
        bg.localScale = new Vector3(1, 1, 1);
    }
}
