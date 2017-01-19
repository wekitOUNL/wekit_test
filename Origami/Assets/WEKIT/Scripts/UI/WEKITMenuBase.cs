using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity;
using HoloToolkit.Unity.InputModule;

/// <summary>
/// handles context menus for game objects. 
/// initializes a set of menu items according to the provided menu item texts and function names.
/// </summary>
public class WEKITMenuBase : MonoBehaviour, IInputClickHandler {

    /// <summary>
    /// array of menu item texts.
    /// </summary>
    [Tooltip("Provide the menu item texts to display.")]
    public string[] menuItemTexts;

    /// <summary>
    /// array of menu item names. These are actually the functions to be called. 
    /// Make sure, this array contains the same amount of entries as menuItemTexts.
    /// </summary>
    [Tooltip("Provide the menu item names to use. These are also the functions called on click.")]
    public string[] menuItemNames;

    private static GameObject[] menuItems;

    private static float menuOffset = 0.03f; // spacial offset between to menu entries

    /// <summary>
    /// handles the input click to open the menu.
    /// </summary>
    /// <param name="eventData"></param>
    public void OnInputClicked(InputEventData eventData)
    {
        //Debug.Log("WEKITMenuBase.OnInputClicked: " + this.gameObject.name + ", " + GazeManager.Instance.HitObject);
        OpenMenu();
    }

    /// <summary>
    /// opens the menu. Creates the menu items.
    /// </summary>
    void OpenMenu()
    {
        CloseMenu();
        if (WEKITGlobalsManager.Instance.MenuItem == null)
        {
            return;
        }

        Vector3 targetPoint = WEKITUtilities.placeObject(Camera.main.transform, 0.5f, 3.0f, 0.2f);

        menuItems = new GameObject[menuItemNames.Length];

        for (int i=0; i<menuItemNames.Length; i++)
        {
            menuItems[i] = GameObject.Instantiate(WEKITGlobalsManager.Instance.MenuItem);
            menuItems[i].SetActive(true);
            menuItems[i].AddComponent<Billboard>();
            menuItems[i].name = menuItemNames[i];
            menuItems[i].GetComponent<TextMesh>().text = menuItemTexts[i];
            WEKITMenuItem item = menuItems[i].GetComponent<WEKITMenuItem>();
            item.rootGameObject = this.gameObject;
            menuItems[i].transform.position = targetPoint;
            menuItems[i].transform.localPosition -= new Vector3(0, menuOffset * i, 0);
        }

    }

    /// <summary>
    /// closes the menu.
    /// </summary>
    public static void CloseMenu()
    {
        if (menuItems != null)
        {
            foreach (GameObject menuItem in menuItems)
            {
                Destroy(menuItem);
            }
        }

    }



}
