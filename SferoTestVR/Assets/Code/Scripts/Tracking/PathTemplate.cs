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



[CustomEditor(typeof(Transform), true)]
[CanEditMultipleObjects]
public class CustomTransformInspector : Editor
{

    //Unity's built-in editor
    Editor defaultEditor;
    Transform transform;

    void OnEnable()
    {
        //When this inspector is created, also create the built-in inspector
        defaultEditor = Editor.CreateEditor(targets, Type.GetType("UnityEditor.TransformInspector, UnityEditor"));
        transform = target as Transform;
    }

    void OnDisable()
    {
        //When OnDisable is called, the default editor we created should be destroyed to avoid memory leakage.
        //Also, make sure to call any required methods like OnDisable
        MethodInfo disableMethod = defaultEditor.GetType().GetMethod("OnDisable", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
        if (disableMethod != null)
            disableMethod.Invoke(defaultEditor, null);
        DestroyImmediate(defaultEditor);
    }
    
    public override void OnInspectorGUI()
    {
        EditorGUILayout.LabelField("Local Space", EditorStyles.boldLabel);
        defaultEditor.OnInspectorGUI();

        /* it created GUI bugs, refreshes prefabs for unknown reason :/
         * 
        //Show World Space Transform
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("World Space", EditorStyles.boldLabel);

        GUI.enabled = false;
        Vector3 localPosition = transform.localPosition;
        transform.localPosition = transform.position;

        Quaternion localRotation = transform.localRotation;
        transform.localRotation = transform.rotation;

        Vector3 localScale = transform.localScale;
        transform.localScale = transform.lossyScale;

        defaultEditor.OnInspectorGUI();
        transform.localPosition = localPosition;
        transform.localRotation = localRotation;
        transform.localScale = localScale;
        GUI.enabled = true;
        */
    }
    
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
