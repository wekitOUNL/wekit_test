using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.InputModule;

/*
 * represents a menu item. A menu item is instantiated by WEKITMenuBase, 
 * which is the object for which the menu has been opened.
 */
public class WEKITMenuItem : MonoBehaviour, IFocusable, IInputClickHandler {

    public GameObject rootGameObject { get; set; } // the game object this menu is opened for

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}


    public void OnFocusEnter()
    {
        //Debug.Log("WEKITMenuItem.OnFocusEnter: " + this.gameObject.name + ", " + GazeManager.Instance.HitObject);
        this.gameObject.GetComponent<TextMesh>().color = Color.yellow;
    }

    public void OnFocusExit()
    {
        //Debug.Log("WEKITMenuItem.OnFocusExit: " + this.gameObject.name + ", " + GazeManager.Instance.HitObject);
        this.gameObject.GetComponent<TextMesh>().color = Color.white;
    }

    public void OnInputClicked (InputEventData eventData)
    {
        //Debug.Log("WEKITMenuItem.OnInputClicked: " + eventData.ToString() + ", " + this.gameObject.name + ", " + GazeManager.Instance.HitObject);
        this.gameObject.GetComponent<TextMesh>().color = Color.green;
        string name = this.gameObject.name;
        this.PerformMenuAction(name);
    }

    void PerformMenuAction(string message)
    {
        if (rootGameObject != null)
        {
            rootGameObject.SendMessage(message);
        }
        WEKITMenuBase.CloseMenu();
    }

}
