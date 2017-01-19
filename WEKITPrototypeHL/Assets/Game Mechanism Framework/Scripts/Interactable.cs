using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace GameMechanism
{
    public class Interactable : MonoBehaviour
    {

        public enum DisableConditions
        {
            None,
            Enter,
            Exit
        }


        public DisableConditions DisableCondition; //Dedicated variable instead of a part of EnterEvents to ensure safe order of operations
        public UnityEvent EnterEvents;
        public UnityEvent ExitEvents;

        public void Enter()
        {
            if (EnterEvents != null)
            {
                EnterEvents.Invoke();
            }
            else
            {
                Debug.Log("Enter");
            }
            gameObject.SetActive(DisableCondition!=DisableConditions.Enter);
        }

        public void Exit()
        {
            if (ExitEvents != null)
            {
                ExitEvents.Invoke();
            }
            else
            {
                Debug.Log("Exit");
            }
            gameObject.SetActive(DisableCondition != DisableConditions.Exit);
        }

        //Necessary for checkbox to show on component
        void Start()
        {
            
        }
    }
}