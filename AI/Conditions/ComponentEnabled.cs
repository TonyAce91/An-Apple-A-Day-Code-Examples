using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class is used to check if a certain component is active
/// 
/// Code written by Antoine Kenneth Odi in 2018.
/// </summary>
public class ComponentEnabled : Condition
{

    private Component m_component;

    // Sets the component that needs to be checked
    public void SetParameters(Component component)
    {
        m_component = component;
    }

    // Updates the behaviour and returns success if the agent and target is within the set range
    public override BehaviourResult UpdateBehaviour(Agent agent)
    {
        if (m_component == null)
            return BehaviourResult.FAILURE;

        return BehaviourResult.SUCCESS;
    }
}
