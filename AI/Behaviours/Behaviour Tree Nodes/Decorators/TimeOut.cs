using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This is a timeout decorator
/// </summary>
public class TimeOut : IBehaviour {

    public IBehaviour child;
    public float timeout, duration;

    public BehaviourResult UpdateBehaviour(Agent agent)
    {
        timeout -= agent.internalDeltaTime;
        if (timeout > 0)
            return BehaviourResult.FAILURE;
        timeout = duration;
        return child.UpdateBehaviour(agent);
    }

    public void Exit() { }
}
