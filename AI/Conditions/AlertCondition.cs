using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class is used specifically in this game which toggles depending on alert events in game
/// 
/// Code written by Antoine Kenneth Odi in 2018.
/// </summary>

public class AlertCondition : Condition {

    // Updates Behaviour acts like Monobehaviour's update function
    public override BehaviourResult UpdateBehaviour(Agent agent)
    {
        if (agent.alerted)
        {
            //Debug.Log("Alert Condition succeed");
            return BehaviourResult.SUCCESS;
        }

        return BehaviourResult.FAILURE;
    }
}
