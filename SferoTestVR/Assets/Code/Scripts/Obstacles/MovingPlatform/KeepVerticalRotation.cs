using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// in order to work properly these things must be set up:
/// rigidbody of platform must have all positions freezed checked
/// </summary>
public class KeepVerticalRotation : MonoBehaviour
{
    [Range(0.2f, 5f)]
    [SerializeField] float strength = 0.2f;    

    Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        //rb.constraints = RigidbodyConstraints.FreezePosition; // in case us forgot to set freezed positions!
    }
        

    private void FixedUpdate()
    {
        //rb.MoveRotation(Quaternion.identity);        
        // rb.MoveRotation(Quaternion.Lerp(transform.rotation, Quaternion.identity, strength));
        Vector3 actRot = transform.rotation.eulerAngles;
        Vector3 reverseTorque;
        reverseTorque.x = Mathf.DeltaAngle(actRot.x, 0);
        reverseTorque.y = Mathf.DeltaAngle(actRot.y, 0);
        reverseTorque.z = Mathf.DeltaAngle(actRot.z, 0);
        reverseTorque *= strength;
        rb.AddTorque(reverseTorque);
    }

}

