using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Move : Action
{

    Vector3 startPos;
    [SerializeField] public Vector3 newPos;
    [SerializeField] public bool posUsingCurve = false;
    [SerializeField] public AnimationCurve posCurve = AnimationCurve.Linear(0, 0, 1, 1);


    public Move() : base(ActionType.MOVE)
    {

    }


    public Move(float startTime, float duration) : base(ActionType.MOVE, startTime, duration) { }    

    protected override void StartTransform()
    {
        startPos = objectToTransform.transform.localPosition;
    }


    protected override void InterpolateTransform(float percents)
    {
        if (posUsingCurve)
            objectToTransform.transform.localPosition = Vector3.LerpUnclamped(startPos, newPos, posCurve.Evaluate(percents));
        else
            objectToTransform.transform.localPosition = Vector3.Lerp(startPos, newPos, percents);
    }

    protected override void SetEndTransform()
    {
        objectToTransform.transform.localPosition = newPos;
    }    
}
