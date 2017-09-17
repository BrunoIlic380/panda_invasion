using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
    //stores the next waypoint in the chain, is visible in the inspector
    [SerializeField]
    private Waypoint nextWaypoint;

    //public method that retrieves the position of this waypoint
    public Vector3 GetPosition()
    {
        return transform.position;
    }

    //public method that retrieves the next waypoint in the chain
    public Waypoint GetNextWaypoint()
    {
        return nextWaypoint;
    }
    
}
