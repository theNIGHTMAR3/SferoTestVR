﻿using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[System.Serializable]
public class Action
{
    public enum ActionType
    {
        MOVE,
        ROTATE,
        SCALE,
        TRIGERRABLE

    }

    public GameObject objectToTransform;

    /*
                            Vector2 startPos;
    [SerializeField] public Vector2 newPos;
    [SerializeField] public bool posUsingCurve = false;
    [SerializeField] public AnimationCurve posCurve = AnimationCurve.Linear(0, 0, 1, 1);
    */

    [SerializeField] public ActionType type;
    [SerializeField]
    [Range(0f, float.PositiveInfinity)]
    public float startTime;
    [SerializeField] 
    [Range(0f, float.PositiveInfinity)]
    public float duration;                                    

    protected bool started = false;
    protected bool finished = false;



    public Action(ActionType type)
    {
        this.type = type;
    }

    public Action(ActionType type, float startTime, float duration) : this(type)
    {
        this.startTime = startTime;
        this.duration = duration;
    }

    public virtual void ResetAction()
    {
        finished = false;
        started = false;
    }

    public void CheckSelf(float time)
    {
        if (!finished)
        {
            if(time > startTime)
            {
                if (!started)
                {
                    started = true;
                    StartTransform();
                }
                if (time < startTime + duration && duration != 0)
                    InterpolateTransform((time - startTime)/duration);
                

                if (time >= startTime + duration)
                {
                    finished = true;                    
                    SetEndTransform();
                }
            }
        }
    }

    virtual protected void StartTransform()
    {
    }


    virtual protected void InterpolateTransform(float percents)
    {

    }
    virtual protected void SetEndTransform()
    {

    }

}
