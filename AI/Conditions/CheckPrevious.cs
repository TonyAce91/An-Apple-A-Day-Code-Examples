using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPrevious : Condition {

    private string m_prevBehaviour = "";

    public void SetParameters(string prevBehaviour)
    {
        m_prevBehaviour = prevBehaviour;
    }

    public override BehaviourResult UpdateBehaviour(Agent agent)
    {
        if (m_prevBehaviour == "")
        {
            Debug.Log("Previous Behaviour not set");
            return BehaviourResult.ERROR;
        }

        if (agent.prevBehaviour == m_prevBehaviour)
            return BehaviourResult.SUCCESS;

        return BehaviourResult.FAILURE;
    }
}
