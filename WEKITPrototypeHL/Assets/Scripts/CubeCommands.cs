using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Put this script on any Object that should react to taps and put the function that should be triggered on
//the manager in the "FunctionName" field in the Unity editor inspector.
public class CubeCommands : MonoBehaviour
{
    //Name of the function that shall be called by interaction with this object and the responsible manager object.
    public string FunctionName;
    public GameObject DataManager;

    void Update()
    {
        //Start this objects referenced function with a keybinding.
        //if (Input.GetKeyDown("s"))
        //{
        //    DataManager.GetComponent<UIDisplayAPI>().SendMessageUpwards(FunctionName);
        //}
    }

	public void OnSelect()
    {
        //DEBUG: Transform the cube scale to see if this method was run properly.
        //GetComponent<Transform>().transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);

        //Run the method of the previously defined name of an UIDisplayAPI class.
        DataManager.GetComponent<UIDisplayAPI>().SendMessageUpwards(FunctionName);
    }
}