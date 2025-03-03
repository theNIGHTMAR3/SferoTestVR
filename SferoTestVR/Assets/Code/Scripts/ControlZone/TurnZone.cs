using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Assumes that the turn is a fragment of a circle.
/// Uses Forward vector of start and end Directions!
/// Those vectors are the tangent to the circle!
/// </summary>
public class TurnZone : ControlZone
{
    [SerializeField]
    Transform startDirection;

    [SerializeField]
    Transform endDirection;
    [SerializeField] float middlePointsCount;

    float turnRadius = 0;
    Vector3 circleCenter = Vector3.zero;

    private void Start()
    {
        float chordLength = Vector3.Distance(startDirection.position, endDirection.position);
        float angleDegreesBetweenDirections = 
            Vector3.SignedAngle(
                startDirection.forward, 
                endDirection.forward,
                Vector3.up);
        turnRadius = chordLength / (2 * Mathf.Sin(angleDegreesBetweenDirections / 2 * Mathf.Deg2Rad));
        turnRadius = Mathf.Abs(turnRadius); // sinus may be negative
        CalcCenter();


        CreateMiddlePoints();
    }

    void CalcCenter()
    {
        circleCenter = startDirection.transform.position + Quaternion.Euler(0,90,0)*startDirection.transform.forward * turnRadius;
        if( Mathf.Abs( Vector3.Distance(circleCenter, endDirection.transform.position) - turnRadius) > 0.01f)
        {
            circleCenter = startDirection.transform.position + Quaternion.Euler(0, -90, 0) * startDirection.transform.forward * turnRadius;
        }
    }

    void CreateMiddlePoints()
    {
        float angleDegreesBetwenStartEndPoints =
            Vector3.SignedAngle(
                startDirection.position - circleCenter,
                endDirection.position - circleCenter,
                Vector3.up);

        Vector3 fromCenterToStart = startDirection.position - circleCenter;
        for (int i = 0; i < middlePointsCount; i++)
        {
            GameObject middlePoint = new GameObject();
            middlePoint.name = i.ToString();
            middlePoint.transform.SetParent(transform);
            middlePoint.transform.position = Quaternion.Euler(
                0,
                angleDegreesBetwenStartEndPoints * (i+1) / (middlePointsCount+1),
                0
                ) * fromCenterToStart + circleCenter;
            middlePoints.Add(middlePoint);
        }
    }
}
