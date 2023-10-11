using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ActionQueue : Triggerable
{
    [SerializeField] public bool looped;
    [SerializeField] public float loopDuration = 5;
    [Tooltip("Number of repetitions. 0 means infinity")]
    [SerializeField] public int loopCount = 2;
    [SerializeReference] public List<Action> actions = new List<Action>(); 

    [SerializeField] public bool activatedByTrigger = false;

    [SerializeField] private float offset = 0 ;
    private int loopIteration = -1;
    private float startTime;            


    protected bool started = false;
    protected bool finished = false;    

    // Start is called before the first frame update
    void Start()
    {
        foreach(Action action in actions)
        {
            action.objectToTransform = this.gameObject;            
        }

        
        if (!activatedByTrigger)
        {
            started = true;
            startTime = 0;
        }
    }

    override public void Trigger()
    {        

        started = true;
        finished = false;
        startTime = Time.time;
        loopIteration = -1;
        foreach (Action action in actions)
        {
            action.ResetAction();
        }
    }

    // Update is called once per frame
    protected void Update()
    {
        //Debug.Log(gameObject.name + " " + finished);
        if (started && !finished)
        {
            if (looped)
            {
                int loop = (int)(Mathf.Floor((Time.time - startTime) / loopDuration));
                if ((loop != loopIteration && loop < loopCount) || loopCount == 0)
                {                    
                    loopIteration = loop;                    
                    foreach (Action action in actions)
                    {
                        action.ResetAction();
                    }
                }
                else if(loop  >= loopCount)
                {
                    finished = true;
                }
            }
            if (!finished)
            {
                //check all actions
                foreach (Action action in actions)
                {
                    action.CheckSelf(looped ? (Time.time - startTime + offset) % loopDuration : Time.time - startTime + offset);
                    
                }
            }
        }
    }
    
}