using System.Collections.Generic;
using UnityEngine;

public class WayPoint : MonoBehaviour
{
    private List<Transform> waypoints = new List<Transform>();

    private int lastIndex;

    private void Awake()
    {
        FillWaypoints();
        lastIndex = Random.Range(0, waypoints.Count);
    }

    private void FillWaypoints()
    {
        for (int i = 0; i < this.transform.childCount; i++)
        {
            waypoints.Add(this.transform.GetChild(i));
        }
    }

    public Vector3 GetRandomWayPoint()
    {
        var absoluteWaypoints = new List<Transform>(waypoints);

        absoluteWaypoints.RemoveAt(lastIndex);

        var randomWaypointIndex = Random.Range(0, absoluteWaypoints.Count);

        lastIndex = randomWaypointIndex;

        return absoluteWaypoints[randomWaypointIndex].position;
    }

}
