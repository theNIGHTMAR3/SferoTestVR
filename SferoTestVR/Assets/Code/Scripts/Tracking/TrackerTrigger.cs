using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackerTrigger : MonoBehaviour
{
    [SerializeField] Tracker tracker;
    [SerializeField] bool startSignal = true;


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (startSignal)
            {
                tracker.StartTracking();
            }
            else
            {
                tracker.StopTracking();
            }
        }
    }
}
