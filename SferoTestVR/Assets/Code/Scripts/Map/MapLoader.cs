using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Linq;



public class MapLoader : MonoBehaviour
{
    public const string CONFIG_FILE = "config.txt";

    public const string FINAL_ROOMS_PATH = "Prefabs/Rooms/Final Rooms/";

    void Awake()
    {
        List<string> roomTypes = LoadRooms();
        List<Room> rooms = InstantiateRooms(roomTypes);
        //technicaly functions as a linked list XD  
        //holds only rooms! It doesn't have corridors!
        List<ExtendedRoom> extendedRooms = rooms.Select(x => new ExtendedRoom(x)).ToList(); 
        for(int i = 0; i < extendedRooms.Count - 1; i++)
        {
            extendedRooms[i].nextCorridor = new Corridor();
            extendedRooms[i].nextCorridor.nextRoom = extendedRooms[i + 1];
            extendedRooms[i].nextCorridor.prevRoom = extendedRooms[i];
            extendedRooms[i].nextCorridor.nextRoom.prevCorridor = extendedRooms[i].nextCorridor;

        }
        
        //try building map
        //in process random corridors are selected
        if (extendedRooms[0].Build() == false)
        {
            Debug.LogError("Build Map failed!");
        }

        //move each room to calculated place
        MoveRooms(extendedRooms);

        //TODO
        InstantiateCorridors(extendedRooms);// and mvoe them();
    }

    /// <summary>
    /// set the rooms' position calculated when trying to build
    /// </summary>
    /// <param name="rooms"></param>
    void MoveRooms(List<ExtendedRoom> rooms)
    {
        int index = 0;
        foreach (ExtendedRoom room in rooms)
        {
            room.room.transform.position = new Vector3(room.pos.x,0,room.pos.y) * 8; //multiply by U unit
            room.room.index = index++;

            switch (room.directon)
            {
                case Directon.UP:
                    //don't change rotation
                    break;
                case Directon.LEFT:
                    room.room.transform.rotation = Quaternion.Euler(0, -90,0);
                    break;
                case Directon.RIGHT:
                    room.room.transform.rotation = Quaternion.Euler(0, -270,0);
                    break;
                case Directon.DOWN:
                    room.room.transform.rotation = Quaternion.Euler(0,-180,0);
                    break;
            }
        }
    }

    void InstantiateCorridors(List<ExtendedRoom> rooms)
    {
        for(int roomIndex = 0; roomIndex < rooms.Count - 1; roomIndex++)
        {
            Corridor corridor = rooms[roomIndex].nextCorridor;
            Object corridorObject = Resources.Load(corridor.type.GetPrefabPath());
            GameObject corridorGameoBject = GameObject.Instantiate((GameObject)corridorObject);
            corridorGameoBject.transform.position = new Vector3(corridor.pos.x, 0, corridor.pos.y) * 8;            

            switch (corridor.directon)
            {
                case Directon.UP:
                    //don't change rotation
                    break;
                case Directon.LEFT:
                    corridorGameoBject.transform.rotation = Quaternion.Euler(0, -90, 0);
                    break;
                case Directon.RIGHT:
                    corridorGameoBject.transform.rotation = Quaternion.Euler(0, -270, 0);
                    break;
                case Directon.DOWN:
                    corridorGameoBject.transform.rotation = Quaternion.Euler(0, -180, 0);
                    break;
            }
        }
    }

    /// <summary>
    /// load rooms list from file
    /// </summary>
    /// <returns></returns>
    List<string> LoadRooms()
    {
        List<string> rooms = new List<string>();
        string configPath;

#if UNITY_EDITOR
        Debug.Log("EDITOR");
        configPath = CONFIG_FILE;
#else
    configPath = CONFIG_FILE;

#endif
        
        if (File.Exists(configPath))
        {            
            StreamReader streamReader = new StreamReader(configPath); //open the file

            string room;
            while ((room = streamReader.ReadLine()) != null) //read all lines
            {
                //add rooms to the list                
                rooms.Add(room);
            }

            streamReader.Close();
        }
        else
        {
            Debug.LogError("MapLoader: Missing map config file!");
        }

        return rooms;
    }


    void InstantiateRoom(List<Room> rooms, string name)
    {
        Object roomObject = Resources.Load(FINAL_ROOMS_PATH + name);
        Room room = GameObject.Instantiate((GameObject)roomObject).GetComponent<Room>();
        rooms.Add(room);
    }

    List<Room> InstantiateRooms(List<string> roomNames)
    {
        List<Room> rooms = new List<Room>();

        //InstantiateRoom(rooms, FINAL_ROOMS_PATH + "Start Room");        
        InstantiateRoom(rooms, "Start Room");        

        foreach (string roomName in roomNames)
        {
            InstantiateRoom(rooms,roomName);                        
        }

        InstantiateRoom(rooms,"End Room");

        return rooms;
    }
  
}



static class MyRectOverlaps
{
    public static bool OverlapWithoutEdge(this Rect a, Rect b)
    {
        if(
            a.x +a.width <= b.x //a on the left 
            ||
            a.y - a.height >= b.y //a above
            ||
            a.x >= b.x+b.width //a on the right
            ||
            a.y <= b.y-b.height //a below
            )
        {
            return false;
        }
        else
        {
            return true;
        }
    }
}


public enum Directon
{
    //up is default
    UP, LEFT, DOWN, RIGHT
}

static class DirectionRotator
{
    public static Directon Rotate(this Directon direction, TurnType turn)
    {
        switch (turn)
        {
            case TurnType.FORWARD:
                return direction;
            case TurnType.RIGHT:
                return direction == Directon.UP ? Directon.RIGHT : direction-1;
            case TurnType.LEFT:
                return direction == Directon.RIGHT ? Directon.UP : direction + 1;
            default:                
                return Directon.UP;
        }
    }
}


public class ExtendedRoom
{
    public Vector2 pos;
    public Directon directon;
    public Room room;    
    public Corridor prevCorridor;
    public Corridor nextCorridor;
    

    public ExtendedRoom(Room room)
    {
        this.room = room;
    }


    /// <summary>
    /// returns rooms rectangle size and position
    /// </summary>    
    Rect GetRect() //XDDD
    {
        Rect rect = new Rect();
        switch (directon)
        {
            case Directon.UP:
                rect.x = pos.x - ((float)room.width) / 2;
                rect.y = pos.y + room.length;
                rect.width = room.width;
                rect.height = room.length;
                break;
            case Directon.DOWN:
                rect.x = pos.x - ((float)room.width) / 2;
                rect.y = pos.y;
                rect.width = room.width;
                rect.height = room.length;
                break;
            case Directon.RIGHT:
                rect.x = pos.x;
                rect.y = pos.y + ((float)room.width) / 2;
                rect.width = room.length;
                rect.height = room.width;
                break;
            case Directon.LEFT:
                rect.x = pos.x - room.length;
                rect.y = pos.y + ((float)room.width) / 2;
                rect.width = room.length;
                rect.height = room.width;
                break;
        }
        return rect;
    }
    

    /// <summary>
    /// returns true if succeded in building
    /// </summary>    
    public bool Build()
    {
        if (prevCorridor != null)
        {
            pos = prevCorridor.GetExitPos();//set position from corridor
            directon = prevCorridor.GetExitDirection(); //set direction from corridor

            if (prevCorridor.CheckCollisions(GetRect()))
            {
                return false;
            }
        }
        else
        {
            pos = Vector2.zero;
            directon = Directon.UP;
        }


        if(nextCorridor != null)
        {
            return nextCorridor.Build();
        }
        else
        {
            return true;
        }
    }
   
    /// <summary>
    /// returns true if collided
    /// collisions are checked recursively
    /// </summary>    
    public bool CheckCollisions(Rect rect)
    {        
        if (GetRect().OverlapWithoutEdge(rect))
        {
            return true;
        }
        else
        {
            if (prevCorridor != null)
            {
                return prevCorridor.CheckCollisions(rect);
            }
            else
            {
                return false;
            }
        }
    }
    public Vector2 GetExitPos()
    {
        switch (directon)
        {
            case Directon.UP:
                return new Vector2(0,room.length) + pos;
                
            case Directon.DOWN:
                return new Vector2(0, -room.length) + pos;
            case Directon.RIGHT:
                return new Vector2(room.width,0) + pos;
            case Directon.LEFT:
                return new Vector2(-room.width, 0) + pos;

            default:
                return new Vector2(-1,-1);
        }
    }
}




public class Corridor
{

    public Vector2 pos;
    public Directon directon;
    public TurnType exitDirecton;    
    public CorridorType type;

    public List<CorridorType> notUsedCorridors;
    public ExtendedRoom prevRoom;
    public ExtendedRoom nextRoom;


    public Vector2 GetExitPos()
    {
        Vector2 offset = type.GetDoorOffset();

        //rotate offset
        switch (directon)
        {
            case Directon.UP:
                //do nothing
                break;
            case Directon.DOWN:
                offset.x *= -1;
                offset.y *= -1;
                break;
            case Directon.LEFT:
                float tempX = offset.x;
                offset.x = - offset.y;
                offset.y = tempX;
                break;
            case Directon.RIGHT:
                tempX = offset.x;
                offset.x = offset.y;
                offset.y = -tempX;
                break;
        }

        return offset + pos;
    }

    public Directon GetExitDirection()
    {
        return directon.Rotate(exitDirecton);
    }

    /// <summary>
    /// returns true if succeded in building, otherwise false
    /// </summary>
    public bool Build()
    {
        pos = prevRoom.GetExitPos();//set position
        directon = prevRoom.directon; //set direction
        ResetNotUsedCorridors();

        do
        {
            do
            {
                //check all possible solutions!!!
                if (SelectRandomCorridor() == false)
                {
                    return false;
                }
            } while (prevRoom.CheckCollisions(GetRect()));
        } while (nextRoom.Build() == false);

        return true;
    }

    /// <summary>
    /// returns true if collided
    /// </summary>
    public bool CheckCollisions(Rect rect)
    {
        if (GetRect().OverlapWithoutEdge(rect))
        {
            return true;
        }
        else
        {            
            return prevRoom.CheckCollisions(rect);
        }
    }


    Rect GetRect() //XDDD
    {
        (float leftWidth, float rightWidth, float length) corridorSize = type.GetSize();
        Rect rect = new Rect();
        switch (directon)
        {
            case Directon.UP:
                rect.x = -corridorSize.leftWidth;
                rect.y = corridorSize.length;
                rect.width = corridorSize.rightWidth + corridorSize.leftWidth;
                rect.height = corridorSize.length;
                break;
            case Directon.DOWN:
                rect.x =  -corridorSize.rightWidth;
                rect.y = 0;
                rect.width = corridorSize.rightWidth + corridorSize.leftWidth;
                rect.height = corridorSize.length;
                break;
            case Directon.RIGHT:
                rect.x = 0;
                rect.y = corridorSize.leftWidth;
                rect.width = corridorSize.length;
                rect.height = corridorSize.rightWidth + corridorSize.leftWidth;
                break;
            case Directon.LEFT:
                rect.x =  - corridorSize.length;
                rect.y =  corridorSize.rightWidth;
                rect.width = corridorSize.length;
                rect.height = corridorSize.rightWidth + corridorSize.leftWidth;
                break;
        }

        rect.x += pos.x;
        rect.y += pos.y;

        return rect;
    }

        /// <summary>
        /// resets the room not used corridors list, for given room
        /// </summary>   
        public void ResetNotUsedCorridors()
    {
        notUsedCorridors = new List<CorridorType>();
        for (int i = 0; i < (int)CorridorType.TYPES_NUMBER; i++)
        {
            notUsedCorridors.Add((CorridorType)i);
        }
    }


    /// <summary>
    /// changes the corridor type. If can't return false, otherwise true.
    /// </summary>
    public bool SelectRandomCorridor()
    {
        
        if (notUsedCorridors.Count == 0)
        {
            return false;
        }
        type = notUsedCorridors[Random.Range(0, notUsedCorridors.Count - 1)];        
        notUsedCorridors.Remove(type);
        
        if(type != CorridorType.FORWARD)
        {
            int a = 2;
        }
        exitDirecton = type.ToTurnType();
        return true;        
    }
}