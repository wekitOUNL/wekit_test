using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameMechanism
{
    public class Cursor : MonoBehaviour
    {
        public GMFGazeManager GazeManager;
        
        /// <summary>
        /// The rotation the cursor object will have when not hitting anything. Set at start.
        /// </summary>
        private Quaternion _standardRotation;

        void Start()
        {
            _standardRotation = transform.rotation;
        }

        void Update()
        {
            if (GazeManager.Hit)
            {
                transform.position = GazeManager.HitInfo.point;
                transform.rotation = Quaternion.FromToRotation(Vector3.up, GazeManager.HitInfo.normal);
            }
            else
            {
                transform.position = GazeManager.CameraPos + GazeManager.CameraForward * GazeManager.MaxDistance;
                transform.rotation = _standardRotation;
            }
        }
    }
}