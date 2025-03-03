using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ControlZone : MonoBehaviour
{
    protected Player player;

    private void FixedUpdate()
    {
        if (player != null)
        {
            SetPlayerVelocity();
        }
    }

    abstract protected void SetPlayerVelocity();
    virtual protected void OnPlayerEnter() { }
    virtual protected void OnPlayerExit() { }

    private void OnTriggerEnter(Collider other)
    {
        Player player = other.GetComponent<Player>();
        if (player != null)
        {
            this.player = player;
            player.SetPlayerControlsSelf(false);
            OnPlayerEnter();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Player player = other.GetComponent<Player>();
        if (player != null)
        {
            OnPlayerExit();
            player.SetPlayerControlsSelf(true);
            this.player = null;
        }
    }
}
