using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class CameraFollower : MonoBehaviour
{
    public float sensitivity = 5f;

    private GameObject player;
   
    private Vector2 mouseRotation;
    

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
        transform.position = player.transform.position;

        mouseRotation.x += Input.GetAxis("Mouse X") * sensitivity;
        mouseRotation.y += Input.GetAxis("Mouse Y") * sensitivity;
        mouseRotation.y = Mathf.Clamp(mouseRotation.y, -90f, 90f);
        transform.localRotation = Quaternion.Euler(-mouseRotation.y, mouseRotation.x, 0);

    }
}
