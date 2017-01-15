using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace GameMechanism
{
    public class DirectedFocusTarget : GazeInteractable
    {
        public UnityEvent Result;
        public bool DisableOnFocus;

        public override void Enter()
        {
            if (Result != null)
            {
                Result.Invoke();
            }
            if (DisableOnFocus)
            {
                this.enabled = false;
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}