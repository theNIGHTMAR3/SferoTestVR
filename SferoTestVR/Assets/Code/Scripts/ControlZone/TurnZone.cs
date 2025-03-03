using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Assumes that the turn is a fragment of a circle.
/// Uses Forward vector of start and end Directions!
/// Those vectors are the tangent to the circle!
/// </summary>
public class TurnZone : ControlZone
{
    [SerializeField]
    Transform startDirection;

    [SerializeField]
    Transform endDirection;

    float turnRadius = 0;
    float speed = 0;
    private void Start()
    {
        float chordLength = Vector3.Distance(startDirection.position, endDirection.position);
        float angleDegreesBetweenDirections = 
            Vector3.SignedAngle(
                startDirection.forward, 
                endDirection.forward,
                Vector3.up);
        turnRadius = chordLength / (2 * Mathf.Sin(angleDegreesBetweenDirections / 2 * Mathf.Deg2Rad));
    }

    protected override void SetPlayerVelocity()
    {
        float angleDegreesBetweenStartAndPlayer =
            Vector3.SignedAngle(
                startDirection.forward,
                player.transform.position - startDirection.position,
                Vector3.up);
        Vector3 playerMoveDirection = 
            Quaternion.Euler(0, angleDegreesBetweenStartAndPlayer, 0)
            * startDirection.forward;
        player.SetRotationSpeed(playerMoveDirection);
    }

    protected override void OnPlayerEnter()
    {
        base.OnPlayerEnter();
        speed = player.GetRigidBody2DVelocity().magnitude;
    }
}
