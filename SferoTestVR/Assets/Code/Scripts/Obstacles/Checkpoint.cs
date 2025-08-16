using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public Player player;

    private bool used = false;

    // define event when players set checkpoint
    public delegate void CheckpointDelegate(Checkpoint checkPoint);
    static public event CheckpointDelegate OnCheckPointCollision;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && used == false)
        {
            other.gameObject.GetComponent<Player>().SetNewCheckPoint(gameObject);
            used = true;
        }

        if (other.CompareTag("Player"))
        {
            // If anything is connected call all listeners.
            OnCheckPointCollision?.Invoke(this);            
        }

    }
}
