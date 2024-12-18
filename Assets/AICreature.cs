using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Creature : MonoBehaviour
{
    public Transform[] waypoints; // Array of waypoints for the seeking behavior

    public Transform[] GetWaypoints()
    {
        if (waypoints == null || waypoints.Length == 0)
        {
            Debug.LogError("Waypoints are not set or empty!");
        }
        return waypoints;
    }
}
