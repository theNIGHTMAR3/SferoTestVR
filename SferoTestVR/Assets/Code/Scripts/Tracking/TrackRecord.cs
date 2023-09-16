using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// holds 2D position and speed. vertical position in game is ignored
/// </summary>
public struct TrackRecord
{
    public Vector2 pos;
    public Vector2 vel;

    public TrackRecord(Vector2 pos, Vector2 speed)
    {
        this.pos = pos;
        this.vel = speed;
    }

    public override string ToString()
    {
        return pos.x + " " + pos.y + " " + vel.x + " " + vel.y;
    }
}
