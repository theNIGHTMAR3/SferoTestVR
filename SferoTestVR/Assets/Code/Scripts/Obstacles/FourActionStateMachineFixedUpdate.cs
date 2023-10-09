using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FourActionStateMachineFixedUpdate : MonoBehaviour
{
    [SerializeField] float delay;

    [SerializeField] MachineState machineState = MachineState.ACTION1;
    float startTime = 0; // one variable to track "timers"

    protected float action1Time = 1f;
    protected float action2Time = 1f;
    protected float action3Time = 1f;
    protected float action4Time = 1f;

    public enum MachineState
    {
        ACTION1,
        ACTION2,
        ACTION3,
        ACTION4

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float percentage;
        float delayedTime = Time.time - delay;
        switch (machineState)
        {
            case MachineState.ACTION1:
                if (delayedTime > startTime + action1Time) //if ended
                {
                    machineState = MachineState.ACTION2;
                    startTime = delayedTime;
                    OnAction2Start();
                }
                else
                {
                    percentage = (delayedTime - startTime) / (action1Time);
                    Action1(percentage);
                }
                break;
            case MachineState.ACTION2:
                if (delayedTime > startTime + action2Time)//if ended
                {
                    machineState = MachineState.ACTION3;
                    startTime = delayedTime;
                    OnAction3Start();
                }
                else
                {
                    percentage = (delayedTime - startTime) / (action2Time);
                    Action2(percentage);
                }
                break;
            case MachineState.ACTION3:
                if (delayedTime > startTime + action3Time) //if ended
                {
                    machineState = MachineState.ACTION4;
                    startTime = delayedTime;
                    OnAction4Start();
                }
                else
                {
                    percentage = (delayedTime - startTime) / (action3Time);
                    Action3(percentage);
                }
                break;
            case MachineState.ACTION4:
                if (delayedTime > startTime + action4Time) //if ended
                {
                    machineState = MachineState.ACTION1;
                    startTime = delayedTime;
                    OnAction1Start();
                }
                else
                {
                    percentage = (delayedTime - startTime) / (action4Time);
                    Action4(percentage);
                }
                break;
        }
    }

    virtual protected void OnAction1Start()
    {

    }
    virtual protected void Action1(float percentage)
    {

    }
    virtual protected void OnAction2Start()
    {

    }
    virtual protected void Action2(float percentage)
    {

    }
    virtual protected void OnAction3Start()
    {

    }
    virtual protected void Action3(float percentage)
    {

    }
    virtual protected void OnAction4Start()
    {

    }
    virtual protected void Action4(float percentage)
    {

    }
}
