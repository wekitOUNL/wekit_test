using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity;
using HoloToolkit.Unity.InputModule;

public class WEKITMenuBase : MonoBehaviour, IInputClickHandler {

    [Tooltip("Drag the MenuItem prefab asset you want to create the menu with.")]
    public GameObject MenuItem;

    [Tooltip("Provide the menu item texts to display.")]
    public string[] menuItemTexts;

    [Tooltip("Provide the menu item names to use. These are also the functions called on click.")]
    public string[] menuItemNames;

    private static GameObject[] menuItems;

    public void OnInputClicked(InputEventData eventData)
    {
        //Debug.Log("WEKITMenuBase.OnInputClicked: " + this.gameObject.name + ", " + GazeManager.Instance.HitObject);
        OpenMenu();
    }

    void OpenMenu()
    {
        CloseMenu();
        if (MenuItem == null)
        {
            return;
        }

        Vector3 headPosition = Camera.main.transform.position;
        Vector3 gazeDirection = Camera.main.transform.forward;
        RaycastHit hitInfo;
        Physics.Raycast(headPosition, gazeDirection, out hitInfo);

        gazeDirection.Normalize();

        menuItems = new GameObject[menuItemNames.Length];

        for (int i=0; i<menuItemNames.Length; i++)
        {
            menuItems[i] = GameObject.Instantiate(MenuItem);
            menuItems[i].SetActive(true);
            menuItems[i].AddComponent<Billboard>();
            menuItems[i].name = menuItemNames[i];
            menuItems[i].GetComponent<TextMesh>().text = menuItemTexts[i];
            WEKITMenuItem item = menuItems[i].GetComponent<WEKITMenuItem>();
            item.rootGameObject = this.gameObject;
            menuItems[i].transform.position = hitInfo.point;
            menuItems[i].transform.position -= gazeDirection * 0.2f;
            menuItems[i].transform.localPosition -= new Vector3(0, 0.025f * i, 0);
        }

    }

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
