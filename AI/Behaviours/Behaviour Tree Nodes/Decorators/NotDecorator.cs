using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotDecorator : IBehaviour
{
    IBehaviour child;
    public void Exit()
    {    }

    public BehaviourResult UpdateBehaviour(Agent agent)
    {
        BehaviourResult result = child.UpdateBehaviour(agent);
        switch(result)
        {
            case BehaviourResult.SUCCESS: return BehaviourResult.FAILURE;
            case BehaviourResult.FAILURE: return BehaviourResult.SUCCESS;
            case BehaviourResult.ERROR: return BehaviourResult.ERROR;
        }
        throw new System.NotImplementedException();
    }
}
