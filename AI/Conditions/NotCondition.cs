using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// This class is a subclass of condition that reverses the result of the condition it has reference
/// 
/// Code written by Antoine Kenneth Odi in 2017.
/// </summary>

public class NotCondition : Condition
{

    private Condition m_currentCondition;


    // Sets a reference to the condition that requires the opposite result
    public void SetParameters(Condition currentCondition)
    {
        m_currentCondition = currentCondition;
    }

    // Updates the behaviour and returns the opposite result of referenced behaviour
    public override BehaviourResult UpdateBehaviour(Agent agent)
    {
        if (m_currentCondition == null)
            return BehaviourResult.ERROR;

        if (m_currentCondition.UpdateBehaviour(agent) == BehaviourResult.SUCCESS)
        {
            return BehaviourResult.FAILURE;
        }
        return BehaviourResult.SUCCESS;
    }
}
