using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// Code written by Antoine Kenneth Odi in 2018

public class FaceTarget : IBehaviour {

    private GameObject m_target = null;
    private Scarab m_scarab = null;
    private float m_rotationSpeed = 1f;

    public void SetParameters(GameObject target, float rotationSpeed)
    {
        m_target = target;
        m_rotationSpeed = rotationSpeed;
    }

    public BehaviourResult UpdateBehaviour(Agent agent)
    {
        Quaternion lookAtTarget = Quaternion.LookRotation(m_target.transform.position - agent.transform.position, agent.transform.up);
        agent.transform.rotation = Quaternion.Slerp(agent.transform.rotation, lookAtTarget, agent.internalDeltaTime * m_rotationSpeed);
        return BehaviourResult.SUCCESS;
    }
    public void Exit() { }
}
