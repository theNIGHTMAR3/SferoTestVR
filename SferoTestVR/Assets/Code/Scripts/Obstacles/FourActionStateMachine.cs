using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FourActionStateMachine : MonoBehaviour
{
    [SerializeField] float delay;

    [SerializeField] MachineState machineState = MachineState.ACTION1;
    float startTime = 0; // one variable to track "timers"

    protected float action1Time = 1f;
    protected float action2Time = 1f;
    protected float action3Time = 1f;
    protected float action4Time = 1f;

    protected float sequenceTime=0;

    public enum MachineState
    {
        ACTION1,
        ACTION2,
        ACTION3,
        ACTION4
        
    }

    /// <summary>
    /// when overriided it's called at the end of start
    /// </summary>
    virtual protected void Start()
    {
        sequenceTime = action1Time + action2Time + action3Time + action4Time;
    }


    // Update is called once per frame
    void Update()
    {
        float percentage;
        float delayedTime = Time.time - delay;
        float moduloTime = delayedTime % sequenceTime;
        switch (machineState)
        {
            case MachineState.ACTION1:
                if (moduloTime > action1Time) //if ended
                {
                    machineState = MachineState.ACTION2;                    
                    OnAction2Start();
                }
                else
                {
                    percentage = (moduloTime) / (action1Time);
                    Action1(percentage);
                }
                break;
            case MachineState.ACTION2:
                if (moduloTime > action1Time + action2Time)//if ended
                {
                    machineState = MachineState.ACTION3;                    
                    OnAction3Start();
                }
                else
                {
                    percentage = (moduloTime - action1Time) / (action2Time);
                    Action2(percentage);
                }
                break;
            case MachineState.ACTION3:
                if (moduloTime > action1Time + action2Time + action3Time) //if ended
                {
                    machineState = MachineState.ACTION4;                    
                    OnAction4Start();
                }
                else
                {
                    percentage = (moduloTime - action1Time - action2Time) / (action3Time);
                    Action3(percentage);
                }
                break;
            case MachineState.ACTION4:
                if (moduloTime < action1Time) //if ended
                {
                    machineState = MachineState.ACTION1;                    
                    OnAction1Start();
                }
                else
                {
                    percentage = (moduloTime - action1Time - action2Time - action3Time) / (action4Time);
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
