using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// base class providing core editing functionality for all annoation objects.
/// Inherits from WEKITFocusableObject to allow highlighting of editable objects.
/// Handles removal of annotations.
/// </summary>
public class WEKITAnnotationBaseEditor : WEKITFocusableObject {

    /// <summary>
    /// removes the link from the annotated object to this annotation.
    /// </summary>
    public void RemoveAnnotationLink()
    {
        try
        {
            this.gameObject.GetComponent<WEKITAnnotationObject>().annotatedObject.GetComponent<WEKITAnnotatable>().RemoveAnnotation(this.gameObject);
        }
        catch (Exception e)
        {
            Debug.Log("Could not remove annotation from annotated object: " + e.Message);
            Debug.Log("StackTrace: " + e.StackTrace);
        }
    }


    /// <summary>
    /// removes this annotation.
    /// </summary>
    public void RemoveAnnotation()
    {
        RemoveAnnotationLink();
        Destroy(this.gameObject);
    }


}
