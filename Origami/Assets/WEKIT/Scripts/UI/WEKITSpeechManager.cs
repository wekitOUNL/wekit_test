using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Windows.Speech;
using HoloToolkit.Unity;
using HoloToolkit.Unity.InputModule;

/// <summary>
/// class to handle speech commands as alternative input.
/// 
/// </summary>
public class WEKITSpeechManager : Singleton<WEKITSpeechManager>
{
    KeywordRecognizer keywordRecognizer = null;
    Dictionary<string, System.Action> keywords = new Dictionary<string, System.Action>();

    /// <summary>
    /// initializes the list of keywords to listen to and starts listening.
    /// </summary>
    void Start()
    {
        keywords.Add("Menu", () =>
        {
            var focusObject = GazeManager.Instance.HitObject;
            if (focusObject != null)
            {
                // Call the OpenMenu method on just the focused object.
                focusObject.SendMessage("OpenMenu");
            } else
            {
                WEKITGlobalsManager.Instance.gameObject.SendMessage("OpenMenu");
            }
        });

        keywords.Add("Add Text", () =>
        {
            var focusObject = GazeManager.Instance.HitObject;
            if (focusObject != null)
            {
                // Call the OnDrop method on just the focused object.
                focusObject.SendMessage("AddTextAnnotation");
            }
        });

        keywords.Add("Remove Annotation", () =>
        {
            var focusObject = GazeGestureManager.Instance.FocusedObject;
            if (focusObject != null)
            {
                // Call the OnDrop method on just the focused object.
                focusObject.SendMessage("RemoveAnnotation");
            }
        });

        // Tell the KeywordRecognizer about our keywords.
        keywordRecognizer = new KeywordRecognizer(keywords.Keys.ToArray());

        // Register a callback for the KeywordRecognizer and start recognizing!
        keywordRecognizer.OnPhraseRecognized += KeywordRecognizer_OnPhraseRecognized;
        keywordRecognizer.Start();
    }

    /// <summary>
    /// handler method for recognized keywords.
    /// </summary>
    /// <param name="args"></param>
    private void KeywordRecognizer_OnPhraseRecognized(PhraseRecognizedEventArgs args)
    {
        System.Action keywordAction;
        if (keywords.TryGetValue(args.text, out keywordAction))
        {
            keywordAction.Invoke();
        }
    }


    /// <summary>
    /// called, when microphone is needed for audio recording or other purposes.
    /// </summary>
    public void PauseRecognizer()
    {
        if (keywordRecognizer != null && keywordRecognizer.IsRunning)
        {
            keywordRecognizer.Stop();
        }
    }


    /// <summary>
    /// called, when microphone can be used for speech recognition again.
    /// </summary>
    public void ContinueRecognizer()
    {
        if (keywordRecognizer != null && !keywordRecognizer.IsRunning)
        {
            keywordRecognizer.Start();
        }
    }
}