using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
public class MapCreator : EditorWindow
{

    List<int> rooms = new List<int>();
    Vector2 scrollPos;
    


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
            MapConfigManager.SaveLocally(rooms);
        }
        GUILayout.EndHorizontal();
            
        if (GUILayout.Button("Add room"))
        {
            rooms.Add(0);
            MapConfigManager.SaveLocally(rooms);
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

        int prevType = rooms[index];
        //rooms[index] =(RoomType) EditorGUILayout.EnumPopup(rooms[index]);
        rooms[index] =EditorGUILayout.Popup(rooms[index], MapConfigManager.GetRoomsList().ToArray());

        if (prevType != rooms[index])
        {
            MapConfigManager.SaveLocally(rooms);
            return false;
        }
        else
        {
            if (GUILayout.Button("X"))
            {
                rooms.RemoveAt(index);
                GUILayout.EndHorizontal();
                MapConfigManager.SaveLocally(rooms);
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

    void LoadConfig()
    {
        //check if config file exist
        if (File.Exists(MapLoader.CONFIG_FILE))
        {
            List<string> roomsNames = MapConfigManager.GetRoomsList();
            rooms = new List<int>();
            StreamReader streamReader = new StreamReader(MapLoader.CONFIG_FILE); //open the file

            string room;
            while ((room = streamReader.ReadLine()) != null) //read all lines
            {
                //add rooms to the list                
                rooms.Add(MapConfigManager.roomNameToIndex(room));
            }

            streamReader.Close();
        }
    }
}
