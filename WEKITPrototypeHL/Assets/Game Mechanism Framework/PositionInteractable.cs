using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameMechanism
{
    public class PositionInteractable : MonoBehaviour
    {
        public float Accuracy;

        public virtual void Enter ()
        {
            Debug.Log("Entered range");
        }

       public virtual void Exit ()
        {
            Debug.Log("Exited range");
        }
    }
}