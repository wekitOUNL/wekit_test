using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WEKITAnnotationEditor : MonoBehaviour {

    TouchScreenKeyboard keyboard;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
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


    void RemoveAnnotation()
    {
        Destroy(this.gameObject);
    }


    void EditAnnotation()
    {
        TextMesh textMesh = this.gameObject.GetComponentInChildren<TextMesh>();
        keyboard = new TouchScreenKeyboard(textMesh.text, TouchScreenKeyboardType.Default, false, false, false, false, "Edit Annotation");

    }

}
