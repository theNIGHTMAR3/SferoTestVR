using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class TrackMaster : MonoBehaviour
{
    /// <summary>
    /// path to the folder where tracks of the recent run will be held
    /// </summary>
    public static string folderPath = "";
    public static float recordDelay = 0.25f;

    //used to set the static field /|\
    //                              |
    [SerializeField] public float setRecordDelay = 0.25f;


    const string baseFolder = "Tracks";

    // Start is called before the first frame update
    void Start()
    {
        recordDelay = setRecordDelay;
        CreateFolder();
    }

    void CreateFolder()
    {        

        //create folder for all tracks if not exists
        if (!Directory.Exists(baseFolder))
        {
            Directory.CreateDirectory(baseFolder);
        }

        string folderName = DateTime.Now.ToString("yyyy-MM-dd HH.mm.ss");
        folderPath = Path.Combine(baseFolder, folderName);
        Directory.CreateDirectory(folderPath);
    }

}
