using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity;

namespace GameMechanism
{
    public class SpatialUnderstandingSpawner : Singleton<SpatialUnderstandingSpawner>
    {
        public GameObject Prefab;

        //Currently the rules, constraints and definitions have to be set inside the script; at a later point, an editor script may be created so you can set them in the inspector
        public List<SpatialUnderstandingDllObjectPlacement.ObjectPlacementRule> Rules;
        public List<SpatialUnderstandingDllObjectPlacement.ObjectPlacementConstraint> Constraints;
        public SpatialUnderstandingDllObjectPlacement.ObjectPlacementDefinition Definition;
        private bool _init;
        private SpatialUnderstandingDll _understandingDll;

        // Use this for initialization
        void Start()
        {
            _understandingDll = SpatialUnderstanding.Instance.UnderstandingDLL;
            SpatialUnderstanding.Instance.ScanStateChanged += Init_Spawner;
        }

        // Update is called once per frame
        void Update()
        {
        }

        //Should maybe be a coroutine or a System.Threading task if not run in Unity
        public void Spawn()
        {
            //Sollte nicht gemacht werden, bevor Scan fertig ist.
            if (_init)
            {
                //Mit Definition nicht so sicher (Online-Beispiel ist falsch bzw. nicht komplett)
                if (SpatialUnderstandingDllObjectPlacement.Solver_PlaceObject(Prefab.name,
                        _understandingDll.PinObject(Definition),
                        Rules.Count,
                        _understandingDll.PinObject(Rules.ToArray()),
                        Constraints.Count,
                        _understandingDll.PinObject(Constraints.ToArray()),
                        _understandingDll.GetStaticObjectPlacementResultPtr()) > 0)
                {
                    SpatialUnderstandingDllObjectPlacement.ObjectPlacementResult placementResult =
                        _understandingDll.GetStaticObjectPlacementResult();
                    Quaternion rot = Quaternion.LookRotation(placementResult.Forward, Vector3.up);
                    GameObject newGameObject = Instantiate(Prefab, placementResult.Position, rot);
                }
            }
            else
            {
                Debug.Log("Not initialized yet");
            }
        }

        public void Init_Spawner()
        {
            SpatialUnderstandingDllObjectPlacement.Solver_Init();
            _init = true;
        }

        public void DestroyObjects()
        {
            if (_init)
            {
                SpatialUnderstandingDllObjectPlacement.Solver_RemoveObject(Prefab.name); 
            }
        }
    }
}