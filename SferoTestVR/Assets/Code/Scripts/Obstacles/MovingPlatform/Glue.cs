using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Glues Player to the object, until it leaves it colliders
/// </summary>
public class Glue : MonoBehaviour
{
    Rigidbody player;
    Vector3 relativePos = Vector3.zero;

    private void Update()
    {
        if (player != null)
        {
            player.transform.position = transform.position + relativePos;
        }
    }



    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.transform.SetParent(transform.parent.transform);
            Debug.Log("GLUED");
            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.transform.SetParent(null, true);
        }
    }
}
