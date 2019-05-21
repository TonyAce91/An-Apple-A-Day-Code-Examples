using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// This class is used for playing animation which can be added as part of behaviour tree
/// 
/// Code written by Antoine Kenneth Odi in 2018.
/// </summary>

public class PlayAnimation : IBehaviour {

    private string m_animationTriggerName;
    private Animator m_animator;
    private List<string> m_offTriggers = null;
    private UnityEvent m_behaviourEvent = null;

    public void SetParameters(Animator animator, string animationTriggerName, List<string> offTriggers = null, UnityEvent behaviourEvent = null)
    {
        m_animator = animator;
        m_animationTriggerName = animationTriggerName;
        m_offTriggers = offTriggers;
        m_behaviourEvent = behaviourEvent;
    }


    // Updates the behaviour
    public BehaviourResult UpdateBehaviour(Agent agent)
    {
        if (m_offTriggers != null)
            foreach (string trigger in m_offTriggers)
                m_animator.SetBool(trigger, false);

        if (m_behaviourEvent != null)
            m_behaviourEvent.Invoke();

        m_animator.SetBool(m_animationTriggerName, true);
        return BehaviourResult.SUCCESS;
    }
    public void Exit() {
        m_animator.SetBool(m_animationTriggerName, false);
    }
}
