using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public Player player;

    private bool used = false;


    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && used == false)
        {
            other.gameObject.GetComponent<Player>().SetNewCheckPoint(gameObject);
            used = true;
        }
        
    }
}
