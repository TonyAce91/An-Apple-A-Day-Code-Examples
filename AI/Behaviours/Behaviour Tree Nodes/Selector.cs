using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selector : CompositeBehaviour {

    // Updates the behaviour
    public override BehaviourResult UpdateBehaviour(Agent agent)
    {
        foreach (IBehaviour child in m_childBehaviours)
        {
            if (child.UpdateBehaviour(agent) == BehaviourResult.SUCCESS)
            {
                // activates exit function of previous behaviour
                if (child != currentBehaviour && currentBehaviour != null)
                        currentBehaviour.Exit();

                // Sets a new current behaviour
                currentBehaviour = child;
                return BehaviourResult.SUCCESS;
            }
        }
        return BehaviourResult.FAILURE;
    }
}
