using System.Collections;
using System.Collections.Generic;
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


[CustomEditor(typeof(PathTemplate))]
public class PathShower: Editor
{
    void OnSceneGUI()
    {
        // get the chosen game object
        PathTemplate t = target as PathTemplate;

        if (t == null || t.gameObject == null)
            return;


        if (t.transform.childCount > 0)
        {
            Vector3 prevPos = t.transform.GetChild(0).transform.position;
            Vector3 nextPos;            
            foreach (Transform transform in t.transform)
            {
                nextPos = transform.position;
                Handles.color = Color.white;
                Handles.DrawLine(prevPos, nextPos);
                prevPos = nextPos;
            }
        }
    }
}

[CustomEditor(typeof(Transform))]
public class PathChildShower : Editor
{
    void OnSceneGUI()
    {
        // get the chosen game object
        Transform t = target as Transform;        
        if (t == null || t.gameObject == null)
            return;

        if (t.parent != null && t.parent.GetComponent<PathTemplate>() != null)
        {

            if (t.parent.transform.childCount > 0)
            {
                Vector3 prevPos = t.parent.transform.GetChild(0).transform.position;
                Vector3 nextPos;
                foreach (Transform transform in t.parent.transform)
                {
                    nextPos = transform.position;
                    Handles.color = Color.white;
                    Handles.DrawLine(prevPos, nextPos);
                    prevPos = nextPos;
                }
            }
        }
    }
}
