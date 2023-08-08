using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Trigger : Action
{

    [SerializeReference] public Triggerable triggerable;

    public Trigger() : base(ActionType.TRIGERRABLE)
    {
    }

    public Trigger(float startTime) : base(ActionType.TRIGERRABLE, startTime,0) { }
    protected override void StartTransform()
    {
        triggerable.Trigger();
    }


}
