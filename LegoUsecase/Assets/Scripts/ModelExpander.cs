using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// class for expanding the 3d model based on the parent gameobject which holds all the subcomponenet 3d models
/// The methods is called by the speech manager 
/// </summary>
public class ModelExpander : MonoBehaviour {
    /// <summary>
    /// array for holding all the 3d models
    /// </summary>
    GameObject[] modelObject;
    /// <summary>
    /// the parent object that holds all the 3d model
    /// </summary>
    GameObject parentObject;
    
	// Use this for initialization
	void Start () {
        modelObject = GameObject.FindGameObjectsWithTag("lego");
        parentObject = GameObject.FindGameObjectWithTag("LegoModel");
        
	}
    /// <summary>
    /// Method to expand the 3d model by racasting in the direction from the central point of the parent object in the direction of the child objects.
    /// </summary>
    void ExpandModel()
    {
        Vector3 rayVector = parentObject.GetComponent<Transform>().position;
       // Raycast in all the direction to the central point of the child objects to infinity and get the position at the specified value back and set it as the new point
        foreach (GameObject g in modelObject)
        {
           
            Vector3 tempVector = g.GetComponent<Transform>().position;
            Ray rayToTest = new Ray( rayVector, tempVector);
            //only assigned the x & y due to misbehaving of the z axis
            g.transform.position = new Vector3(rayToTest.GetPoint(1f).x, rayToTest.GetPoint(1f).y, g.GetComponent<Transform>().position.z);
            Debug.Log(g.transform.position);

        }
    }
}
