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

    //Public variables.
    [HideInInspector]
    public Vector3[] HandPositions = new Vector3[] { Vector3.zero, Vector3.zero };
    [HideInInspector]
    public Vector3 headPosition;
    [HideInInspector]
    public Vector3 gazeDirection;
    [HideInInspector]
    public RaycastHit hitInfo;
    [HideInInspector]
    public bool castHit;
    [HideInInspector]
    public string output;

    void Awake()
    {

    }

    void Update()
    {
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

        //Display the collected information in the UI.
        output = castHit.ToString() +
                    Environment.NewLine + "pos:" + headPosition.ToString() +
                    Environment.NewLine + "dir:" + gazeDirection.ToString() +
                    Environment.NewLine + "h1:" + HandPositions[0].x + "," + HandPositions[0].y + "," + HandPositions[0].z +
                    Environment.NewLine + "h2:" + HandPositions[1].x + "," + HandPositions[1].y + "," + HandPositions[1].z +
                    Environment.NewLine + Time.time;

        UIText.GetComponent<Text>().text = output;
    }
}