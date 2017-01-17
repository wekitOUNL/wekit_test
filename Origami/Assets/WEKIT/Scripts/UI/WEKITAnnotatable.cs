using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity;
using System;

/// <summary>
/// an annotatable object is an object placed in 3D space, which can be extended with various kinds of annotations.
/// Annotatable objects correspond to a single task place, where several annoations guide task execution.
/// </summary>
public class WEKITAnnotatable : WEKITFocusableObject {

    private bool isActive = false;
    private List<GameObject> annotations = new List<GameObject>();

    /// <summary>
    /// activates this object and declares it to be the activeAnnotationObject in WEKITGlobalsManager.
    /// </summary>
    public void SetActive()
    {
        if(isActive)
        {
            return;
        }
        WEKITGlobalsManager.Instance.activeAnnotationObject = this.gameObject;
        try
        {
            Transform t = transform.Find("DirectionAnchor");
            if (t != null)
            {
                GameObject indicator = t.gameObject;
                indicator.GetComponent<Renderer>().enabled = true;
                indicator.GetComponent<DirectionIndicator>().DirectionIndicatorObject.SetActive(true);
                indicator.SetActive(true);
            }

        } catch(Exception e)
        {
            Debug.Log("Could not set direction indicator: " + e.Message);
            Debug.Log("StackTrace: " + e.StackTrace);
        }
        try
        {
            foreach (Material mat in this.gameObject.GetComponent<Renderer>().materials)
            {
                mat.SetFloat("_Highlight", 0.5f);
            }
        }
        catch (Exception e)
        {
            Debug.Log("Could not highlight material: " + e.Message);
            Debug.Log("StackTrace: " + e.StackTrace);
        }
        isActive = true;
    }


    /// <summary>
    /// sets this object to be inactive. Should not be called directly. 
    /// Rather set the new active object via WEKITGlobalsManager.activeAnnotationObject property.
    /// </summary>
    public void SetInactive()
    {
        if(!isActive)
        {
            return;
        }
        isActive = false;
        try
        {
            foreach (Material mat in this.gameObject.GetComponent<Renderer>().materials)
            {
                mat.SetFloat("_Highlight", 0f);
            }
        }
        catch (Exception e)
        {
            Debug.Log("Could not reset material highlighting: " + e.Message);
            Debug.Log("StackTrace: " + e.StackTrace);
        }
        try
        {
            Transform t = transform.Find("DirectionAnchor");
            if (t != null)
            {
                GameObject indicator = t.gameObject;
                indicator.GetComponent<Renderer>().enabled = false;
                indicator.GetComponent<DirectionIndicator>().DirectionIndicatorObject.SetActive(false);
                indicator.SetActive(false);
            }
        }
        catch (Exception e)
        {
            Debug.Log("Could not remove direction indicator: " + e.Message);
            Debug.Log("StackTrace: " + e.StackTrace);
        }
    }


    /// <summary>
    /// adds a new annotation to this object. 
    /// The annotation will be placed in the user's gaze direction in front of this object. 
    /// </summary>
    /// <param name="AnnotationPrefab">the prefab to use as annotation.</param>
    /// <returns></returns>
    private GameObject AddAnnotation(GameObject AnnotationPrefab)
    {
        //Debug.Log("WEKITAnnotatable.AddAnnotation");
        GameObject annotationObject = (GameObject)Instantiate(AnnotationPrefab);

        WEKITAnnotationObject ao = annotationObject.AddComponent<WEKITAnnotationObject>();
        ao.annotatedObject = this.gameObject;
        annotations.Add(annotationObject);

        // Do a raycast into the world based on the user's
        // head position and orientation.
        Vector3 targetPosition = WEKITUtilities.placeObject(Camera.main.transform, 0.5f, 3.0f, 0.2f);
        annotationObject.transform.position = targetPosition;

        MeshRenderer meshRenderer = annotationObject.gameObject.GetComponentInChildren<MeshRenderer>();
        meshRenderer.enabled = true;

        return annotationObject;

    }


    /// <summary>
    /// adds a new text annotation to this object. 
    /// The annotation will be placed in the user's gaze direction in front of this object.
    /// The text annotation will be supplied with default text.
    /// </summary>
    void AddTextAnnotation() {
        //Debug.Log("WEKITAnnotatable.AddAnnotation");
        GameObject annotationObject = AddAnnotation(WEKITGlobalsManager.Instance.annotationObject);

        TextMesh textMesh = annotationObject.gameObject.GetComponentInChildren<TextMesh>();
        textMesh.text = "Annotation added here ";
        textMesh.text += "\n\r" + annotationObject.transform.position.ToString();
    }


    /// <summary>
    /// adds a new audio annotation to this object. 
    /// The annotation will be placed in the user's gaze direction in front of this object. 
    /// </summary>
    void AddAudioAnnotation()
    {
        //Debug.Log("WEKITAnnotatable.AddAnnotation");
        GameObject annotationObject = AddAnnotation(WEKITGlobalsManager.Instance.audioAnnotationObject);
    }


    /// <summary>
    /// removes the annotation from this object. 
    /// Should not be called directly, use WEKITAnnotationEditor.RemoveAnnotation instead.
    /// </summary>
    /// <param name="annotationObject">the annotation to remove</param>
    public void RemoveAnnotation(GameObject annotationObject)
    {
        annotations.Remove(annotationObject);
    }

    /// <summary>
    /// deletes this object. Also deletes all annotations belonging to this object.
    /// </summary>
    void DeleteObject()
    {
        // TODO: only removes one.
        foreach(GameObject annotation in annotations)
        {
            try
            {
                annotation.GetComponent<WEKITAnnotationBaseEditor>().RemoveAnnotationLink();
                Destroy(annotation);
            }
            catch (Exception e)
            {
                Debug.Log("Could not remove annotation from annotated object: " + e.Message);
                Debug.Log("StackTrace: " + e.StackTrace);
            }
        }
        Destroy(this.gameObject);
    }


}
