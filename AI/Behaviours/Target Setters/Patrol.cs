using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patrol : TargetSetter {
    private List<Vector3> m_waypoints = new List<Vector3>();

    private int m_currentWaypointNumber = 0;
    private Vector3 m_currentTarget;
    private float targetDetectionDistance = 0.1f;

    private Vector3 m_closestWaypoint;
    private float m_closestDistance = float.MaxValue;


    private bool m_waypointValid = false;

    public void SetParameters(List<Vector3> waypoints)
    {
        m_waypoints = waypoints;
    }

    // Setting parameters when given a list of transform by turning them into vectors
    public void SetParameters(List<Transform> waypoints)
    {
        foreach(Transform waypoint in waypoints)
            m_waypoints.Add(waypoint.position);
        m_currentTarget = m_waypoints[0];
        m_currentWaypointNumber = 0;
    }

    public override void Initialise(Agent agent)
    {
        FindClosestNode(agent);
    }

    /// <summary>
    /// Use to initialise the patrol behaviour where the entity finds the closest waypoint and continue from there
    /// </summary>
    /// <param name="agent"></param>
    public void FindClosestNode (Agent agent)
    {
        // waypoint number counter which determines its position on the list
        int waypointNumber = 0;
        foreach (Vector3 waypoint in m_waypoints)
        {
            float waypointDistance = (agent.transform.position - waypoint).sqrMagnitude;

            // If closest waypoint found, save its information to compare with next iteration
            if (waypointDistance < m_closestDistance)
            {
                m_closestWaypoint = waypoint;
                m_closestDistance = waypointDistance;
                m_currentWaypointNumber = waypointNumber;
            }

            // Increments waypoint number for next iteration
            waypointNumber++;
        }
        m_currentTarget = m_closestWaypoint;
        Debug.Log("Patrolling");
        m_waypointValid = true;
    }

    public override Vector3 UpdateTarget(Agent agent)
    {
        if (!m_waypointValid)
            FindClosestNode(agent);

        Vector3 agentPos = agent.transform.position;

        // If target is not close enough, keep it as a target
        if ((agentPos - m_currentTarget).sqrMagnitude > targetDetectionDistance)
            return m_currentTarget;

        // Changes target if current target is close. Next target is the next waypoint of the patrol waypoints
        if (m_currentWaypointNumber < m_waypoints.Count - 1)
        {
            m_currentWaypointNumber++;
            m_currentTarget = m_waypoints[m_currentWaypointNumber];
        }
        else
        {
            m_currentWaypointNumber = 0;
            m_currentTarget = m_waypoints[0];
        }

        return m_currentTarget;
    }

    public override void Reset()
    {
        m_waypointValid = false;
    }

    public override bool FailureTest()
    {
        return false;
    }
}
