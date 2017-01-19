using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity;

/// <summary>
/// class to manage a context menu to popup, when no object has been clicked.
/// Inherits from Singleton<>.
/// </summary>
public class WEKITGlobalMenuActions : Singleton<WEKITGlobalMenuActions> {

    [Tooltip("Drag the gameObject prefab asset you want to display.")]
    public GameObject gameObjectPrefab;


    /// <summary>
    /// adds a new annotatable game object to the scene. 
    /// </summary>
    void AddGameObject()
    {
        //Debug.Log("WEKITAnnotatable.AddAnnotation");
        GameObject newObject = (GameObject)Instantiate(Instance.gameObjectPrefab);
        newObject.SetActive(true);

        // Do a raycast into the world based on the user's
        // head position and orientation.
        var headPosition = Camera.main.transform.position;
        var gazeDirection = Camera.main.transform.forward;

        MeshRenderer meshRenderer = newObject.gameObject.GetComponentInChildren<MeshRenderer>();
        meshRenderer.enabled = true;

        Vector3 targetPosition = WEKITUtilities.placeObject(Camera.main.transform, 0.5f, 3.0f, 0.2f);
        newObject.transform.position = targetPosition;

        // the new object is active by default.
        newObject.GetComponent<WEKITAnnotatable>().SetActive();

    }
}
