using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// IBehaviour is an interface for all AI behaviour in game
/// </summary>


// This enum specifies the result of the behaviour if it succeed, failed or error
public enum BehaviourResult
{
    SUCCESS,
    FAILURE,
    ERROR
}

public interface IBehaviour {

    BehaviourResult UpdateBehaviour(Agent agent);

    //void Enter();
    void Exit();
}
