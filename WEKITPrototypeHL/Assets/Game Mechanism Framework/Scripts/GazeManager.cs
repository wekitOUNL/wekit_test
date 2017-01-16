using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameMechanism
{
    public class GazeManager : MonoBehaviour
    {
        public RaycastHit HitInfo;
        private GameObject LastHit;
        public float MaxDistance;
        public bool hit = false;
        public Vector3 CameraPos;
        public Vector3 CameraForward;
        // Update is called once per frame
        void Update()
        {
            CameraPos = Camera.main.transform.position;
            CameraForward = Camera.main.transform.forward;
            hit = Physics.Raycast(CameraPos, CameraForward, out HitInfo,
                MaxDistance);
            if (hit)
            {
                HandleTargets();
            }
        }

        void HandleTargets()
        {
            GameObject go = HitInfo.collider.gameObject;
            if (go != LastHit)
            {
                LastHit = go;
                Interactable_Gaze target = LastHit.GetComponent<Interactable_Gaze>();
                if (target != null)
                {
                    target.Enter();
                }
            }
        }
    }
}