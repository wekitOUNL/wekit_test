using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.VR.WSA.WebCam;

public class WEKITPhotoAnnotationEditor : WEKITAnnotationBaseEditor {

    PhotoCapture photoCaptureObject = null;


    // Update is called once per frame
    void Update () {
		
	}


    void StartPhotoCapture()
    {
        //Debug.Log("StartPhotoCapture");
        PhotoCapture.CreateAsync(false, OnPhotoCaptureCreated);
    }

    void OnPhotoCaptureCreated(PhotoCapture captureObject)
    {
        //Debug.Log("OnPhotoCaptureCreated");
        photoCaptureObject = captureObject;

        Resolution cameraResolution = PhotoCapture.SupportedResolutions.OrderByDescending((res) => res.width * res.height).First();

        CameraParameters c = new CameraParameters();
        c.hologramOpacity = 0.0f;
        c.cameraResolutionWidth = cameraResolution.width;
        c.cameraResolutionHeight = cameraResolution.height;
        c.pixelFormat = CapturePixelFormat.BGRA32;

        //captureObject.StartPhotoModeAsync(c, false, OnPhotoModeStarted);
        captureObject.StartPhotoModeAsync(c, OnPhotoModeStarted);
    }

    private void OnPhotoModeStarted(PhotoCapture.PhotoCaptureResult result)
    {
        //Debug.Log("OnPhotoModeStarted");
        if (result.success)
        {
            photoCaptureObject.TakePhotoAsync(OnCapturedPhotoToMemory);
        }
        else
        {
            Debug.LogError("Unable to start photo mode!");
        }
    }

    void OnCapturedPhotoToMemory(PhotoCapture.PhotoCaptureResult result, PhotoCaptureFrame photoCaptureFrame)
    {
        //Debug.Log("OnCapturedPhotoToMemory: start");
        if (result.success)
        {
            //Debug.Log("OnCapturedPhotoToMemory: success");
            // Create our Texture2D for use and set the correct resolution
            Resolution cameraResolution = PhotoCapture.SupportedResolutions.OrderByDescending((res) => res.width * res.height).First();
            //Debug.Log("OnCapturedPhotoToMemory: create texture");
            Texture2D targetTexture = new Texture2D(cameraResolution.width, cameraResolution.height);
            // Copy the raw image data into our target texture
            //Debug.Log("OnCapturedPhotoToMemory: upload image to texture");
            photoCaptureFrame.UploadImageDataToTexture(targetTexture);
            // Do as we wish with the texture such as apply it to a material, etc.
            Debug.Log("OnCapturedPhotoToMemory: put it to the material (adjusted): " + cameraResolution.width + " " + cameraResolution.height);
            Transform photo = transform.Find("PhotoDisplay");
            Debug.Log("OnCapturedPhotoToMemory: old size: " + photo.localScale.x + " " + photo.localScale.y + " " + photo.localScale.z);
            Debug.Log("OnCapturedPhotoToMemory: old pos: " + photo.localPosition.x + " " + photo.localPosition.y + " " + photo.localPosition.z);
            photo.localScale = new Vector3((cameraResolution.width / 100), (cameraResolution.height / 100), photo.localScale.z);
            photo.localPosition = new Vector3(cameraResolution.width / 200, cameraResolution.height / 200, photo.localPosition.z);
            Debug.Log("OnCapturedPhotoToMemory: new size: " + photo.localScale.x + " " + photo.localScale.y + " " + photo.localScale.z);
            Debug.Log("OnCapturedPhotoToMemory: new pos: " + photo.localPosition.x + " " + photo.localPosition.y + " " + photo.localPosition.z);
            photo.gameObject.GetComponent<Renderer>().material.mainTexture = targetTexture;
            photo.transform.Rotate(new Vector3(0, 0, 180));

        }
        // Clean up
        Debug.Log("OnCapturedPhotoToMemory: clean up");
        photoCaptureObject.StopPhotoModeAsync(OnStoppedPhotoMode);
    }


    void OnStoppedPhotoMode(PhotoCapture.PhotoCaptureResult result)
    {
        Debug.Log("OnCapturedPhotoToMemory");
        photoCaptureObject.Dispose();
        photoCaptureObject = null;
    }


}
