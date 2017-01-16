using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace GameMechanism
{
    public class Interactable : MonoBehaviour
    {
        public UnityEvent EnterEvents;

        [Tooltip("Should the gameObject be disabled after enter occurs once?")]
        public bool SingleUse=true; //Dedicated variable instead of a part of EnterEvents to ensure safe order of operations

        public void Enter()
        {
            Debug.Log("Enter");
            if (EnterEvents!= null)
            {
                EnterEvents.Invoke();
            }
            gameObject.SetActive(!SingleUse);
        }

        //Necessary for checkbox to show on component
        void Start()
        {
            
        }
    }
}