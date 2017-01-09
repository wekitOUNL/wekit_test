using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WEKITAnnotationBaseEditor : MonoBehaviour {

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


    public void RemoveAnnotation()
    {
        RemoveAnnotationLink();
        Destroy(this.gameObject);
    }


}
