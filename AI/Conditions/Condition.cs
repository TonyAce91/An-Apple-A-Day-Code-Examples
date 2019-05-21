using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Condition : IBehaviour {

    public abstract BehaviourResult UpdateBehaviour(Agent agent);
    //public abstract void Enter();
    public virtual void Exit() { }
}
