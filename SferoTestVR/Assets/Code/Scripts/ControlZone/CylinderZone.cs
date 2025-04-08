using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CylinderZone : ControlZone
{
    /// <summary>
    /// Referently to player
    /// </summary>
    public float cylinderSpeed = 0.1f;
    public GameObject cylinderObject;

    /// <summary>
    /// Read from the scale. Cylinder primitive is used for scale reference!
    /// </summary>
    float cylinderRadius = 1f;

    float rotationAngleSpeed;
    Vector3 right;

    private void Start()
    {
        cylinderRadius = cylinderObject.transform.lossyScale.x/2;
        rotationAngleSpeed = 
            (Player.Instance.moveSpeed * cylinderSpeed) // relative speed to players speed
            / cylinderRadius // to convert radial velocity
            * Mathf.Rad2Deg // convert to degrees
            / Mathf.PI; // no idea why, but the rotation looks ok with it????
        right = transform.right;
    }

    private void Update()
    {
        cylinderObject.transform.localRotation *= Quaternion.Euler(0,rotationAngleSpeed*Time.deltaTime,0);
    }

    protected override void MoveToTarget()
    {
        if (targetPoint == startPoint)
        {
            base.MoveToTarget();
        }
        else
        {
            Vector3 cylinderInfluence = right * cylinderSpeed;
            Vector3 direction = (endPoint.transform.position - startPoint.transform.position).normalized;


            Vector3 sum = cylinderInfluence + (direction * (moveSpeed - cylinderInfluence.magnitude));

            Vector3 offsetFromStart = CastPointOnLine(
                    startPoint.transform.position,
                    endPoint.transform.position,
                    playerController.transform.position
                );
            playerRb.MovePosition(startPoint.transform.position + offsetFromStart);
            playerController.Move(sum.normalized, moveSpeed);
        }
    }


    /// <summary>
    ///       .B (End)
    ///     D/
    ///     /\
    ///    /___C (point)
    ///  A. (start)
    /// 
    /// To get the point D, we project the AC vector on AB
    /// </summary>
    /// <returns></returns>
    Vector3 CastPointOnLine(Vector3 lineStart, Vector3 lineEnd, Vector3 point)
    {
        return Vector3.ProjectOnPlane(point - lineStart, Quaternion.Euler(0, 90, 0) * (lineEnd - lineStart));
    }
}
