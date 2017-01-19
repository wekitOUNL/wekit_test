using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Editor class for text annotations.
/// Provides functionality for text editing.
/// </summary>
public class WEKITAnnotationEditor : WEKITAnnotationBaseEditor {

    TouchScreenKeyboard keyboard;

	/// <summary>
    /// checks, if the keyboard is still active or if the keyboard text can be read.
    /// </summary>
	void Update () {
        if (keyboard != null && keyboard.active == false)
        {
            if (keyboard.done == true)
            {
                TextMesh textMesh = this.gameObject.GetComponentInChildren<TextMesh>();
                textMesh.text = keyboard.text;
                keyboard = null;
            }
        }

    }


    /// <summary>
    /// opens the TouchScreenKeyboard for TextEntry.
    /// </summary>
    void EditAnnotation()
    {
        TextMesh textMesh = this.gameObject.GetComponentInChildren<TextMesh>();
        keyboard = new TouchScreenKeyboard(textMesh.text, TouchScreenKeyboardType.Default, false, false, false, false, "Edit Annotation");
    }


    /// <summary>
    /// overloading WEKITFocusable to change text color.
    /// </summary>
    public new void OnFocusEnter()
    {
        base.OnFocusEnter();
        //Debug.Log("WEKITMenuItem.OnFocusEnter: " + this.gameObject.name + ", " + GazeManager.Instance.HitObject);
        this.gameObject.GetComponent<TextMesh>().color = FocusColor;
    }

    /// <summary>
    /// overloading WEKITFocusable to change text color.
    /// </summary>
    public new void OnFocusExit()
    {
        base.OnFocusExit();
        //Debug.Log("WEKITMenuItem.OnFocusExit: " + this.gameObject.name + ", " + GazeManager.Instance.HitObject);
        this.gameObject.GetComponent<TextMesh>().color = Color.white;
    }



}
