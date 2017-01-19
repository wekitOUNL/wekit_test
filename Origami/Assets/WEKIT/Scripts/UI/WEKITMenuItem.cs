using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.InputModule;

/// <summary>
/// represents a menu item. A menu item is instantiated by WEKITMenuBase, 
/// which is the object for which the menu has been opened.
/// </summary>
public class WEKITMenuItem : WEKITFocusableObject, IInputClickHandler {

    /// <summary>
    /// the game object this menu is opened for.
    /// </summary>
    public GameObject rootGameObject { get; set; }

    /// <summary>
    /// highlights this menu item. Overloads WEKITFocusableObject.
    /// </summary>
    public new void OnFocusEnter()
    {
        base.OnFocusEnter();
        //Debug.Log("WEKITMenuItem.OnFocusEnter: " + this.gameObject.name + ", " + GazeManager.Instance.HitObject);
        this.gameObject.GetComponent<TextMesh>().color = FocusColor;
    }

    /// <summary>
    /// resets highlighting for this menu item. Overloads WEKITFocusableObject.
    /// </summary>
    public new void OnFocusExit()
    {
        base.OnFocusExit();
        //Debug.Log("WEKITMenuItem.OnFocusExit: " + this.gameObject.name + ", " + GazeManager.Instance.HitObject);
        this.gameObject.GetComponent<TextMesh>().color = Color.white;
    }

    /// <summary>
    /// activates this menuItem.
    /// </summary>
    /// <param name="eventData"></param>
    public void OnInputClicked (InputEventData eventData)
    {
        //Debug.Log("WEKITMenuItem.OnInputClicked: " + eventData.ToString() + ", " + this.gameObject.name + ", " + GazeManager.Instance.HitObject);
        this.gameObject.GetComponent<TextMesh>().color = Color.green;
        string name = this.gameObject.name;
        this.PerformMenuAction(name);
    }

    /// <summary>
    /// performs the action associated with this menuItem. 
    /// Sends the message to the object this item has been opened for.
    /// </summary>
    /// <param name="message"></param>
    void PerformMenuAction(string message)
    {
        if (rootGameObject != null)
        {
            rootGameObject.SendMessage(message);
        }
        WEKITMenuBase.CloseMenu();
    }

}
