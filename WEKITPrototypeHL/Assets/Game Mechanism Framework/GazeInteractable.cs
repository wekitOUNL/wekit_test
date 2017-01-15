using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameMechanism
{
    public class GazeInteractable : MonoBehaviour
    {
        public float Accuracy;

        public virtual void Enter()
        {
            Debug.Log("Entered gaze");
        }

        public virtual void Exit()
        {
            Debug.Log("Exited gaze");
        }
    }
}