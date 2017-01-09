using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity;

public class WEKITGlobalMenuActions : Singleton<WEKITGlobalMenuActions> {

    [Tooltip("Drag the gameObject prefab asset you want to display.")]
    public GameObject gameObjectPrefab;


    void AddGameObject()
    {
        //Debug.Log("WEKITAnnotatable.AddAnnotation");
        GameObject newObject = (GameObject)Instantiate(Instance.gameObjectPrefab);

        // Do a raycast into the world based on the user's
        // head position and orientation.
        var headPosition = Camera.main.transform.position;
        var gazeDirection = Camera.main.transform.forward;

        MeshRenderer meshRenderer = newObject.gameObject.GetComponentInChildren<MeshRenderer>();
        meshRenderer.enabled = true;

        Vector3 targetPosition = WEKITUtilities.placeObject(Camera.main.transform, 0.5f, 3.0f, 0.2f);
        newObject.transform.position = targetPosition;

    }
}
