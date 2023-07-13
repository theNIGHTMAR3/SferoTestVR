using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPCController : MonoBehaviour
{
    public GameObject playerObject;

    public float moveSpeed = 10f;
    public float sensitivity = 5f;

    private Rigidbody playerRigidbody;
    private Camera playerCamera;

    private float xInput;
    private float zInput;

    private Vector2 mouseRotation;


    // Start is called before the first frame update
    void Start()
    {
        playerObject = GameObject.FindGameObjectWithTag("Player");
        playerCamera = playerObject.GetComponentInChildren<Camera>(); 
        playerRigidbody= playerObject.GetComponentInChildren<Rigidbody>(); 

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        xInput = Input.GetAxis("Horizontal");
        zInput = Input.GetAxis("Vertical");

        playerCamera.transform.position = playerRigidbody.transform.position;

        mouseRotation.x += Input.GetAxis("Mouse X") * sensitivity;
        mouseRotation.y += Input.GetAxis("Mouse Y") * sensitivity;
        playerCamera.transform.localRotation = Quaternion.Euler(-mouseRotation.y, mouseRotation.x, 0);
    }

    private void FixedUpdate()
    {
        Vector3 cameraForward= playerCamera.transform.forward;

        cameraForward.y = 0f;
        cameraForward.Normalize();

        Vector3 playerMovement = (cameraForward * zInput + playerCamera.transform.right * xInput) * moveSpeed;


        playerRigidbody.AddForce(playerMovement);
    }
}
