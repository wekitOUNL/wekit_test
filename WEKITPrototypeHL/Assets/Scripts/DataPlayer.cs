using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataPlayer : MonoBehaviour
{
	void Start ()
	{
        
	}
	
	void Update ()
	{

	}


    //This function takes a list of SaveData and distributes them accordingly.
    public void Activate(List<SaveData> tempRecords)
    {
        //Turns the ghost model visible.
        GetComponent<MeshRenderer>().enabled = true;
        transform.GetChild(0).GetComponent<MeshRenderer>().enabled = true;

        //Gives each entry of the recorded data to the PlayOverTimeCoroutine
        foreach (SaveData entry in tempRecords)
        {
            StartCoroutine(PlayOverTime(entry));
        }
    }

    //This function sets the position of the ghost to match the recorded data
    void Play(SaveData tempData)
    {
        GetComponent<Transform>().transform.position = tempData.HeadPosition;
        GetComponent<Transform>().transform.rotation = Quaternion.LookRotation(tempData.GazeDirection);
    }

    //This Coroutine plays the data saved in the SaveData list using their timestamps.
    IEnumerator PlayOverTime(SaveData tempData)
    {
        yield return new WaitForSeconds(tempData.TimeStamp);
        Play(tempData);
    }
}