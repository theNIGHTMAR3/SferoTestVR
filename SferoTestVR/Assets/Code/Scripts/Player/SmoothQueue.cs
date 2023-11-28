using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// smooth queue. Allows only pushing values. If it has
/// too many, it will remove the oldest.
/// It only returns the average of its elements
/// </summary>
public class SmoothQueue
{
    Queue<float> queue;
    int smoothness = 5;

    public SmoothQueue()
    {
        queue = new Queue<float>();
    }

    public void Push(float val)
    {
        queue.Enqueue(val);
        if (queue.Count > smoothness)
        {
            queue.Dequeue();
        }
    }

    public float GetSmooth()
    {
        return queue.Average();
    }
}

public class SmoothVectorQueue
{
    Queue<Vector3> queue;
    int smoothness = 5;

    public SmoothVectorQueue()
    {
        queue = new Queue<Vector3>();
    }

    public void Push(Vector3 val)
    {
        queue.Enqueue(val);
        if (queue.Count > smoothness)
        {
            queue.Dequeue();
        }
    }

    public Vector3 GetSmooth()
    {
        Vector3 avg = Vector3.zero;

        for(int i=0;i< queue.Count; i++)
        {
            avg += queue.ElementAt(i);
        }
        avg /= smoothness;

        return avg;
    }
}