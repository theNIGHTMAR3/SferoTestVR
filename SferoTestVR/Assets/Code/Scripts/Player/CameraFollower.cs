using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class CameraFollower : MonoBehaviour
{
    public GameObject player;

    public float sensitivity = 5f;

    private Vector2 mouseRotation;
    
    public float xOffset, yOffset, zOffset;


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        // FPP camera
        if (xOffset == 0 && yOffset== 0 && zOffset == 0)
        {
            transform.position = player.transform.position;

            mouseRotation.x += Input.GetAxis("Mouse X") * sensitivity;
            mouseRotation.y += Input.GetAxis("Mouse Y") * sensitivity;
            transform.localRotation = Quaternion.Euler(-mouseRotation.y, mouseRotation.x, 0);    
        }
        // TPP camera
        else
        {
            transform.position = player.transform.position + new Vector3(xOffset, yOffset, zOffset);
            transform.LookAt(player.transform.position);
        }
    }
}
