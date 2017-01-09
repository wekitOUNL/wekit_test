using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using HoloToolkit.Unity.InputModule;
using System;

[RequireComponent(typeof(AudioSource))]

public class WEKITAudioAnnotationEditor : WEKITAnnotationBaseEditor, IFocusable
{
    
    // set to false to use the holotoolkit microphone approach
    private bool useUnityMic = true; 
    
    /*** Unity Microphone settings ***/

    //A boolean that flags whether there's a connected microphone  
    private bool micConnected = false;

    //The maximum and minimum available recording frequencies  
    private int minFreq;
    private int maxFreq;



    /*** HoloToolkit Microphone settings ***/

    /// <summary>
    /// Which type of microphone/quality to access
    /// </summary>
    public MicStream.StreamCategory StreamType = MicStream.StreamCategory.HIGH_QUALITY_VOICE;

    /// <summary>
    /// can boost volume here as desired. 1 is default but probably too quiet. can change during operation. 
    /// </summary>
    public float InputGain = 1;

    /// <summary>
    /// if keepAllData==false, you'll always get the newest data no matter how long the program hangs for any reason, but will lose some data if the program does hang 
    /// can only be set on initialization
    /// </summary>
    public bool KeepAllData = false;

    /// <summary>
    /// Do you want to hear what the microphone sounds like by listening to the AudioSource in Unity?
    /// </summary>
    public bool ListenToAudioSource = true;

    /// <summary>
    /// The name of the file to which to save audio (for commands that save to a file)
    /// </summary>
    private string SaveFileName = "MicrophoneTest.wav";
    private string OutputPath = "";

    /// <summary>
    /// Records estimation of volume from the microphone to affect other elements of the game object
    /// </summary>
    private float averageAmplitude = 0;

    /// <summary>
    /// how small can our object be in this demo?
    /// </summary>
    private float minSize = .3f;

    //A handle to the attached AudioSource  
    private AudioSource goAudioSource;

    // default color of this mesh
    private Color color;

    private bool isRecording = false;

    private float[] audioBuffer;

    private int _numChannels = 2;

    //Use this for initialization  
    void Start()
    {
        //Get the attached AudioSource component  
        goAudioSource = this.GetComponent<AudioSource>();

        color = this.gameObject.GetComponent<Renderer>().material.color;
        foreach (Renderer rend in this.gameObject.GetComponentsInChildren<Renderer>())
        {
            color = rend.material.color;
        }
    }

    private void OnAudioFilterRead(float[] buffer, int numChannels)
    {
        if (useUnityMic)
        {
            return;
        }
        Debug.Log("OAFR." + buffer + "." + numChannels);
        // this is where we call into the DLL and let it fill our audio buffer for us
        CheckForErrorOnCall(MicStream.MicGetFrame(buffer, buffer.Length, numChannels));

        if (_numChannels != numChannels)
        {
            _numChannels = numChannels;
        }
        if (audioBuffer == null)
        {
            audioBuffer = buffer;
        } else
        {
            audioBuffer = audioBuffer.Concat(buffer).ToArray();
        }

        float sumOfValues = 0;
        // figure out the average amplitude from this new data
        for (int i = 0; i < buffer.Length; i++)
        {
            sumOfValues += Mathf.Abs(buffer[i]);
        }
        averageAmplitude = sumOfValues / buffer.Length;
    }


    private void Awake()
    {
        if (useUnityMic)
        {
            return;
        }
        if (!ListenToAudioSource)
        {
            this.gameObject.GetComponent<AudioSource>().volume = 0; // can set to zero to mute mic monitoring
        }

    }

    void Update()
    {
        if (isRecording)
        {
            CheckForErrorOnCall(MicStream.MicSetGain(InputGain));
            float blue = (minSize + averageAmplitude + 1 / minSize + 2) * 255;
            changeColor(new Color(255, 0, blue));
        } else if (goAudioSource.isPlaying)
        {
            changeColor(Color.green);
        }
    }


    void StartRecording()
    {
        WEKITSpeechManager.Instance.PauseRecognizer();
        Debug.Log("Stopped listening to voice commands.");
        isRecording = true;
        if (useUnityMic)
        {
            //Check if there is at least one microphone connected  
            if (Microphone.devices.Length <= 0)
            {
                //Throw a warning message at the console if there isn't  
                Debug.LogWarning("Microphone not connected!");
            }
            else //At least one microphone is present  
            {
                //Set 'micConnected' to true  
                micConnected = true;

                //Get the default microphone recording capabilities  
                Microphone.GetDeviceCaps(null, out minFreq, out maxFreq);

                //According to the documentation, if minFreq and maxFreq are zero, the microphone supports any frequency...  
                if (minFreq == 0 && maxFreq == 0)
                {
                    //...meaning 44100 Hz can be used as the recording sampling rate  
                    maxFreq = 44100;
                }

                //Get the attached AudioSource component  
                goAudioSource = this.GetComponent<AudioSource>();

                //If the audio from any microphone isn't being captured  
                if (!Microphone.IsRecording(null))
                {
                    //Start recording and store the audio captured from the microphone at the AudioClip in the AudioSource  
                    goAudioSource.clip = Microphone.Start(null, true, 20, maxFreq);
                }
            }

        }
        else
        {
            CheckForErrorOnCall(MicStream.MicInitializeCustomRate((int)StreamType, AudioSettings.outputSampleRate));
            CheckForErrorOnCall(MicStream.MicSetGain(InputGain));
            //CheckForErrorOnCall(MicStream.MicStartStream(KeepAllData, false));
            SaveFileName = "WEKIT_Audio_" + DateTime.Now.ToFileTimeUtc() + ".wav";
            CheckForErrorOnCall(MicStream.MicStartRecording(SaveFileName, false));
        }
        Debug.Log("Started audio recording");
        changeColor(Color.red);
    }


    void StopRecording()
    {
        isRecording = false;
        if (useUnityMic)
        {
            Microphone.End(null); //Stop the audio recording  

        }
        else
        {
            OutputPath = MicStream.MicStopRecording();
            Debug.Log("Saved audio recording to " + OutputPath + ", " + SaveFileName);
            CheckForErrorOnCall(MicStream.MicStopStream());
            //Debug.Log("Recorded " + audioBuffer.Length + " audio data entries.");
            //goAudioSource.clip = AudioClip.Create("Recording", audioBuffer.Length, _numChannels, AudioSettings.outputSampleRate, false);
            //goAudioSource.clip.SetData(audioBuffer, 0);
            CheckForErrorOnCall(MicStream.MicDestroy());
        }
        Debug.Log("Stopped audio recording.");
        WEKITSpeechManager.Instance.ContinueRecognizer();
        Debug.Log("Listening to voice commands again.");
        changeColor(color);
    }

    void PlayAudio()
    {
        if (isRecording)
        {
            StopRecording();
        }
        Debug.Log("Starting to play audio.");
        if (useUnityMic)
        {
            goAudioSource.Play(); //Playback the recorded audio
        }
        else
        {
            goAudioSource.clip = Resources.Load(OutputPath) as AudioClip;
            Debug.Log("Having " + goAudioSource.clip.length + " audio data entries.");
            goAudioSource.Play(); //Playback the recorded audio
        }
        changeColor(Color.green);
    }


    void StopAudio()
    {
        if (isRecording)
        {
            StopRecording();
        }
        else if (goAudioSource.isPlaying)
        {
            goAudioSource.Stop();
        }
        changeColor(color);
    }


    public void OnFocusEnter()
    {
        this.gameObject.transform.localScale *= 1.1f;
        changeColor(Color.yellow);
    }


    public void OnFocusExit()
    {
        this.gameObject.transform.localScale /= 1.1f;
        changeColor(color);
    }


    private void changeColor(Color color)
    {
        this.gameObject.GetComponent<Renderer>().material.color = color;
        foreach (Renderer rend in this.gameObject.GetComponentsInChildren<Renderer>())
        {
            rend.material.color = color;
        }
    }


    private void CheckForErrorOnCall(int returnCode)
    {
        MicStream.CheckForErrorOnCall(returnCode);
    }

#if DOTNET_FX
        // on device, deal with all the ways that we could suspend our program in as few lines as possible
        private void OnApplicationPause(bool pause)
        {
            if (pause)
            {
                CheckForErrorOnCall(MicStream.MicPause());
            }
            else
            {
                CheckForErrorOnCall(MicStream.MicResume());
            }
        }

        private void OnApplicationFocus(bool focused)
        {
            this.OnApplicationPause(!focused);
        }

        private void OnDisable()
        {
            this.OnApplicationPause(true);
        }

        private void OnEnable()
        {
            this.OnApplicationPause(false);
        }
#endif
}
