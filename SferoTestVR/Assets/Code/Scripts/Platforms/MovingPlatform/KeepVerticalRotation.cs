using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeepVerticalRotation : MonoBehaviour
{
    [Range(0.2f, 1f)]
    [SerializeField] float strength = 0.1f;    

    Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
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

