using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WEKITAnnotatable : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}


    void AddAnnotation() {
        //Debug.Log("WEKITAnnotatable.AddAnnotation");
        GameObject annotationObject = (GameObject)Instantiate(WEKITGlobalsManager.Instance.annotationObject);

        // Do a raycast into the world based on the user's
        // head position and orientation.
        var headPosition = Camera.main.transform.position;
        var gazeDirection = Camera.main.transform.forward;

        RaycastHit hitInfo;
        MeshRenderer meshRenderer = annotationObject.gameObject.GetComponentInChildren<MeshRenderer>();
        meshRenderer.enabled = true;

        Physics.Raycast(headPosition, gazeDirection, out hitInfo);
        //Debug.Log("WEKITAnnotatable.AddAnnotation: found position: " + hitInfo.point.ToString());

        annotationObject.transform.position = hitInfo.point;
        gazeDirection.Normalize();
        annotationObject.transform.position -= gazeDirection * 0.2f;

        TextMesh textMesh = annotationObject.gameObject.GetComponentInChildren<TextMesh>();
        textMesh.text = "Annotation added here";
        textMesh.text += " " + annotationObject.transform.position.ToString();
    }
}
