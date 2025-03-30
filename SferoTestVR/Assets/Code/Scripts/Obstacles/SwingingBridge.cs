using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwingingBridge : MonoBehaviour
{

    [SerializeField] float swingAngle = 45f;
    [SerializeField] float swingFrequency = 0.5f;

    Quaternion startRot;

    private void Start()
    {
        startRot = transform.localRotation; 
    }

    // Update is called once per frame
    void Update()
    {
        transform.localRotation = startRot * Quaternion.Euler(
            0,
            0,
            Mathf.Sin(Time.time * swingFrequency) * swingAngle
            );
    }
}
