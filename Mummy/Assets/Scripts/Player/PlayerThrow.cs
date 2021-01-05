using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerThrow : MonoBehaviour
{
    [Header("Settings")]
    public ToiletPaper toiletPaper;

    private void Start()
    {
        toiletPaper.gameObject.SetActive(false);
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire1") && !toiletPaper.gameObject.activeSelf) // if the toilet paper is not away 
        {
            Debug.Log("Should throw");
            toiletPaper.Throw();
            toiletPaper.gameObject.SetActive(true);
        }
    }
}
