using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
/**
 * collection of useful utility functions.
 */
public class WEKITUtilities {

    /*
     * looks for an empty space to place a new object within a given range.
     * headPosition: where to start searching
     * gazeDirection: in which direction to look
     * minDistance: how far away the new object should be placed at least
     * maxDistance: how far away the new object should be placed at most
     * minDistanceToObstacle: how far in front of an obstacle the new object should be placed
     */
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

    /*
     * looks for an empty space to place a new object within a given range.
     * transform: where to start searching (transform.position) and in which direction to look (transform.forward)
     * minDistance: how far away the new object should be placed at least
     * maxDistance: how far away the new object should be placed at most
     * minDistanceToObstacle: how far in front of an obstacle the new object should be placed
     */
    public static Vector3 placeObject(Transform transform, float minDistance, float maxDistance, float minDistanceToObstacle)
    {
        return placeObject(transform.position, transform.forward, minDistance, maxDistance, minDistanceToObstacle);
    }

}
