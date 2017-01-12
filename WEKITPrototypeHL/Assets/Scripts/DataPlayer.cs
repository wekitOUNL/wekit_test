using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataPlayer : MonoBehaviour
{
    public GameObject Hand1;
    public GameObject Hand2;

    public bool isPlaying = false;
    public int currentFrame = 0;


    float startTime = 0;
    List<SaveData> myRecording;

	void Start ()
	{
        
	}
	
	void Update ()
	{
        //If the Class is supposed to play the recorded data, isPlaying is true and the
        //code will be executed (if the end of the recording is  not reached yet).
        if (isPlaying)
        {
            if (currentFrame < myRecording.Count)
            {
                //Determines at which time the starting frame lies and plays a new
                //frame every 0.04 seconds.
                currentFrame = (int)((Time.time - startTime) / 0.04f);
                Play(myRecording[currentFrame]);
            }
            else
            {
                isPlaying = false;
            }
        }
    }


    //This function takes a list of SaveData and distributes them accordingly.
    public void Activate(List<SaveData> tempRecords)
    {
        //Sets the current data for the Play function and starts it.
        currentFrame = 0;
        myRecording = tempRecords;
        startTime = Time.time;
        isPlaying = true;

        //Turns the ghost model visible.
        GetComponent<MeshRenderer>().enabled = true;
        transform.GetChild(0).GetComponent<MeshRenderer>().enabled = true;
        transform.GetChild(1).GetComponent<MeshRenderer>().enabled = true;
        Hand1.GetComponent<MeshRenderer>().enabled = true;
        Hand2.GetComponent<MeshRenderer>().enabled = true;
    }

    //This function stops playing the recording; or if it was already stopped
    //will start playing from the frame it was stopped on.
    public void Stop()
    {
        if(isPlaying)
        {
            isPlaying = false;
        }
        else
        {
            startTime = Time.time - currentFrame * 0.04f;
            isPlaying = true;
        }
    }

    //This function sets the position of the ghost to match the recorded data
    void Play(SaveData tempData)
    {
        GetComponent<Transform>().transform.position = tempData.HeadPosition;
        GetComponent<Transform>().transform.rotation = Quaternion.LookRotation(tempData.GazeDirection);

        Hand1.GetComponent<Transform>().transform.position = tempData.HandPosition1;
        Hand2.GetComponent<Transform>().transform.position = tempData.HandPosition2;
    }
}