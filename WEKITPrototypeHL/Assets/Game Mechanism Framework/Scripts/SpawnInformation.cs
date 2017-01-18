//Information taken from HoloToolkit examples (LevelSolver) and modified to be more generic - in the examples, the half dimensions are a float randomly generated within a range.

using System;
using System.Collections;
using System.Collections.Generic;
using HoloToolkit.Unity;
using UnityEngine;

namespace GameMechanism
{
    public struct PlacementQuery
    {
        public PlacementQuery(
            SpatialUnderstandingDllObjectPlacement.ObjectPlacementDefinition placementDefinition,
            List<SpatialUnderstandingDllObjectPlacement.ObjectPlacementRule> placementRules = null,
            List<SpatialUnderstandingDllObjectPlacement.ObjectPlacementConstraint> placementConstraints = null)
        {
            PlacementDefinition = placementDefinition;
            PlacementRules = placementRules;
            PlacementConstraints = placementConstraints;
        }

        public SpatialUnderstandingDllObjectPlacement.ObjectPlacementDefinition PlacementDefinition;
        public List<SpatialUnderstandingDllObjectPlacement.ObjectPlacementRule> PlacementRules;
        public List<SpatialUnderstandingDllObjectPlacement.ObjectPlacementConstraint> PlacementConstraints;
    }

    public class SpawnInformation : MonoBehaviour
    {
        public PlacementQuery OnFloor(Vector3 halfDims)
        {
            return new PlacementQuery(SpatialUnderstandingDllObjectPlacement.ObjectPlacementDefinition.Create_OnFloor(new Vector3(halfDims.x, halfDims.y, halfDims.z/* * 2.0f*/)),
                                    new List<SpatialUnderstandingDllObjectPlacement.ObjectPlacementRule>() {
                                            SpatialUnderstandingDllObjectPlacement.ObjectPlacementRule.Create_AwayFromOtherObjects(Math.Max(halfDims.x,halfDims.z) * 3.0f),
                                    });
        }
    }

    public enum PlacementTypes
    {
        
    }
}