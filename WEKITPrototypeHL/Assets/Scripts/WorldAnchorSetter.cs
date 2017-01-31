using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VR.WSA;
using HoloToolkit.Unity;

public class WorldAnchorSetter : MonoBehaviour
{
    public WorldAnchorManager myWAM;

	void Awake ()
	{
        myWAM.AttachAnchor(this.gameObject, "me");
    }
	
	void Update ()
	{
		
	}
}