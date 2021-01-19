using System;
using UnityEngine;

public class ToiletPaper : MonoBehaviour
{
    [Header("Settings")]
    public float speed = 5f; // default speed 1 unit / second
    public float distance = 5f; // default distance 5 units
    public Transform toiletPaper; // the object you want to throw (assign from the scene)
    private float _distance; // the distance it moves
    private bool _back; // is it coming back

    public void Throw ()
    {
        _distance = 0; 
        _back = false; 
        enabled = true;
    }
 
    private void Update ()
    {
        float travel = Time.deltaTime * speed;
        if (!_back)
        {
            // TODO need to add the option to throw 135 degrees. 
            
            toiletPaper.Translate(new Vector3(1, 1, 0) * travel); // moves object
            _distance += travel; // update distance
            _back = _distance >= distance; // goes back if distance reached
        }
        else
        {
            toiletPaper.Translate(new Vector3(1, 1, 0) * -travel); // moves object
            _distance -= travel; // update distance;
            enabled = _distance > 0; // turning off when done
        }
    }
 
    private void OnEnable ()
    {
        toiletPaper.gameObject.SetActive (true); // activating the object
    }
    private void OnDisable ()
    {
        toiletPaper.gameObject.SetActive (false);
    }

    public void StopThrow()
    {
        transform.localPosition = Vector3.zero;
    }
}
