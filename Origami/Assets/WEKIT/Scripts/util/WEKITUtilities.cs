using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

/// <summary>
/// collection of global utility methods. 
/// </summary>
public class WEKITUtilities
{


    /// <summary>
    /// looks for an empty space to place a new object within a given range.
    /// </summary>
    /// <param name="headPosition">where to start searching</param>
    /// <param name="gazeDirection">in which direction to search</param>
    /// <param name="minDistance">how far away the new object should be placed at least</param>
    /// <param name="maxDistance">how far away the new object should be placed at most</param>
    /// <param name="minDistanceToObstacle">how far in front of an obstacle the new object should be placed</param>
    /// <returns>the place whre to put the new object.</returns>
    public static Vector3 placeObject(Vector3 headPosition, Vector3 gazeDirection, float minDistance, float maxDistance, float minDistanceToObstacle)
    {
        RaycastHit hitInfo;
        Vector3 targetPoint;
        if (minDistance>maxDistance || minDistance<0.0f)
        {
            return headPosition;
        }
        if (Physics.Raycast(headPosition+gazeDirection*minDistance, gazeDirection, out hitInfo, maxDistance-minDistance+minDistanceToObstacle))
        {
            targetPoint = hitInfo.point - gazeDirection * minDistanceToObstacle; // position menu in front of hit object
        }
        else
        {
            targetPoint = headPosition + gazeDirection * maxDistance; // position menu at distance maxDistance
        }

        return targetPoint;

    }


    /// <summary>
    /// looks for an empty space to place a new object within a given range.
    /// </summary>
    /// <param name="transform">where to start searching(transform.position) and in which direction to look(transform.forward)</param>
    /// <param name="minDistance">how far away the new object should be placed at least</param>
    /// <param name="maxDistance">how far away the new object should be placed at most</param>
    /// <param name="minDistanceToObstacle">how far in front of an obstacle the new object should be placed</param>
    /// <returns></returns>
    public static Vector3 placeObject(Transform transform, float minDistance, float maxDistance, float minDistanceToObstacle)
    {
        return placeObject(transform.position, transform.forward, minDistance, maxDistance, minDistanceToObstacle);
    }

}
