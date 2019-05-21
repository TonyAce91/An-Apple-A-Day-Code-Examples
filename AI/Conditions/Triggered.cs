using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Triggered : Condition {

    string m_triggerTag;

    public void SetParameters(string triggerTag)
    {

    }

    public override BehaviourResult UpdateBehaviour(Agent agent)
    {
        if (agent.triggered == true)
        {
            //agent.triggered = false;
            return BehaviourResult.SUCCESS;
        }

        return BehaviourResult.FAILURE;
    }
}
