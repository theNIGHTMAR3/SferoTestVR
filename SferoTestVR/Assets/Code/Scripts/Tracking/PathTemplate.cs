using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

public class PathTemplate : MonoBehaviour
{
    [SerializeField] Room room;

    public List<Vector2> GetPathPoints()
    {
        List<Vector2> points = new List<Vector2>();

        foreach(Transform transform in transform)
        {
            Vector3 pos = room.transform.InverseTransformPoint(transform.position);
            //ignore y axis
            points.Add(new Vector2(pos.x,pos.z));
        }
        return points;
    }
    
}


