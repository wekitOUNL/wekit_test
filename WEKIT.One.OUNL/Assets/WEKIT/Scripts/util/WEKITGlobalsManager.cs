﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity;

/// <summary>
/// manages global state and global settings.
/// </summary>
public class WEKITGlobalsManager : Singleton<WEKITGlobalsManager> {

    /// <summary>
    /// the prefab to be used for menu items.
    /// </summary>
    [Tooltip("Drag the MenuItem prefab asset you want to create the menu with.")]
    public GameObject MenuItem;

    /// <summary>
    /// the prefab to be used for text annotations.
    /// </summary>
    [Tooltip("Drag the Annotation prefab asset you want to display.")]
    public GameObject annotationObject;

    /// <summary>
    /// the prefab to be used for audio annotations.
    /// </summary>
    [Tooltip("Drag the Audio Annotation prefab asset you want to display.")]
    public GameObject audioAnnotationObject;

    /// <summary>
    /// the prefab to be used for sensor annotations.
    /// </summary>
    [Tooltip("Drag the Sensor Annotation prefab asset you want to display.")]
    public GameObject sensorAnnotationObject;

    /// <summary>
    /// the prefab to be used for photo annotations.
    /// </summary>
    [Tooltip("Drag the Photo Annotation prefab asset you want to display.")]
    public GameObject photoAnnotationObject;

    /// <summary>
    /// the prefab to be used as cursor.
    /// </summary>
    [Tooltip("Drag the cursor prefab asset you use.")]
    public GameObject Cursor;

    [Tooltip("Drag the direction indicator prefab asset you use.")]
    public GameObject DirectionIndicatorPrefab;

    /// <summary>
    /// @bibeg
    /// the prefab to be used for 3D model annotations.
    /// </summary>
    [Tooltip("Drag the AnnotationModel prefab asset you want to display.")]
    public GameObject modelAnnotationObject;

    /// <summary>
    /// contains the currently active annotatable object. 
    /// In the scene, directional pointers are used to direct the user's attention towards the active object. 
    /// </summary>
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
