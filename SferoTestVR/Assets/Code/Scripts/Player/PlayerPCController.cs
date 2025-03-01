using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerPCController : Player
{

	[SerializeField] protected CameraFollower cameraScript;


	protected override void FixedUpdate()
    {
        base.FixedUpdate();
        if (playerControlsSelf)
        {
            Vector3 cameraForward = Camera.main.transform.forward;

            cameraForward.y = 0f;
            cameraForward.Normalize();

            Vector3 playerMovement = (Camera.main.transform.right * playerInput.x + cameraForward * playerInput.y);

            Move(playerMovement);
        }
        else
        {

        }
    }


    protected override void GetInput()
    {
        playerInput.x = Input.GetAxis("Horizontal");
        playerInput.y = Input.GetAxis("Vertical");
    }

    protected override void Revive()
    {
        cameraScript.SetCameraRotation(savedRotation);
        base.Revive();
    }

	/// <summary>
	/// moves user to Main Menu
	/// </summary>    
	protected override void GoBackToMainMenu()
	{
		cameraScript.UnlockCursor();
		SceneManager.LoadScene("MainMenu");
	}

}
