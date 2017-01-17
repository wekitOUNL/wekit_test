using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// class to be attached to any annotation prefab.
/// makes sure, line between annotation and annotated object is drawn.
/// </summary>
public class WEKITAnnotationObject : MonoBehaviour {

    public GameObject annotatedObject;

    private LineRenderer lineRenderer;

    /// <summary>
    /// initialzes the line renderer.
    /// </summary>
    void Start () {
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.material = new Material(Shader.Find("Standard"));
        lineRenderer.widthMultiplier = 0.001f;
        lineRenderer.startWidth = 0.001f;
        lineRenderer.endWidth = 0.001f;
        lineRenderer.startColor = Color.grey;
        lineRenderer.endColor = Color.grey;

        if (annotatedObject != null)
        {
            lineRenderer.SetPosition(0, gameObject.transform.position);
            lineRenderer.SetPosition(1, annotatedObject.transform.position);
        }
    }


    /// <summary>
    /// updates the line renderer to changed object positions.
    /// </summary>
    void Update () {
        if (annotatedObject != null)
        {
            lineRenderer.SetPosition(0, gameObject.transform.position);
            lineRenderer.SetPosition(1, annotatedObject.transform.position);
        }
    }


}
