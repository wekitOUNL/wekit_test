using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace GameMechanism
{
    public class InformationFilter : MonoBehaviour
    {

        public struct Filter
        {
            public float MinProximity;
            public UnityEvent MinProximityEvent;
            public float MaxProximity;
            public UnityEvent MaxProximityEvent;
        }

        private

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }

}