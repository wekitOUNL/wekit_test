﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameMechanism
{
    public class GMFGazeManager : MonoBehaviour
    {
        [HideInInspector]
        public RaycastHit HitInfo;
        private GameObject _lastHit;
        public float MaxDistance=5;
        [HideInInspector]
        public bool Hit;
        [HideInInspector]
        public Vector3 CameraPos, CameraForward;
        // Update is called once per frame
        void Update()
        {
            CameraPos = Camera.main.transform.position;
            CameraForward = Camera.main.transform.forward;
            Hit = Physics.Raycast(CameraPos, CameraForward, out HitInfo,
                MaxDistance);
            if (Hit)
            {
                EnterTargets();
            }
            else
            {
                if (_lastHit != null)
                {
                    ExitTargets();
                    _lastHit = null;
                }
            }
        }

        void EnterTargets()
        {
            GameObject go = HitInfo.collider.gameObject;
            if (go != _lastHit)
            {
                _lastHit = go;
                Interactable_Gaze target = _lastHit.GetComponent<Interactable_Gaze>();
                if (target != null)
                {
                    target.Enter();
                }
            }
        }

        void ExitTargets()
        {
            Interactable_Gaze target = _lastHit.GetComponent<Interactable_Gaze>();
            if (target != null)
            {
                target.Exit();
            }
        }
    }
}