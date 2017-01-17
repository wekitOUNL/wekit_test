using UnityEngine;
using UnityEngine.VR.WSA.Input;
using HoloToolkit.Unity;
using HoloToolkit.Unity.InputModule;

/// <summary>
/// Singleton class to handle recognized gestures and forward the coresponding event to the object gazed at.
/// </summary>
public class WEKITGazeGestureManager : Singleton<WEKITGazeGestureManager>
{


    /// <summary>
    /// Represents the hologram that is currently being gazed at.
    /// </summary>
    public GameObject FocusedObject { get; private set; }

    GestureRecognizer recognizer;

    /// <summary>
    /// initializes the underlying GestureRecognizer and its event handler methods.
    /// </summary>
    void Start()
    {
        // Set up a GestureRecognizer to detect Select gestures.
        recognizer = new GestureRecognizer();
        recognizer.TappedEvent += (source, tapCount, ray) =>
        {
            // Send an OnSelect message to the focused object and its ancestors.
            if (FocusedObject != null)
            {
                FocusedObject.SendMessage("OnSelect");
                //                FocusedObject.SendMessageUpwards("OnSelect");
            }
        };
        recognizer.StartCapturingGestures();
    }


    /// <summary>
    /// checks for changes in focus objects.
    /// </summary>
    void Update()
    {
        // Figure out which hologram is focused this frame.
        GameObject oldFocusObject = FocusedObject;

        // Do a raycast into the world based on the user's
        // head position and orientation.
        var headPosition = Camera.main.transform.position;
        var gazeDirection = Camera.main.transform.forward;


        RaycastHit hitInfo = GazeManager.Instance.HitInfo;
        if (hitInfo.transform != null && hitInfo.transform.gameObject != null)
        {
            // If the raycast hit a hologram, use that as the focused object.
            FocusedObject = hitInfo.transform.gameObject;
        }
        else
        {
            // If the raycast did not hit a hologram, clear the focused object.
            FocusedObject = null;
        }

        // If the focused object changed this frame,
        // start detecting fresh gestures again.
        if (FocusedObject != oldFocusObject)
        {
            recognizer.CancelGestures();
            recognizer.StartCapturingGestures();
        }
    }
}