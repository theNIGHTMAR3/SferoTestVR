using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Tracker : MonoBehaviour
{
    [SerializeField] Room room;
    [SerializeField] PathTemplate pathTemplate;

    bool tracking = false;
    GameObject player;
    Rigidbody playerRb;


    List<TrackRecord> records = new List<TrackRecord>();

    float lastRecord = 0;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").gameObject;
        playerRb = player.gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (tracking)
        {
            if(Time.time > lastRecord + TrackMaster.recordDelay)
            {
                lastRecord = Time.time;

                Vector3 playerPos = room.transform.InverseTransformPoint(player.transform.position);
                Vector3 playerVel = room.transform.InverseTransformDirection(playerRb.velocity);

                records.Add(new TrackRecord(
                    new Vector2(playerPos.x, playerPos.z),
                    new Vector2(playerVel.x, playerVel.z)));
            }
        }
    }



    public void StartTracking()
    {
        tracking = true;
        records = new List<TrackRecord>();
    }


    public void StopTracking()
    {
        tracking = false;
        //save to the file (always?)

        SaveToFile();
        SVG.CreateSVG("Room " + room.index.ToString() + ".svg", records, pathTemplate.GetPathPoints(),room.length,room.width);
    }


    class TrackFile
    {
        public string roomName;
        public string roomDimensions;
        public List<TrackRecord> records;
        public List<Vector2> points;
    }


    void SaveToFile()
    {
        string filename = "Room " + room.index.ToString();
        string filePath = Path.Combine(TrackMaster.folderPath, filename);

        using (StreamWriter writer = new StreamWriter(File.Create(filePath)))
        {
            /*
            //write the room name
            writer.WriteLine(room.gameObject.name);

            //write the room size
            writer.WriteLine(room.width + " x " + room.length);

            //write the players path
            writer.WriteLine(records.Count);
            foreach (TrackRecord record in records)
            {
                writer.WriteLine(record);
            }



            //write the template path
            List<Vector2> points = pathTemplate.GetPathPoints();

            writer.WriteLine(points.Count);

            foreach (Vector2 point in points)
            {
                writer.WriteLine(point);
            }
            */

            TrackFile trackFile = new TrackFile();
            trackFile.roomName = room.gameObject.name;
            trackFile.roomDimensions = room.width + " x " + room.length;
            trackFile.records = records;
            trackFile.points = pathTemplate.GetPathPoints();

            string stringjson = JsonUtility.ToJson(trackFile);
            writer.Write(stringjson);
            writer.Close();
            
        }
    }

}
