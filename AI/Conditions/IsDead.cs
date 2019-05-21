using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class is used to check if the agent is dead
/// 
/// Code written by Antoine Kenneth Odi in 2018.
/// </summary>
public class IsScarabAttacked : Condition
{
    private Scarab scarabReference;
    // Updates the behaviour and returns success if the agent and target is within the set range
    public override BehaviourResult UpdateBehaviour(Agent agent)
    {
        if (!scarabReference)
            scarabReference = agent as Scarab;
        if (scarabReference == null)
            return BehaviourResult.ERROR;

        if (scarabReference.attacked)
            return BehaviourResult.SUCCESS;
        return BehaviourResult.FAILURE;
    }
}
