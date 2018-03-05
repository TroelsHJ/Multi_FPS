using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerMovement : MonoBehaviour
{

    public float movementSpeed = 25f;
    public GameObject playerCamera;
    public float cameraSensitivity = 10f;
    public float cameraLimiter = 85f;

    private Rigidbody rb;
    private float currentCameraRotaion;
    //private Camera playerCamera;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        //playerCamera = GetComponentInChildren<Camera>();
    }


    void Update()
    {
        // moves the player 
        float movementX = Input.GetAxis("Horizontal");
        float movementY = Input.GetAxis("Vertical");
        transform.Translate(movementX * Time.deltaTime * movementSpeed, 0, movementY * Time.deltaTime * movementSpeed);

        // rotates the player
        float rotationX = Input.GetAxis("Mouse X");
        Vector3 yRotation = new Vector3(0, rotationX, 0) * cameraSensitivity;
        rb.MoveRotation(rb.rotation * Quaternion.Euler(yRotation));

        // rotates the player camera
        float rotationY = Input.GetAxis("Mouse Y");
        float cameraRotationX = rotationY * cameraSensitivity;

        currentCameraRotaion -= cameraRotationX;
        currentCameraRotaion = Mathf.Clamp(currentCameraRotaion, -cameraLimiter, cameraLimiter);

        playerCamera.transform.localEulerAngles = new Vector3(currentCameraRotaion, 0, 0);

    }

}
