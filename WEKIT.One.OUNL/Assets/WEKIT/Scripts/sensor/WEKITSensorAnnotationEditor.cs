using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Xml.Serialization;
using System.IO;
using UnityEngine;

public class WEKITSensorAnnotationEditor : WEKITAnnotationBaseEditor
{
    //Reference to the DataPlayer that handles the recordings.
    public DataPlayer MyPlayer;

    //Reference to the DataManager that handles the data.
    public UIDisplayAPI DM;

    //Private variables.
    List<SaveData> recordList = new List<SaveData>();
    bool isRecording = false;


    public void StartRecording()
    {
        //Stop ongoing recordings, then run the Record function 25 times per second.
        StopRecording();
        if (!isRecording)
        {
            isRecording = true;
            InvokeRepeating("Record", 0f, 0.04f);
        }
        Debug.Log("Recording");
    }

    public void Record()
    {
        //Add the data in our current frame to the list of recorded data.
        SaveData sd = new SaveData(DM.headPosition, DM.gazeDirection, DM.castHit, recordList.Count, DM.HandPositions);
        recordList.Add(sd);
        //Debug.Log("Record: " + recordList.Count + ", " + sd.ToString());
    }

    public void StopRecording()
    {
        //Stop the Record function.
        isRecording = false;
        CancelInvoke("Record");
        Debug.Log("Stopped");
    }

    public void WipeRecord()
    {
        //Wipe the temporarily stored data.
        StopRecording();
        recordList.Clear();
        Debug.Log("Wiped");
    }

    public void PlayRecord()
    {
        //Calls the DataPlayer to replay the temporarily stored Data.
        MyPlayer.Activate(recordList);
        Debug.Log("Playing");
    }

    public void PauseRecord()
    {
        //Calls the DataPlayer to stop the current playback.
        MyPlayer.Stop();
    }

    public void Save()
    {
        //Locally save the temporarily stored data.
        StopRecording();
        SaveRecording(recordList);
    }

    public void Load()
    {
        //Open the first file "test0" if it exists and load its content into the recordList.
        StopRecording();
        recordList = LoadRecording();
    }

    void SaveRecording(List<SaveData> currentRecording, int count = 0)
    {
        //Check the count of files and name the new one accordingly.
        //Note: A new file will always be created; overwriting data not yet implemented.
        string tempName = "/test" + this.id + "_" + count.ToString();

        if (File.Exists(Application.persistentDataPath + tempName))
        {
            SaveRecording(currentRecording, count + 1);
        }
        else
        {
            //Write the data in an .xml file and save it locally.
            FileStream file = File.Create(Application.persistentDataPath + tempName);
            XmlSerializer xS = new XmlSerializer(typeof(List<SaveData>));
            TextWriter tW = new StreamWriter(file);
            xS.Serialize(tW, currentRecording);
            Debug.Log("Saved");
        }
    }

    public List<SaveData> LoadRecording(int count = 0)
    {
        string tempName;

        //Check which file shall be opened.
        if (count < 0)
        {
            tempName = "/test" + this.id + "_";
        }
        else
        {
            tempName = "/test" + this.id + "_" + count.ToString();
        }

        if (File.Exists(Application.persistentDataPath + tempName))
        {
            //Open the chosen file with an .xml reader.
            //Store the data in the file in our temporary data record.
            //New records will be added to this data if not wiped.
            //Saving will produce a new file. Old data cannot be overwritten.
            FileStream file = File.Open(Application.persistentDataPath + tempName, FileMode.Open);
            XmlSerializer xS = new XmlSerializer(typeof(List<SaveData>));
            TextReader tR = new StreamReader(file);
            List<SaveData> tempList = (List<SaveData>)xS.Deserialize(tR);

            Debug.Log("loaded");
            return tempList;
        }
        else
        {
            Debug.Log("failed");
            return null;
        }
    }
}



//The serializable custom class in which the gathered data will be stored, one instance for each step.
[Serializable]
public class SaveData
{
    public float TimeStamp;

    public Vector3 HandPosition1;
    public Vector3 HandPosition2;
    public Vector3 HeadPosition;
    public Vector3 GazeDirection;
    public bool CastHit;

    public SaveData()
    {

    }

    public SaveData(Vector3 hP, Vector3 gD, bool cH, int tS, Vector3[] haP)
    {
        HeadPosition = hP;
        GazeDirection = gD;
        CastHit = cH;
        HandPosition1 = haP[0];
        HandPosition2 = haP[1];

        TimeStamp = tS * 0.04f;
    }
}