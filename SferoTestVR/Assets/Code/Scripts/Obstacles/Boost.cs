using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boost : MonoBehaviour
{
    [SerializeField] float force = 5f;

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {            
            other.gameObject.GetComponent<Rigidbody>().AddForce(transform.forward * force);            
        }
    }
    
}
