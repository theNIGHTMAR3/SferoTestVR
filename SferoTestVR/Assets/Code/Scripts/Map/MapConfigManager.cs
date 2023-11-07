using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
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

        var roomsPrefabs = Resources.LoadAll("Prefabs/Rooms/Final Rooms");
        foreach (var room in roomsPrefabs)
        {
            
            string roomName = room.name;            
            rooms.Add(roomName);
            
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
        Debug.Log("GetRoomImage");
#if UNITY_EDITOR
        
        Object roomObject = Resources.Load(MapLoader.FINAL_ROOMS_PATH + name);

        Texture2D preview = null;
        preview = AssetPreview.GetAssetPreview(roomObject);
        while (AssetPreview.IsLoadingAssetPreview(roomObject.GetInstanceID()))
        {
            //wait
            preview = AssetPreview.GetAssetPreview(roomObject);
        }        
        

        return preview;


#else
        Texture2D tex = null;
        byte[] fileData;


        string currentDirectiory = Directory.GetCurrentDirectory();
        var dirPath = Path.Combine(currentDirectiory, "Previews");
        string previewPath = Path.Combine(dirPath, name + ".png");

        Debug.Log(dirPath);
        Debug.Log(previewPath);
        Debug.Log(Directory.GetCurrentDirectory());

        if (File.Exists(previewPath))
        {
            do
            {
                fileData = File.ReadAllBytes(previewPath);
                tex = new Texture2D(2, 2);
                tex.LoadImage(fileData); //..this will auto-resize the texture dimensions.
            } while (!tex.IsRequestedMipmapLevelLoaded()); //how to check if the texture is loaded?
        }
        return tex;
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
