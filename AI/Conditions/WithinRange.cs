using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class is used to check if the target and agent are within the set range
/// 
/// Code written by Antoine Kenneth Odi in 2017.
/// </summary>
public class WithinRange : Condition
{

    private GameObject m_target;
    private float m_range = 0;
    // Temporary
    private string m_behaviourName;

    // Sets the target of the agent and the required range for the condition to be true
    public void SetParameters(GameObject target, float range, string behaviourName = "")
    {
        m_target = target;
        m_range = range;
        m_behaviourName = behaviourName;
    }

    // Updates the behaviour and returns success if the agent and target is within the set range
    public override BehaviourResult UpdateBehaviour(Agent agent)
    {
        if (m_target == null)
            return BehaviourResult.ERROR;

        if ((agent.gameObject.transform.position - m_target.transform.position).sqrMagnitude <= m_range * m_range)
        {
            return BehaviourResult.SUCCESS;
        }
        return BehaviourResult.FAILURE;
    }
}
