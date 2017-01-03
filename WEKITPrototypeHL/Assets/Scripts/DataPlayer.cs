using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataPlayer : MonoBehaviour
{
    List<SaveData> myRecording;
    int counter = 0;

	void Start ()
	{
        
	}
	
	void Update ()
	{
		
	}


    public void Activate(List<SaveData> tempRecords)
    {
        GetComponent<MeshRenderer>().enabled = true;
        GetComponentInChildren<MeshRenderer>().enabled = true;

        myRecording = tempRecords;
        foreach (SaveData entry in myRecording)
        {
            StartCoroutine(Wait(entry));
        }
    }

    void Play(SaveData tempData)
    {
        SaveData currentData = tempData;

        GetComponent<Transform>().transform.position = currentData.HeadPosition;
        GetComponent<Transform>().transform.rotation = Quaternion.Euler(currentData.GazeDirection);
    }

    IEnumerator Wait(SaveData tempData)
    {
        yield return new WaitForSeconds(tempData.TimeStamp);
        Play(tempData);
    }
}