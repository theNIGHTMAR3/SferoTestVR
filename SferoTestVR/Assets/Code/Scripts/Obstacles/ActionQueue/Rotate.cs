using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Rotate : Action
{
    float startRot;
    [SerializeField] public float newRot;
    [SerializeField] public bool rotIsOffset;
    [SerializeField] public bool rotUsingCurve = false;
    [SerializeField] public AnimationCurve rotCurve = AnimationCurve.Linear(0, 0, 1, 1);

    public Rotate() : base(ActionType.ROTATE)
    {
    }
    public Rotate(float startTime, float duration) : base(ActionType.ROTATE, startTime, duration) { }

    protected override void StartTransform()
    {
        startRot = objectToTransform.transform.eulerAngles.z;
    }


    protected override void InterpolateTransform(float percents)
    {
        float targetRot = rotIsOffset ? startRot + newRot : newRot;
        if (rotUsingCurve)
            objectToTransform.transform.eulerAngles = new Vector3(0, 0, Mathf.LerpUnclamped(startRot, targetRot, rotCurve.Evaluate(percents)));
        else
            objectToTransform.transform.eulerAngles = new Vector3(0, 0, Mathf.Lerp(startRot, targetRot, percents));
    }

    protected override void SetEndTransform()
    {
        float targetRot = rotIsOffset ? startRot + newRot : newRot;
        objectToTransform.transform.eulerAngles = new Vector3(0, 0, targetRot);
    }
}
