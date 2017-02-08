using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class ModelExpander : MonoBehaviour {
    GameObject[] modelObject;
    GameObject parentObject;
    
	// Use this for initialization
	void Start () {
        modelObject = GameObject.FindGameObjectsWithTag("lego");
        parentObject = GameObject.FindGameObjectWithTag("LegoModel");
        
	}

    void ExpandModel()
    {
        Vector3 rayVector = parentObject.GetComponent<Transform>().position;
       
        foreach (GameObject g in modelObject)
        {
            Vector3 tempVector = g.GetComponent<Transform>().localPosition;
            Ray rayToTest = new Ray(rayVector, tempVector);
            g.transform.position = new Vector3(rayToTest.GetPoint(5.0f).x, rayToTest.GetPoint(5.0f).y, g.transform.position.z);



            //Vector3 heading = parentVector - g.transform.position;
            //float distance = heading.magnitude;
            //Vector3 direction = heading / distance;

            //g.transform.position = direction*distance;

        }
    }
 
    // Update is called once per frame
    void Update () {
		
	}
}
