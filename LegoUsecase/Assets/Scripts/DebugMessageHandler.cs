using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugMessageHandler : MonoBehaviour {
    TextMesh textMesh;

    // Use this for initialization
    void Start()
    {
        textMesh = gameObject.GetComponentInChildren<TextMesh>();
    }

    void OnEnable()
    {
        Application.logMessageReceived += LogMessage;
    }

    void OnDisable()
    {
        Application.logMessageReceived -= LogMessage;
    }

    public void LogMessage(string message, string stackTrace, LogType type)
    {
        if (textMesh)
        {
            if (textMesh.text.Length > 10)
            {
                textMesh.text = " "+ message + "\n";
            }
            else
            {
                textMesh.text += " "+ message + "\n";
            }
        }
        else
        {
            Debug.Log("TextMesh not found");
        }
    }
}
