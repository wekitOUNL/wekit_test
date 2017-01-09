using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity;
using System;

public class WEKITAnnotatable : MonoBehaviour {

    private bool isActive = false;
    private List<GameObject> annotations = new List<GameObject>();

    /**
     * activates this object and declares it to be the activeAnnotationObject in WEKITGlobalsManager.
     */
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


    /**
     * sets this object to be inactive. Should not be called directly. 
     * Rather set the new active object via WEKITGlobalsManager.activeAnnotationObject property.
     */
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


    /**
     * adds a new annotation to this object. 
     * The annotation will be placed in the user's gaze direction in front of this object. 
     */
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


    /**
     * adds a new text annotation to this object. 
     * The annotation will be placed in the user's gaze direction in front of this object.
     * The text annotation will be supplied with default text.
     */
    void AddTextAnnotation() {
        //Debug.Log("WEKITAnnotatable.AddAnnotation");
        GameObject annotationObject = AddAnnotation(WEKITGlobalsManager.Instance.annotationObject);

        TextMesh textMesh = annotationObject.gameObject.GetComponentInChildren<TextMesh>();
        textMesh.text = "Annotation added here ";
        textMesh.text += "\n\r" + annotationObject.transform.position.ToString();
    }


    /**
     * adds a new audio annotation to this object. 
     * The annotation will be placed in the user's gaze direction in front of this object. 
     */
    void AddAudioAnnotation()
    {
        //Debug.Log("WEKITAnnotatable.AddAnnotation");
        GameObject annotationObject = AddAnnotation(WEKITGlobalsManager.Instance.audioAnnotationObject);
    }


    /**
     * removes the annotation from this object. 
     * Should not be called directly, use WEKITAnnotationEditor.RemoveAnnotation instead.
     */
    public void RemoveAnnotation(GameObject annotationObject)
    {
        annotations.Remove(annotationObject);
    }

    /**
     * deletes this object. Also deletes all annotations belonging to this object.
     */
    void DeleteObject()
    {
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
