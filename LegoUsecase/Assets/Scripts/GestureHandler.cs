using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GestureHandler : MonoBehaviour {
    private bool isActive;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (isActive)
        {
            this.transform.Rotate(0, 1, 0);
        }
	}

    void OnAirTapped()
    {
        isActive = !isActive;
    }
}
