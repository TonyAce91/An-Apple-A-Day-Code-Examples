using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This is the base class for all steering behaviours in game
/// 
/// Code written by Antoine Kenneth Odi in 2017.
/// </summary>

public class SteeringBehaviour : IBehaviour {

    // This is a list of all steering forces that produces this particular steering behaviour
    private LinkedList<SteeringForce> m_forces;

    // Use this for initialization
    public void Constructor()
    {
        m_forces = new LinkedList<SteeringForce>();
    }

    // This is used to add new force for a specific behaviour
    public void AddNewForce(SteeringForce newForce)
    {
        m_forces.AddLast(newForce);
    }

    // Updates the behaviour
    public BehaviourResult UpdateBehaviour(Agent agent)
    {
        foreach (SteeringForce force in m_forces)
            agent.AddForce(force.GetForce(agent));
        return BehaviourResult.SUCCESS;
    }
    public void Exit() { }
}
