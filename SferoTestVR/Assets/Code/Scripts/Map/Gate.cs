using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour
{

    [SerializeField] private GameObject gateObject;
    [SerializeField] private GameObject gateCollider;    

    /// <summary>
    /// holds info whether the gate is opened. also sets the start value/position
    /// </summary>
    [SerializeField] private bool isOpenned = false;
    [SerializeField] private bool shouldBeOpened = true;
    [SerializeField] private float height = 2f;
    [SerializeField] private float openingTime = 0.5f;

    [SerializeField] private AnimationCurve positionCurve = new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 1));// = new AnimationCurve();

    /// <summary>
    /// lock for not changing state while opening or closing the gate
    /// </summary>
    private bool busy = false;


    [SerializeField] private bool doOnce = true;
    private bool wasChangedOnce = false;

    private void Update()
    {
        if (!busy)
        {
            if (isOpenned != shouldBeOpened)
            {
                if (isOpenned)
                {
                    StartCoroutine(Close());
                }
                else
                {
                    StartCoroutine(Open());
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!(doOnce && wasChangedOnce)) //if set allow only one change
            {
                shouldBeOpened = !shouldBeOpened;
                wasChangedOnce = true;
            }
        }
    }


    IEnumerator Open()
    {        
        busy = true;

        float startTime = Time.time;

        float lerpedY,t;
        while(Time.time < startTime + openingTime)
        {
            t=(Time.time - startTime)/openingTime;            
            lerpedY = Mathf.Lerp(0, height,positionCurve.Evaluate(t)) ;
            gateObject.transform.localPosition = new Vector3(0, lerpedY, 0);
            gateCollider.transform.localPosition = new Vector3(0, lerpedY, 0);
            yield return null;
        }

        gateObject.transform.localPosition = new Vector3(0, height, 0);

        busy = false;
        isOpenned = true;
    }

    IEnumerator Close()
    {
        gateCollider.transform.localPosition = Vector3.zero;
        busy = true;

        float startTime = Time.time;

        float lerpedY, t;
        while (Time.time < startTime + openingTime)
        {
            t = 1- (Time.time - startTime) / openingTime;
            lerpedY = Mathf.Lerp(0, height, positionCurve.Evaluate(t));
            gateObject.transform.localPosition = new Vector3(0, lerpedY, 0);
            yield return null;
        }

        gateObject.transform.localPosition = Vector3.zero;
        busy = false;
        isOpenned = false;
    }
}
