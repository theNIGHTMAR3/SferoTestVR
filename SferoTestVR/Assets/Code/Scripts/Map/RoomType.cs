using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RoomType
{
    A,
    B,
    C,
    D,
    E,
    F,
    G,
    H,
    I,
    J,
    K,
    L,
    M,

    EndRoom
}

/// <summary>
/// each room prefab needs to have unique path to file
/// </summary>
static class RoomTypeConverter
{
    const string roomsFolder = "Prefabs/Rooms/Final Rooms/";
    public static string GetPrefabPath(this RoomType room)
    {
        
        switch (room)
        {
            case RoomType.A:
                return roomsFolder + "A";
            case RoomType.B:
                return roomsFolder + "B";
            default:
                Debug.LogError("Incorrect Room type");
                return "What?!";
        }
    }
}


