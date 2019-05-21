using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CompositeBehaviour : IBehaviour {

    protected LinkedList<IBehaviour> m_childBehaviours = new LinkedList<IBehaviour>();
    protected IBehaviour currentBehaviour;

    // Updates the behaviour, subclass behaviours need to override this
    public abstract BehaviourResult UpdateBehaviour(Agent agent);

    // Adds child behaviours
    public void addBehaviour(IBehaviour child)
    {
        m_childBehaviours.AddLast(child);
    }

    //public virtual void Enter() { }
    public virtual void Exit()
    {
        foreach (IBehaviour child in m_childBehaviours)
            child.Exit();
    }
}
