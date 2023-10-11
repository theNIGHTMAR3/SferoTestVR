
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ActionQueue))]
public class DamagerEditor : Editor
{
    private SerializedProperty actionsList;

    private void OnEnable()
    {
        actionsList = serializedObject.FindProperty("actions");
    }


    private void ShowAction(SerializedProperty property)
    {

        SerializedProperty action = property.FindPropertyRelative("actionFloat");
        EditorGUILayout.PropertyField(action, true);

    }


    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        #region queue loop toggle
        EditorGUILayout.BeginHorizontal();
        EditorGUIUtility.labelWidth = 70;
        EditorGUI.indentLevel++;

        SerializedProperty looped = serializedObject.FindProperty("looped");
        EditorGUILayout.PropertyField(looped, true);

        if (looped.boolValue)
        {
            SerializedProperty loopCount = serializedObject.FindProperty("loopCount");
            SerializedProperty loopDuration = serializedObject.FindProperty("loopDuration");

            EditorGUIUtility.labelWidth = 90;
            EditorGUILayout.PropertyField(loopCount, true);
            EditorGUIUtility.labelWidth = 100;
            EditorGUILayout.PropertyField(loopDuration, true);
        }
        EditorGUILayout.EndHorizontal();
        #endregion



        EditorGUILayout.BeginHorizontal();
        SerializedProperty activatedByTrigger = serializedObject.FindProperty("activatedByTrigger");
        EditorGUIUtility.labelWidth = 140;
        EditorGUILayout.PropertyField(activatedByTrigger, true);

        SerializedProperty offset = serializedObject.FindProperty("offset");
        EditorGUILayout.PropertyField(offset, true);
        EditorGUILayout.EndHorizontal();


        EditorGUI.indentLevel--;
        //EditorGUI.BeginChangeCheck();

        EditorGUI.indentLevel++;

        bool deleted = false;

        for (int i = 0; i < actionsList.arraySize; i++)
        {
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("X", GUILayout.Width(50)))
            {

                actionsList.DeleteArrayElementAtIndex(i);
                deleted = true;
            }

            if (!deleted)
            {
                EditorGUILayout.BeginHorizontal();
                SerializedProperty action = actionsList.GetArrayElementAtIndex(i);


                EditorGUIUtility.labelWidth = 75;
                SerializedProperty actionType = action.FindPropertyRelative("type");
                EditorGUILayout.PropertyField(actionType, true);

                //if type was changed
                if (actionType.intValue != (int)((ActionQueue)target).actions[i].type)
                {
                    SerializedProperty start = action.FindPropertyRelative("startTime");
                    SerializedProperty duration = action.FindPropertyRelative("duration");
                    //and delete previous
                    switch (actionType.intValue)
                    {
                        case (int)Action.ActionType.MOVE:

                            action.managedReferenceValue = new Move(start.floatValue, duration.floatValue);
                            break;
                        case (int)Action.ActionType.ROTATE:
                            action.managedReferenceValue = new Rotate(start.floatValue, duration.floatValue);
                            break;
                        case (int)Action.ActionType.SCALE:
                            action.managedReferenceValue = new Scale(start.floatValue, duration.floatValue);
                            break;
                        case (int)Action.ActionType.TRIGERRABLE:
                            action.managedReferenceValue = new Trigger(start.floatValue);
                            break;
                    }


                }


                SerializedProperty actionStartTime = action.FindPropertyRelative("startTime");
                EditorGUILayout.PropertyField(actionStartTime, true);

                actionType = action.FindPropertyRelative("type"); //refresh when changed Action
                if (actionType.intValue != (int)Action.ActionType.TRIGERRABLE)
                {
                    EditorGUIUtility.labelWidth = 70;

                    SerializedProperty actionDuration = action.FindPropertyRelative("duration");
                    EditorGUILayout.PropertyField(actionDuration, true);
                }
                EditorGUILayout.EndHorizontal();

                DrawAction(action, actionType);

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("");
                EditorGUILayout.EndHorizontal();
            }
        }

        EditorGUI.indentLevel--;



        DrawButtons();


        serializedObject.ApplyModifiedProperties();
    }


    #region Draw Actions
    void DrawAction(SerializedProperty action, SerializedProperty actionType)
    {
        EditorGUIUtility.labelWidth = 70;
        switch (actionType.intValue)
        {
            case (int)Action.ActionType.MOVE:
                DrawMove(action);
                break;
            case (int)Action.ActionType.ROTATE:
                DrawRotate(action);
                break;
            case (int)Action.ActionType.SCALE:
                DrawScale(action);
                break;
            case (int)Action.ActionType.TRIGERRABLE:
                DrawTrigger(action);
                break;
        }
    }

    void DrawMove(SerializedProperty move)
    {
        SerializedProperty moveNewPos = move.FindPropertyRelative("newPos");
        SerializedProperty movePosUsingCurve = move.FindPropertyRelative("posUsingCurve");
        SerializedProperty movePosCurve = move.FindPropertyRelative("posCurve");


        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PropertyField(moveNewPos);
        EditorGUIUtility.labelWidth = 80;
        EditorGUILayout.PropertyField(movePosUsingCurve);
        EditorGUILayout.EndHorizontal();
        if (movePosUsingCurve.boolValue)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(movePosCurve);
            EditorGUILayout.EndHorizontal();
        }
    }

    void DrawRotate(SerializedProperty rotate)
    {
        SerializedProperty newRot = rotate.FindPropertyRelative("newRot");
        SerializedProperty rotIsOffset = rotate.FindPropertyRelative("rotIsOffset");
        SerializedProperty rotUsingCurve = rotate.FindPropertyRelative("rotUsingCurve");
        SerializedProperty rotCurve = rotate.FindPropertyRelative("rotCurve");

        EditorGUILayout.BeginHorizontal();

        EditorGUILayout.PropertyField(newRot);
        EditorGUILayout.PropertyField(rotIsOffset);
        EditorGUILayout.EndHorizontal();

        EditorGUIUtility.labelWidth = 80;
        EditorGUILayout.PropertyField(rotUsingCurve);

        if (rotUsingCurve.boolValue)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(rotCurve);
            EditorGUILayout.EndHorizontal();
        }
    }

    void DrawScale(SerializedProperty scale)
    {
        SerializedProperty newScale = scale.FindPropertyRelative("newScale");
        SerializedProperty scaleUsingCurve = scale.FindPropertyRelative("scaleUsingCurve");
        SerializedProperty scaleCurve = scale.FindPropertyRelative("scaleCurve");

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PropertyField(newScale);
        EditorGUILayout.EndHorizontal();

        EditorGUIUtility.labelWidth = 80;
        EditorGUILayout.PropertyField(scaleUsingCurve);

        if (scaleUsingCurve.boolValue)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(scaleCurve);
            EditorGUILayout.EndHorizontal();
        }
    }

    void DrawTrigger(SerializedProperty trigger)
    {
        SerializedProperty triggerable = trigger.FindPropertyRelative("triggerable");

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PropertyField(triggerable);
        EditorGUILayout.EndHorizontal();
    }
    #endregion

    void DrawButtons()
    {
        ActionQueue actionQueue = (ActionQueue)target;

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("↔"))
        {
            Undo.RecordObject(actionQueue, "Add Action");
            actionQueue.actions.Add(new Move());
        }

        if (GUILayout.Button("↺"))
        {
            Undo.RecordObject(actionQueue, "Add Action");
            actionQueue.actions.Add(new Rotate());
        }


        if (GUILayout.Button("▣"))
        {
            Undo.RecordObject(actionQueue, "Add Action");
            actionQueue.actions.Add(new Scale());
        }

        if (GUILayout.Button("⌖"))  //◎
        {
            Undo.RecordObject(actionQueue, "Add Action");
            actionQueue.actions.Add(new Trigger());
        }
        EditorGUILayout.EndHorizontal();
    }

    void OnSceneGUI()
    {
        // get the chosen game object
        ActionQueue t = target as ActionQueue;

        if (t == null || t.gameObject == null)
            return;

        // grab the center of the parent
        Vector3 prevPos = t.transform.position;
        Vector3 nextPos = prevPos;
        float prevRot = t.transform.eulerAngles.y;
        float nextRot = prevRot;
        Vector2 prevScale = t.transform.localScale;
        Vector2 nextScale = prevScale;

        foreach (Action action in t.actions)
        {

            switch (action.type)
            {
                case Action.ActionType.MOVE:
                    prevPos = nextPos;
                    nextPos = ((Move)action).newPos;
                    Handles.color = Color.white;
                    Handles.DrawLine(prevPos, nextPos);
                    break;
                case Action.ActionType.ROTATE:
                    Handles.color = Color.green;
                    Handles.ArrowHandleCap(0, nextPos, Quaternion.Euler(0, 90, 0) * Quaternion.Euler(-((Rotate)action).newRot - 90, 0, 0), 1, EventType.Repaint);
                    break;
                case Action.ActionType.SCALE:
                    prevScale = nextScale;
                    nextScale = ((Scale)action).newScale;
                    Handles.color = Color.blue;
                    Handles.DrawWireDisc(nextPos, Vector3.forward, prevScale.x / 5);
                    Handles.color = Color.red;
                    Handles.DrawWireDisc(nextPos, Vector3.forward, nextScale.x / 5);
                    break;
            }
        }
    }

}
