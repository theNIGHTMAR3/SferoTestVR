using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpandingPath : FourActionStateMachine
{
    [SerializeField] GameObject[] platforms;
    [SerializeField] float platformOffset=1;
    [SerializeField] float extendingTime=1;
    [SerializeField] float stayTime=1;
    [SerializeField] float scaleDif=0.05f;    

    protected override void Start()
    {
        for (int i = 0; i < platforms.Length; i++)
        {
            platforms[i].transform.localPosition = Vector3.zero;
            platforms[i].transform.localScale = Vector3.one * (float)(1f - scaleDif * (i+1));            
        }


        //set timers using human readeable names
        action1Time = stayTime; //waiting to extend
        action2Time = extendingTime; //extending
        action3Time = stayTime; //waiting to shorten
        action4Time = extendingTime; //shortening

        base.Start();
    }

    /// <summary>
    /// sets position of child platforms
    /// 0 - completely hidden
    /// 1 - fully extended
    /// </summary>
    /// <param name="percentage"></param>
    void SetPosition(float percentage)
    {
        for (int i = 0; i < platforms.Length; i++)
        {
            Vector3 pos = platforms[i].transform.localPosition;
            pos.x = platformOffset * (i + 1) * percentage;
            platforms[i].transform.localPosition = pos;
        }
    }

    //waiting to extend
    protected override void Action1(float percentage)
    {
        SetPosition(0);
    }

    //extending
    protected override void Action2(float percentage)
    {
        SetPosition(percentage);
    }

    //waiting to shorten
    protected override void Action3(float percentage)
    {
        SetPosition(1);
    }

    //shortening
    protected override void Action4(float percentage)
    {
        SetPosition(1-percentage);
    }
}
