using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
public class MapCreator : EditorWindow
{

    List<RoomType> rooms = new List<RoomType>();
    Vector2 scrollPos;

    [MenuItem("SferoTest/Generate Map %g")]
    public static void OpenWindow()
    {
        GetWindow<MapCreator>();
    }


    void OnGUI()
    {
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Create New Config File"))
        {
            CreateMapFile();
        }

        if (GUILayout.Button("Clear list"))
        {
            rooms.Clear();
        }
        GUILayout.EndHorizontal();
            
        if (GUILayout.Button("Add room"))
        {
            rooms.Add(new RoomType());
        }

        DrawRoomsList();

    }

    void DrawRoomsList()
    {
        scrollPos = GUILayout.BeginScrollView(scrollPos, GUILayout.Height(400));
        bool deletedItem = false;
        for(int i=0; i<rooms.Count && !deletedItem;i++)
        {
            deletedItem = DrawRoom(i);            
        }
        GUILayout.EndScrollView();
    }

    bool DrawRoom(int index)
    {
        
        GUILayout.BeginHorizontal();        
        EditorGUILayout.LabelField((index+1).ToString(), GUILayout.Width(20));
        rooms[index] =(RoomType) EditorGUILayout.EnumPopup(rooms[index]);
        if (GUILayout.Button("X"))
        {
            rooms.RemoveAt(index);
            GUILayout.EndHorizontal();
            return true;
        }
        else
        {
            GUILayout.EndHorizontal();
            return false;
        }        
    }

    /// <summary>
    /// don't know at the moment how to preserve actual list after closing Unity
    /// </summary>
    void SaveActualSetup()
    {

    }


    /// <summary>
    /// Create the config file for building and testing
    /// </summary>
    void CreateMapFile()
    {

    }
}
