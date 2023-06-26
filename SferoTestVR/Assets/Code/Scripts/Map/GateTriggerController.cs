using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateTriggerController : MonoBehaviour
{
    [SerializeField] private bool isOpened = false;

    [SerializeField] private Animator myGate = null;

    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Player"))
        {
            if (isOpened)
            {
                //myGate.Play("GateClose", 0, 0.0f);
                myGate.SetBool("isOpened", false);
                isOpened = false;
            }
            else
            {
                //myGate.Play("GateOpen", 0, 0.0f);
                myGate.SetBool("isOpened", true);
                isOpened = true;
            }
        }
    }
}
