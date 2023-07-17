using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// no more turn types!
/// </summary>
public enum TurnType
{    
    FORWARD,
    RIGHT,
    LEFT,
    TURNS_NUMBER
}

/// <summary>
/// may have more types of corridors
/// </summary>
public enum CorridorType
{
    FORWARD,
    RIGHT_1U,
    LEFT_1U,
    RIGHT_2U,
    LEFT_2U,
    RIGHT_3U,
    LEFT_3U,
    TYPES_NUMBER
}

static class CorridorSizes
{

    /// 
    ///  ------------------------------------     _
    ///  |                                  |     |
    ///  |                                  |     |
    ///  |      . . . . . . . . .  . . . . .EXIT  | 
    ///  |      .                           |     |
    ///  |      .                           |     |  length
    ///  |      .                           |     |
    ///  |      .                           |     |
    ///  -------X----------------------------     -
    ///  [LWidth][   Right Width            ] 


    public static (float leftWidth, float rightWidth, float length) GetSize(this CorridorType corridor)
    {
        switch (corridor)
        {
            case CorridorType.FORWARD:
                return (0.5f, 0.5f, 1);
            case CorridorType.RIGHT_1U:
                return (0.5f, 0.5f, 1);
            case CorridorType.LEFT_1U:
                return (0.5f, 0.5f, 1);

            case CorridorType.RIGHT_2U:
                return (0.5f, 1.5f, 2);
            case CorridorType.LEFT_2U:
                return (1.5f, 0.5f, 2);

            case CorridorType.RIGHT_3U:
                return (0.5f, 2.5f, 3);
            case CorridorType.LEFT_3U:
                return (2.5f, 0.5f, 3);
            default:
                Debug.LogError("Incorrect CorridorType");
                return (-1,-1,-1);
        }
    }


    /// <summary>
    /// door offset from the entrance
    /// </summary>        
    public static Vector2 GetDoorOffset(this CorridorType corridor)
    {
        switch (corridor)
        {
            case CorridorType.FORWARD:
                return new Vector2(0, 1);
            case CorridorType.RIGHT_1U:
                return new Vector2(0.5f, 0.5f);
            case CorridorType.LEFT_1U:
                return new Vector2(-0.5f, 0.5f);

            case CorridorType.RIGHT_2U:
                return new Vector2(1.5f, 1.5f);
            case CorridorType.LEFT_2U:
                return new Vector2(-1.5f, 1.5f);

            case CorridorType.RIGHT_3U:
                return new Vector2(2.5f, 2.5f);
            case CorridorType.LEFT_3U:
                return new Vector2(-2.5f, 2.5f);

            default:
                Debug.LogError("Incorrect CorridorType");
                return new Vector2(-1, -1);
        }
    }


    public static TurnType ToTurnType(this CorridorType corridor)
    {
        switch (corridor)
        {
            case CorridorType.FORWARD:
                return TurnType.FORWARD;
            case CorridorType.RIGHT_1U:
                return TurnType.RIGHT;
            case CorridorType.LEFT_1U:
                return TurnType.LEFT;
            case CorridorType.RIGHT_2U:
                return TurnType.RIGHT;
            case CorridorType.LEFT_2U:
                return TurnType.LEFT;
            case CorridorType.RIGHT_3U:
                return TurnType.RIGHT;
            case CorridorType.LEFT_3U:
                return TurnType.LEFT;
            default:
                Debug.LogError("Incorrect CorridorType");
                return TurnType.FORWARD;
        }
    }
}


static class CorridorTypeConverter
{
    const string corridorsFolder = "Prefabs/Corridors/";
    public static string GetPrefabPath(this CorridorType corridorType)
    {

        switch (corridorType)
        {
            case CorridorType.FORWARD:
                return corridorsFolder + "Corridor_Forward";
            case CorridorType.RIGHT_1U:
                return corridorsFolder + "Corridor_1U_Right";
            case CorridorType.LEFT_1U:
                return corridorsFolder + "Corridor_1U_Left";
            case CorridorType.RIGHT_2U:
                return corridorsFolder + "Corridor_2U_Right";
            case CorridorType.LEFT_2U:
                return corridorsFolder + "Corridor_2U_Left";
            case CorridorType.RIGHT_3U:
                return corridorsFolder + "Corridor_3U_Right";
            case CorridorType.LEFT_3U:
                return corridorsFolder + "Corridor_3U_Left";
            default:
                Debug.LogError("Incorrect corridor type");
                return "What?!";
        }
    }
}