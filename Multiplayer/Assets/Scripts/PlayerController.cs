using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerController : MonoBehaviour
{
    public GameObject visorCamera;


    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        float movementX = Input.GetAxis("Horizontal");
        float movementY = Input.GetAxis("Vertical");
        transform.Translate(movementX, 0, movementY);

        float rotationX = Input.GetAxis("Mouse X");
        float rotationY = Input.GetAxis("Mouse Y");

        transform.Rotate(0, rotationX * Time.deltaTime * 150f, 0);

        visorCamera.transform.Rotate(-rotationY * Time.deltaTime * 150f, 0, 0);

    }

}
