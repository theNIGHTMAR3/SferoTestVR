using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeActivated : MonoBehaviour
{

    [SerializeField] float activationTime;
    [SerializeField] float duration = 0.5f;

    private bool activated = false;
    private bool disactivated = false;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        
    }

    // Update is called once per frame
    protected void Update()
    {
        if (!activated)
        {
            if (Time.time > activationTime)
            {
                OnStartActivate();
            }
            else
            {
                Inactive();
            }
        }
        else
        {
            if (!disactivated)
            {
                if (Time.time > activationTime + duration)
                {
                    OnStopActivate();
                }
                else
                {
                    Active();
                }
            }
        }
    }

    protected virtual void Active()
    {

    }

    protected virtual void Inactive()
    {

    }

    protected virtual void OnStartActivate()
    {
        activated = true;
    }
    protected virtual void OnStopActivate()
    {
        activated = false;
        disactivated= true;
    }
}
