using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scale : Action
{
    Vector2 startScale;
    [SerializeField] public Vector2 newScale;
    [SerializeField] public bool scaleUsingCurve = false;
    [SerializeField] public AnimationCurve scaleCurve = AnimationCurve.Linear(0, 0, 1, 1);


    public Scale() : base(ActionType.SCALE)
    {
    }
    public Scale(float startTime, float duration) : base(ActionType.SCALE, startTime, duration) { }
    protected override void StartTransform()
    {
        startScale = objectToTransform.transform.localScale;
    }


    protected override void InterpolateTransform(float percents)
    {
        if (scaleUsingCurve)
            objectToTransform.transform.localScale = (Vector3)(Vector2.LerpUnclamped(startScale, newScale, scaleCurve.Evaluate(percents))) + new Vector3(0, 0, 1);
        else
            objectToTransform.transform.localScale = (Vector3)(Vector2.Lerp(startScale, newScale, percents)) + new Vector3(0, 0, 1);
    }

    protected override void SetEndTransform()
    {
        objectToTransform.transform.localScale = (Vector3)(newScale) + new Vector3(0, 0, 1);
    }
}
