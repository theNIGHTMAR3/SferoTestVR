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

        Vector3 playerMovement = (Camera.main.transform.right * playerInput.x + cameraForward * playerInput.y) * moveSpeed;

        Move(playerMovement);
    }

    protected override IEnumerator DieCoroutine()
    {
        Vector3 originalPosition = rigidbody.position;
        isRespawning = true;
        float elapsedTime = 0;
        StartCoroutine(FreezePlayer(respawnDuration));
        Vector3 finalPosition = new Vector3();
        while (elapsedTime < respawnDuration)
        {
            if (!isRespawning)
                break;

            float t = elapsedTime / respawnDuration;
            Vector3 newPos = Vector3.Lerp(originalPosition, originalPosition + Vector3.up * respawnHeight, t);
            rigidbody.position = newPos;
            finalPosition = newPos;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        while (isRespawning)
        {
            rigidbody.position = finalPosition;
            yield return null;
        }

        Revive();
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
