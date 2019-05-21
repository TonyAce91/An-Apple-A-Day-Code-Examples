using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sequence : CompositeBehaviour
{
    // Updates behaviour
    public override BehaviourResult UpdateBehaviour(Agent agent)
    {
        foreach (IBehaviour child in m_childBehaviours)
        {
            if (child.UpdateBehaviour(agent) == BehaviourResult.FAILURE)
                return BehaviourResult.FAILURE;
        }
        return BehaviourResult.SUCCESS;
    }
}
