using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class CameraFollower : MonoBehaviour
{

    private GameObject player;
   
    private Vector2 mouseRotation;

    private float sensitivity;
    

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        SetSensitivity();

        LockCursor();
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

	/// <summary>
	/// sets player sensitivity at the start of the game
	/// </summary>
	private void SetSensitivity()
	{
		
        if(!PlayerPrefs.HasKey("Sensitivity"))
        {
			Debug.Log("PlayerPrefs: sensitivity not set, setting it to 1.0");
			PlayerPrefs.SetFloat("Sensitivity", 1.0f);
			PlayerPrefs.Save();
		}
        else
        {
			Debug.Log("Sensitivity: "+ sensitivity);
		}
		sensitivity = PlayerPrefs.GetFloat("Sensitivity");
	}

	/// <summary>
	/// restore camera rotation relevant to checkpoint 
	/// </summary>
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
