using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

public static class MapConfigManager
{


    /// <summary>
    /// return all the rooms names in the folder:
    /// Assets/Resources/Prefabs/Rooms/Final Rooms
    /// 
    /// doesn't return the name:
    /// -Start Room
    /// -End room
    /// -.meta files
    /// </summary>
    /// <returns></returns>
    public static List<string> GetRoomsList()
    {
        List<string> rooms = new List<string>();

#if UNITY_EDITOR
        var files = from file in Directory.EnumerateFiles("Assets/Resources/Prefabs/Rooms/Final Rooms/") select file;
#else
        //different folder
        var files = from file in Directory.EnumerateFiles("Resources/Prefabs/Rooms/Final Rooms/") select file;
#endif
        foreach (var file in files)
        {
            if (Path.GetExtension(file) != ".meta")
            {
                string roomName = Path.GetFileName(file);
                roomName = roomName.Substring(0, roomName.Length - 7); //remove ".prefab"
                rooms.Add(roomName);
            }
        }

        return rooms;
    }



    /// <summary>
    /// converts the room file name without ".prefab" to the index in folder
    /// </summary>    
    public static int RoomNameToIndex(string name)
    {
        List<string> roomsNames = GetRoomsList();

        return roomsNames.IndexOf(name);
    }



    /// <summary>
    /// return the mini thumbnail of the prefab. The image show in the
    /// project window
    /// </summary>    
    public static Texture2D GetRoomImage(string name)
    {
    #if UNITY_EDITOR
        Object roomObject = Resources.Load(MapLoader.FINAL_ROOMS_PATH + name);

        return AssetPreview.GetAssetPreview(roomObject);
    #else   
        return null;
    #endif
    }


    /// <summary>
    /// saves rooms (given as list of indexes) and saves it as
    /// string list in a file
    /// </summary>
    /// <param name="rooms">list of indexes of chosen rooms (order in folder)</param>
    public static void SaveLocally(List<int> rooms)
    {
        StreamWriter streamWriter = new StreamWriter(MapLoader.CONFIG_FILE);

        List<string> roomsNames = GetRoomsList();

        foreach (int room in rooms)
        {
            streamWriter.WriteLine(roomsNames[room]);
        }

        streamWriter.Close();

    }


    /// <summary>
    /// saves rooms (given as string names) and saves it as
    /// string list in a file
    /// </summary>    
    public static void SaveLocally(List<string> rooms)
    {
        StreamWriter streamWriter = new StreamWriter(MapLoader.CONFIG_FILE);
        

        foreach (string room in rooms)
        {
            streamWriter.WriteLine(room);
        }

        streamWriter.Close();

    }

}
