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
        private Quaternion standardRotation;

        void Start()
        {
            standardRotation = transform.rotation;
        }

        void Update()
        {
            if (GazeManager.hit)
            {
                transform.position = GazeManager.HitInfo.point;
                transform.rotation = Quaternion.FromToRotation(Vector3.up, GazeManager.HitInfo.normal);
            }
            else
            {
                transform.position = GazeManager.CameraPos + GazeManager.CameraForward * GazeManager.MaxDistance;
                transform.rotation = standardRotation;
            }
        }
    }
}