using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Xml.Serialization;
using System.IO;
using UnityEngine.VR.WSA.Input;


public class UIDisplayAPI : MonoBehaviour
{
    //Reference to the text that shall display the realtime data.
    public Text UIText;

    //Reference to the DataPlayer that handles the recordings.
    public DataPlayer MyPlayer;
    public WorldAnchorSetter WAS;

    public Vector3[] HandPositions;

    public Transform PMCoords;

    //Private variables.
    List<SaveData> recordList = new List<SaveData>();
    Vector3 relativeHeadPosition;
    Vector3 relativeGazeDirection;
    RaycastHit hitInfo;
    bool castHit;
    string output;
    string status;
    bool isRecording = false;


	void Awake ()
	{
        status = "pending";
        HandPositions = new Vector3[2];
        HandPositions[0] = new Vector3(0, 0, 0);
        HandPositions[1] = new Vector3(0, 0, 0);
	}

    void Update()
    {
        PMCoords = WAS.PositionManager.transform;
        Debug.Log(isRecording);

        //Check if the raycast from the user's head in the direction of his gaze hit an object.
        if (Physics.Raycast(relativeHeadPosition, relativeGazeDirection, out hitInfo))
        {
            castHit = true;
        }
        else
        {
            castHit = false;
        }

        //Get the position of the user's head and the direction of the gaze.
        relativeHeadPosition = PMCoords.InverseTransformPoint(Camera.main.transform.position);
        relativeGazeDirection = PMCoords.InverseTransformDirection(Camera.main.transform.forward);


        //Display the collected information in the UI.
        output = castHit.ToString() +
                    Environment.NewLine + "WAC:" + PMCoords.position.ToString() +
                    Environment.NewLine + "pos2:" + Camera.main.transform.position.ToString() +
                    Environment.NewLine + "pos:" + relativeHeadPosition.ToString() +
                    Environment.NewLine + "dir:" + relativeGazeDirection.ToString() +
                    Environment.NewLine + "h1:" + HandPositions[0].x + "," + HandPositions[0].y + "," + HandPositions[0].z +
                    Environment.NewLine + "h2:" + HandPositions[1].x + "," + HandPositions[1].y + "," + HandPositions[1].z +
                    Environment.NewLine + status + 
                    Environment.NewLine + Time.time + 
                    Environment.NewLine + MyPlayer.currentFrame.ToString() + ", " + MyPlayer.isPlaying.ToString();

        UIText.GetComponent<Text>().text = output;
        

        //Start recording the data with a keybinding.
        if (Input.GetKeyDown("r"))
        {
            StartRecording();
        }

        //Stop recording the data with a keybinding.
        if (Input.GetKeyDown("t"))
        {
            StopRecording();
        }

        //Delete the recorded or temporarily stored data (no influence on opened file).
        if (Input.GetKeyDown("w"))
        {
            WipeRecord();
        }

        //Load the data from the file "test0" and fill its content in the recordList.
        if (Input.GetKeyDown("l"))
        {
            Load();
        }

        //Play the ghost of the recorded data.
        if (Input.GetKeyDown("p"))
        {
            PlayRecord();
        }
    }



    public void StartRecording()
    {

        //Stop ongoing recordings, then run the Record function 25 times per second.
        StopRecording();
        if (!isRecording)
        {
            isRecording = true;
            InvokeRepeating("Record", 0f, 0.04f);
        }
        status = "recording";
        Debug.Log("Recording");
    }

    public void Record()
    {
        //Add the data in our current frame to the list of recorded data.
        recordList.Add(new SaveData(relativeHeadPosition,
                                    relativeGazeDirection,
                                    castHit,
                                    recordList.Count,
                                    PMCoords.InverseTransformPoint(HandPositions[0]),
                                    PMCoords.InverseTransformPoint(HandPositions[1])));

    }

    public void StopRecording()
    {
        //Stop the Record function.
        isRecording = false;
        CancelInvoke("Record");
        status = "stopped";
        Debug.Log("Stopped");
    }

    public void WipeRecord()
    {
        //Wipe the temporarily stored data.
        StopRecording();
        recordList.Clear();
        status = "wiped";
        Debug.Log("Wiped");
    }

    public void PlayRecord()
    {
        //Calls the DataPlayer to replay the temporarily stored Data.
        MyPlayer.Activate(recordList, PMCoords);
        status = "playing";
    }

    public void PauseRecord()
    {
        //Calls the DataPlayer to stop the current playback.
        MyPlayer.Stop();
        status = "play/pause";
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
        string tempName = "/test" + count.ToString();

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
            status = "saved";
            Debug.Log("Saved");
        }
    }

    public List<SaveData> LoadRecording(int count = 0)
    {
        string tempName;

        //Check which file shall be opened.
        if (count < 0)
        {
            tempName = "/test";
        }
        else
        {
            tempName = "/test" + count.ToString();
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

            status = "loaded";
            Debug.Log("loaded");
            return tempList;
        }
        else
        {
            status = "failed";
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

    public SaveData(Vector3 hP, Vector3 gD, bool cH, int tS, Vector3 haP1, Vector3 haP2)
    {
        HeadPosition = hP;
        GazeDirection = gD;
        CastHit = cH;
        HandPosition1 = haP1;
        HandPosition2 = haP2;

        TimeStamp = tS * 0.04f;
    }
}