using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
public class MapCreator : EditorWindow
{

    List<RoomType> rooms = new List<RoomType>();
    Vector2 scrollPos;

    const string CONFIG_FILE = "config.txt";


    [MenuItem("SferoTest/Generate Map %g")]
    public static void OpenWindow()
    {
        GetWindow<MapCreator>();        
    }
    
    public void OnEnable()
    {
        LoadConfig();
    }

    /// <summary>
    /// defining window inputs and behaviour
    /// </summary>
    void OnGUI()
    {
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Save Externaly"))
        {
            SaveExternally();
        }

        if (GUILayout.Button("Clear list"))
        {
            rooms.Clear();
            SaveLocally();
        }
        GUILayout.EndHorizontal();
            
        if (GUILayout.Button("Add room"))
        {
            rooms.Add(new RoomType());
            SaveLocally();
        }

        DrawRoomsList();

    }

    /// <summary>
    /// draw rooms in scroll view
    /// </summary>
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

    /// <summary>
    /// draw single room in the scroll view
    /// returns true if element was deleted
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    bool DrawRoom(int index)
    {
        
        GUILayout.BeginHorizontal();        
        EditorGUILayout.LabelField((index+1).ToString(), GUILayout.Width(20));

        RoomType prevType = rooms[index];
        rooms[index] =(RoomType) EditorGUILayout.EnumPopup(rooms[index]);

        if (prevType != rooms[index])
        {
            SaveLocally();
            return false;
        }
        else
        {
            if (GUILayout.Button("X"))
            {
                rooms.RemoveAt(index);
                GUILayout.EndHorizontal();
                SaveLocally();
                return true;
            }
            else
            {
                GUILayout.EndHorizontal();
                return false;
            }
        }
    }


    /// <summary>
    /// Create the config file for building and testing, in different place
    /// </summary>
    void SaveExternally()
    {

    }
    /// <summary>
    /// Saves locally the map config file. Should be called after each change
    /// </summary>
    void SaveLocally()
    {
        StreamWriter streamWriter = new StreamWriter(CONFIG_FILE);        

        foreach(RoomType room in rooms)
        {
            streamWriter.WriteLine((int) room);
        }

        streamWriter.Close();

    }

    void LoadConfig()
    {
        //check if config file exist
        if (File.Exists(CONFIG_FILE))
        {
            rooms = new List<RoomType>();
            StreamReader streamReader = new StreamReader(CONFIG_FILE); //open the file

            string line;
            while ((line = streamReader.ReadLine()) != null) //read all lines
            {
                //add rooms to the list
                RoomType room = (RoomType)int.Parse(line);
                rooms.Add(room);
            }

            streamReader.Close();
        }
    }
}
