using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Xml.Serialization;
using System.IO;

//All NETFX_Core usages are to be used in a build targeted at Windows platforms (including HoloLens)
//and extend or replace the functions used in Unity.
#if NETFX_CORE
 using System.Threading.Tasks;
 using Windows.Storage;
 using Windows.Storage.Streams;
#endif

public class UIDisplayAPI : MonoBehaviour
{
    //Reference to the text that shall display the realtime data.
    public Text UIText;

    //Private variables.
    List<saveData> recordList = new List<saveData>();
    Vector3 headPosition;
    Vector3 gazeDirection;
    RaycastHit hitInfo;
    bool castHit;
    string output;
    bool isRecording = false;

	void Awake ()
	{

	}

    void Update()
    {
        Debug.Log(recordList.Count);
        Debug.Log(isRecording);
        //Check if the raycast from the user's head in the direction of his gaze hit an object.

        if (Physics.Raycast(headPosition, gazeDirection, out hitInfo))
        {
            castHit = true;
        }
        else
        {
            castHit = false;
        }

        //Get the position of the user's head and the direction of the gaze.
        headPosition = Camera.main.transform.position;
        gazeDirection = Camera.main.transform.forward;



#if NETFX_CORE
        output = castHit.ToString() + Environment.NewLine + "pos:" + headPosition.ToString() + Environment.NewLine + "dir:" + gazeDirection.ToString() + Environment.NewLine + "HALLO";
        output += Environment.NewLine + ApplicationData.Current.LocalFolder.Path.ToString();
#else
        //Display the collected information in the UI.
        output = castHit.ToString() + Environment.NewLine + "pos:" + headPosition.ToString() + Environment.NewLine + "dir:" + gazeDirection.ToString();
#endif

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
        Debug.Log("Recording");
    }

    void Record()
    {
        //Add the data in our current frame to the list of recorded data.
        recordList.Add(new saveData(headPosition, gazeDirection, castHit, recordList.Count));
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

    void SaveRecording(List<saveData> currentRecording, int count = 0)
    {
        //Check the count of files and name the new one accordingly.
        //Note: A new file will always be created; overwriting data not yet implemented.
        string tempName = "/test" + count.ToString();

#if NETFX_CORE
        XmlIO.SaveObjectToXml(currentRecording, tempName);
#else

        if (File.Exists(Application.persistentDataPath + tempName))
        {
            SaveRecording(currentRecording, count + 1);
        }
        else
        {
            //Write the data in an .xml file and save it locally.
            FileStream file = File.Create(Application.persistentDataPath + tempName);
            XmlSerializer xS = new XmlSerializer(typeof(List<saveData>));
            TextWriter tW = new StreamWriter(file);
            xS.Serialize(tW, currentRecording);
            Debug.Log("Saved");
        }
#endif
    }

    public List<saveData> LoadRecording(int count = 0)
    {
        string tempName;

        //Check which file shall be opened.
        if (count < 0)
        {
            tempName = "test";
        }
        else
        {
            tempName = "test" + count.ToString();
        }

#if NETFX_Core
        return XmlIO.ReadObjectFromXmlFileAsync<List<saveData>>(tempName);
#else
        if (File.Exists(Application.persistentDataPath + tempName))
        {
            //Open the chosen file with an .xml reader.
            //Store the data in the file in our temporary data record.
            //New records will be added to this data if not wiped.
            //Saving will produce a new file. Old data cannot be overwritten.
            FileStream file = File.Open(Application.persistentDataPath + tempName, FileMode.Open);
            XmlSerializer xS = new XmlSerializer(typeof(List<saveData>));
            TextReader tR = new StreamReader(file);
            List<saveData> tempList = (List<saveData>)xS.Deserialize(tR);
            return tempList;
        }
        else
        {
            return null;
        }
#endif
    }
}


//The serializable custom class in which the gathered data will be stored, one instance for each step.
[Serializable]
public class saveData
{
    public float TimeStamp;

    public Vector3 HeadPosition;
    public Vector3 GazeDirection;
    public bool CastHit;

    public saveData()
    {

    }

    public saveData(Vector3 hP, Vector3 gD, bool cH, int tS)
    {
        HeadPosition = hP;
        GazeDirection = gD;
        CastHit = cH;
        TimeStamp = tS * 0.04f;
    }
}