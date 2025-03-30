using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwingingBridge : MonoBehaviour
{

    [SerializeField] float swingAngle = 45f;
    [SerializeField] float swingFrequency = 0.5f;

    [SerializeField] Transform bridgeElementsParent;


    Quaternion startRot;
    float bridgeLength = 0;
    Vector3 bridgeNormal;

    private void Start()
    {
        startRot = transform.localRotation;
        var firstElement = bridgeElementsParent.GetChild(0);
        var lastElement = bridgeElementsParent.GetChild(bridgeElementsParent.childCount - 1);
        bridgeLength = Vector3.Distance(
            firstElement.localPosition, // first
            lastElement.localPosition // last
            );

        bridgeNormal = lastElement.localPosition - firstElement.localPosition;     
    }

    // Update is called once per frame
    void Update()
    {
        var firstElement = bridgeElementsParent.GetChild(0);

        foreach (Transform element in bridgeElementsParent)
        {
            var projected = Vector3.Project(
                element.localPosition - firstElement.localPosition
                , bridgeNormal
                );
            var t = projected.magnitude / bridgeLength;
            var intensity = t <= 0.5f ? 2*t : 2*(1 - t);

            element.localRotation = startRot * Quaternion.Euler(
            0,
            0,
            Mathf.Sin(Time.time * swingFrequency) * swingAngle * intensity
            );
        }
    }
}
