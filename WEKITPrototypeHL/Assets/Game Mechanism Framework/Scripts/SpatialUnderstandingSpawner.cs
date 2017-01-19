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
        public SpawnInformation SpawnInformation;
        public SpawnInformation.PlacementTypes PlacementType;
        [Tooltip("Half dimensions of the object to be spawned")] public Vector3 HalfDims;


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

        //Should maybe be a coroutine or a System.Threading task if not run in Unity
        public void Spawn()
        {
            //Sollte nicht gemacht werden, bevor Scan fertig ist.
            if (_init)
            {
                Debug.Log("Spawning object");
                StartCoroutine(ObjectPlacement());
            }
            else
            {
                Debug.Log("Not initialized yet");
            }
        }

        IEnumerator ObjectPlacement()
        {
            SpawnInformation.PlacementQuery query = SpawnInformation.QueryByPlacementType(PlacementType, HalfDims);
            //Mit Definition nicht so sicher (Online-Beispiel ist falsch bzw. nicht komplett)
            if (SpatialUnderstandingDllObjectPlacement.Solver_PlaceObject(Prefab.name,
                    _understandingDll.PinObject(query.PlacementDefinition),
                    Rules.Count,
                    _understandingDll.PinObject(query.PlacementRules.ToArray()),
                    Constraints.Count,
                    _understandingDll.PinObject(query.PlacementConstraints.ToArray()),
                    _understandingDll.GetStaticObjectPlacementResultPtr()) > 0)
            {
                SpatialUnderstandingDllObjectPlacement.ObjectPlacementResult placementResult =
                    _understandingDll.GetStaticObjectPlacementResult();
                Quaternion rot = Quaternion.LookRotation(placementResult.Forward, Vector3.up);
                Instantiate(Prefab, placementResult.Position, rot);
            }
            else
            {
                Debug.Log("Couldn't spawn object");
            }
            yield return null;
        }

        public void Init_Spawner()
        {
            if (SpatialUnderstanding.Instance.ScanState == SpatialUnderstanding.ScanStates.Done)
            {
                SpatialUnderstandingDllObjectPlacement.Solver_Init();
                _init = true;
                Spawn();
            }
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