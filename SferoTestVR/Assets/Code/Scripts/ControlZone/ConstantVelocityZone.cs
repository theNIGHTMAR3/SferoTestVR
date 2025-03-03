using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstantVelocityZone : ControlZoneTest
{
    [SerializeField] float multiplier = 0.8f;
    Vector2 speed;
    protected override void OnPlayerEnter()
    {
        base.OnPlayerEnter();
        speed = player.GetRigidBody2DVelocity() * multiplier;
    }

    protected override void SetPlayerVelocity()
    {
        player.SetRotationSpeed(speed);
    }
}
