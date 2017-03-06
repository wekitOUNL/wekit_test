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
    public void Awake()
    {
        PlaceModel();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PlaceModel()
    {
        Debug.Log("3d model Annotation added");
        placeableModel = Resources.Load("TCZ Cars Free/Models/TCZ_MS_09") as GameObject;
        gameObject.GetComponent<MeshFilter>().mesh = placeableModel.GetComponent<Mesh>();
    }

}
