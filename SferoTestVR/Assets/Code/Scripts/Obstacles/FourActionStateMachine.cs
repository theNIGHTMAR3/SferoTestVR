using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FourActionStateMachine : MonoBehaviour
{
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
    void Update()
    {
        float percentage;
        switch (machineState)
        {
            case MachineState.ACTION1:
                if (Time.time > startTime + action1Time) //if ended
                {
                    machineState = MachineState.ACTION2;
                    startTime = Time.time;
                    OnAction2Start();
                }
                else
                {
                    percentage = (Time.time - startTime) / (action1Time);
                    Action1(percentage);
                }
                break;
            case MachineState.ACTION2:
                if (Time.time > startTime + action2Time)//if ended
                {
                    machineState = MachineState.ACTION3;
                    startTime = Time.time;
                    OnAction3Start();
                }
                else
                {
                    percentage = (Time.time - startTime) / (action2Time);
                    Action2(percentage);
                }
                break;
            case MachineState.ACTION3:
                if (Time.time > startTime + action3Time) //if ended
                {
                    machineState = MachineState.ACTION4;
                    startTime = Time.time;
                    OnAction4Start();
                }
                else
                {
                    percentage = (Time.time - startTime) / (action3Time);
                    Action3(percentage);
                }
                break;
            case MachineState.ACTION4:
                if (Time.time > startTime + action4Time) //if ended
                {
                    machineState = MachineState.ACTION1;
                    startTime = Time.time;
                    OnAction1Start();
                }
                else
                {
                    percentage = (Time.time - startTime) / (action4Time);
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
