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
        private UiPointer _uiPointer;

        private void Awake()
        {
            if (!UiPointerObject)
            {
                _uiPointer = Instantiate(UiPointerObject).GetComponent<UiPointer>();
                if (_uiPointer!=null)
                {
                    _uiPointer.TargetObject = gameObject;
                }
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