using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public Player player;

    private bool used = false;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && used == false)
        {
            player.SetNewCheckPoint(gameObject);
            used = true;
        }
        
    }
}
