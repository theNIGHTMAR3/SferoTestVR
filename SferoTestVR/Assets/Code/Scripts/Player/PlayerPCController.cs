using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPCController : Player
{

    private void FixedUpdate()
    {
        Vector3 cameraForward= Camera.main.transform.forward;

        cameraForward.y = 0f;
        cameraForward.Normalize();

        Vector3 playerMovement = (cameraForward * playerInput.y + Camera.main.transform.right * playerInput.x) * moveSpeed;

        Move(playerMovement);
    }


    protected override void GetInput()
    {
        playerInput.x = Input.GetAxis("Horizontal");
        playerInput.y = Input.GetAxis("Vertical");
    }

}
