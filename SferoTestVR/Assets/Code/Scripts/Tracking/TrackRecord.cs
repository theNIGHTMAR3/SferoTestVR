using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// holds 2D position and speed. vertical position in game is ignored
/// </summary>
[Serializable]
public struct TrackRecord
{
    public Vector2 pos;
    public Vector2 vel;

    public SphereRecord sphereRecord;
    public MotorRecords motorRecords;

    public TrackRecord(Vector2 pos, Vector2 vel, SphereRecord sphereRecord, MotorRecords motorRecords)
    {
        this.pos = pos;
        this.vel = vel;
        this.sphereRecord = sphereRecord;
        this.motorRecords = motorRecords;
    }

    public override string ToString()
    {
        return pos.x + " " + pos.y + " " + vel.x + " " + vel.y;
    }
}

[Serializable]
public struct MotorRecords
{
    public MotorRecord motor1, motor2;
    public MotorRecord motor3, motor4;

    public MotorRecords(MotorRecord motor1, MotorRecord motor2, MotorRecord motor3, MotorRecord motor4)
    {
        this.motor1 = motor1;
        this.motor2 = motor2;
        this.motor3 = motor3;
        this.motor4 = motor4;
    }
}


[Serializable]
public struct MotorRecord
{
    public float motorCurrent;
    public float motorVoltage;
    public float motorVelocity;

    public MotorRecord(float motorCurrent, float motorVoltage, float motorVelocity)
    {
        this.motorCurrent = motorCurrent;
        this.motorVoltage = motorVoltage;
        this.motorVelocity = motorVelocity;
    }
}

[Serializable]
public struct SphereRecord
{
    public long timestamp;
    public float velocity;
    public float direction;
    public float velocityVectorX;
    public float velocityVectorY;
    public float velocityVectorZ;

    public SphereRecord(long timestamp, float velocity, float direction, float velocityVectorX, float velocityVectorY, float velocityVectorZ)
    {
        this.timestamp = timestamp;
        this.velocity = velocity;
        this.direction = direction;
        this.velocityVectorX = velocityVectorX;
        this.velocityVectorY = velocityVectorY;
        this.velocityVectorZ = velocityVectorZ;
    }
}