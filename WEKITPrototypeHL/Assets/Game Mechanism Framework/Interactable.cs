using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace GameMechanism
{
    public class Interactable : MonoBehaviour
    {
        public UnityEvent EnterEvent;
        public GameObject UiPointerObject;

        private void Awake()
        {
            if (!UiPointerObject)
            {
                UiPointerObject = Instantiate(UiPointerObject);
            }
            else
            {
                Debug.Log("No UI Pointer assigned");
            }
        }

        public void Enter()
        {
            if (EnterEvent!= null)
            {
                EnterEvent.Invoke();
            }
            if (UiPointerObject)
            {
                UiPointerObject.SetActive(false);
            }
            gameObject.SetActive(false);
        }
    }
}