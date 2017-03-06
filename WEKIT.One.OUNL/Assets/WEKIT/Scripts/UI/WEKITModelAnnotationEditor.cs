using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// editor for 3D model based annotations.
/// Inherits from WEKITAnnotationBaseEditor.
/// </summary>

public class WEKITModelAnnotationEditor : WEKITAnnotationBaseEditor
{
    GameObject placeableModel;

    public void PlaceBox()
    {
        Debug.Log("3d model Annotation added");
        placeableModel = Resources.Load("Medieval/Models/Box") as GameObject;
        RenderModel(placeableModel);
    }

    public void PlaceBed()
    {
        Debug.Log("3d model Annotation added");
        placeableModel = Resources.Load("Medieval/Models/Bed") as GameObject;
        RenderModel(placeableModel);

    }

    void RenderModel(GameObject g)
    {
        foreach (Transform T in GetComponentsInChildren<Transform>())
        {
            T.GetComponent<Renderer>().enabled = false;
        }
        gameObject.GetComponent<MeshFilter>().mesh = g.GetComponent<MeshFilter>().mesh;
        gameObject.GetComponent<Renderer>().enabled = true;
        gameObject.SetActive(true);
    }

}
