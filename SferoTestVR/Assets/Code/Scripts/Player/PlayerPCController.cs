using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPCController : Player
{

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        Vector3 cameraForward= Camera.main.transform.forward;

        cameraForward.y = 0f;
        cameraForward.Normalize();

        Vector3 playerMovement = (Camera.main.transform.right * playerInput.x + cameraForward * playerInput.y) * moveSpeed;

        Move(playerMovement);
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
}
