using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity;

public class WEKITGlobalsManager : Singleton<WEKITGlobalsManager> {

    [Tooltip("Drag the MenuItem prefab asset you want to create the menu with.")]
    public GameObject MenuItem;

    [Tooltip("Drag the Annotation prefab asset you want to display.")]
    public GameObject annotationObject;

    [Tooltip("Drag the Audio Annotation prefab asset you want to display.")]
    public GameObject audioAnnotationObject;

    [Tooltip("Drag the cursor prefab asset you use.")]
    public GameObject Cursor;

    [Tooltip("Drag the direction indicator prefab asset you use.")]
    public GameObject DirectionIndicatorPrefab;

    private GameObject _activeAnnotationObject = null;
    public GameObject activeAnnotationObject
    {
        get
        {
            return _activeAnnotationObject;
        }
            
        set
        {
            if (_activeAnnotationObject != null && _activeAnnotationObject != value)
            {
                try
                {
                    _activeAnnotationObject.GetComponent<WEKITAnnotatable>().SetInactive();
                }
                catch (Exception e)
                {
                    Debug.Log("Could not inactivate activeAnnotationObject: " + _activeAnnotationObject.ToString());
                    Debug.Log("Exception: " + e.Message);
                    Debug.Log("StackTrace: " + e.StackTrace);
                }
            }
            _activeAnnotationObject = value;
        }
    }

}
