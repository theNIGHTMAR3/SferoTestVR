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
    RIGHT,
    LEFT,
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
            case CorridorType.RIGHT:
                return (0.5f, 0.5f, 1);
            case CorridorType.LEFT:
                return (0.5f, 0.5f, 1);
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
            case CorridorType.RIGHT:
                return new Vector2(0.5f, 0.5f);
            case CorridorType.LEFT:
                return new Vector2(-0.5f, 0.5f);
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
            case CorridorType.RIGHT:
                return TurnType.RIGHT;
            case CorridorType.LEFT:
                return TurnType.LEFT;
            default:
                Debug.LogError("Incorrect CorridorType");
                return TurnType.FORWARD;
        }
    }
}
