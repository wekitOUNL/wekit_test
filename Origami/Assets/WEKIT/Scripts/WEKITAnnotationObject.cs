using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WEKITAnnotationObject : MonoBehaviour {

    public GameObject annotatedObject;

    private LineRenderer lineRenderer;

	// Use this for initialization
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

    // Update is called once per frame
    void Update () {
        if (annotatedObject != null)
        {
            lineRenderer.SetPosition(0, gameObject.transform.position);
            lineRenderer.SetPosition(1, annotatedObject.transform.position);
        }
    }


}
