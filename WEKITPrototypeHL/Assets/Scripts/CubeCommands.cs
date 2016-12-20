using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeCommands : MonoBehaviour
{
    //Name of the function that shall be called by interaction with this object and the responsible manager object.
    public string FunctionName;
    public GameObject DataManager;

    private void Update()
    {
        //Start this objects referenced function with a keybinding.
        if (Input.GetKeyDown("s"))
        {
            OnSelect();
        }
    }

	public void OnSelect()
    {
        //DEBUG: Transform the cube scale to see if this method was run properly.
        GetComponent<Transform>().transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);

        //Run the method of the previously defined name of the UIDisplayAPI class.
        DataManager.GetComponent<UIDisplayAPI>().SendMessageUpwards(FunctionName);
    }
}