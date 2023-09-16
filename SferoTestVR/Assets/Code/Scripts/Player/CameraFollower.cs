using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class CameraFollower : MonoBehaviour
{

    private GameObject player;
   
    private Vector2 mouseRotation;
    

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Sensitivity: "+PlayerPrefs.GetFloat("Sensitivity"));
        player = GameObject.FindGameObjectWithTag("Player");

        LockCursor();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = player.transform.position;

        mouseRotation.x += Input.GetAxis("Mouse X") * PlayerPrefs.GetFloat("Sensitivity");
        mouseRotation.y += Input.GetAxis("Mouse Y") * PlayerPrefs.GetFloat("Sensitivity");
        mouseRotation.y = Mathf.Clamp(mouseRotation.y, -90f, 90f);
        transform.localRotation = Quaternion.Euler(-mouseRotation.y, mouseRotation.x, 0);

    }

    public void SetCameraRotation(Quaternion checkpointRotation)
    {
        mouseRotation.x = checkpointRotation.eulerAngles.y;
        mouseRotation.y = 0;
    }


    /// <summary>
    /// locks player cursor and makes it invisible
    /// </summary>
    public void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    /// <summary>
    /// unlocks player cursor and makes it visible
    /// </summary>
    public void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

}
