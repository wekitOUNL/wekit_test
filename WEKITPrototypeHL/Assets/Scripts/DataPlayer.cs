using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataPlayer : MonoBehaviour
{
    bool isPlaying = false;
    int currentFrame = 0;

	void Start ()
	{
        
	}
	
	void Update ()
	{

	}


    //This function takes a list of SaveData and distributes them accordingly.
    public void Activate(List<SaveData> tempRecords)
    {
        isPlaying = true;

        //Turns the ghost model visible.
        GetComponent<MeshRenderer>().enabled = true;
        transform.GetChild(0).GetComponent<MeshRenderer>().enabled = true;
        transform.GetChild(1).GetComponent<MeshRenderer>().enabled = true;

        //Gives each entry of the recorded data to the PlayOverTimeCoroutine
        foreach (SaveData entry in tempRecords)
        {
            StartCoroutine(PlayOverTime(entry));
        }
    }

    //This Coroutine plays the data saved in the SaveData list using their timestamps.
    IEnumerator PlayOverTime(SaveData tempData)
    {
        yield return new WaitForSeconds(tempData.TimeStamp);
        if (isPlaying)
        {
            Play(tempData);
        }
    }

    //This function sets the position of the ghost to match the recorded data
    void Play(SaveData tempData)
    {
        GetComponent<Transform>().transform.position = tempData.HeadPosition;
        GetComponent<Transform>().transform.rotation = Quaternion.LookRotation(tempData.GazeDirection);
    }
}