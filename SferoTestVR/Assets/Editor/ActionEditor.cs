using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Action))]
public class ActionEditor : Editor
{
    SerializedProperty type;
    SerializedProperty newPos;
    SerializedProperty newRot;
    SerializedProperty newScale;

    void OnEnable()
    {
        type = serializedObject.FindProperty("type");
        newPos = serializedObject.FindProperty("newPos");
        newRot = serializedObject.FindProperty("newRot");
        newScale = serializedObject.FindProperty("newScale");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUILayout.PropertyField(type);
        EditorGUILayout.PropertyField(newPos);
        EditorGUILayout.PropertyField(newRot);
        EditorGUILayout.PropertyField(newScale);

        serializedObject.ApplyModifiedProperties();
        if (type.enumValueIndex == (int)Action.ActionType.MOVE)
        {
            EditorGUILayout.LabelField("MOVE");
        }
        else
        {
            EditorGUILayout.LabelField("NOT MOVE");
        }
    }
}
