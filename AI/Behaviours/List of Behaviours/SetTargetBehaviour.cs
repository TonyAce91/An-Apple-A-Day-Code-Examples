using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

// Code written by Antoine Kenneth Odi in 2018

public class SetTargetBehaviour : IBehaviour {


    private Vector3 m_destination;
    private GameObject m_target;
    private bool m_isMovingTarget = false;
    private TargetSetter m_targetSetter = null;
    private Agent m_agent;
    private float m_speed = 10.0f;
    private string m_behaviourName = "";
    private UnityEvent m_behaviourEvent = null;
    private Animator m_animator;


    /// <summary>
    /// Sets the parameter for the behaviour. This specific overload of the function is used for static target
    /// </summary>
    /// <param name="destination"></param>
    public void SetParameters(Vector3 destination, float speed, string behaviourName = "", UnityEvent behaviourEvent = null)
    {
        m_destination = destination;
        m_isMovingTarget = false;
        m_behaviourName = behaviourName;
        m_speed = speed;
        m_behaviourEvent = behaviourEvent;
    }

    /// <summary>
    /// Sets the parameter for the behaviour. This specific overload of the function is used for moving target
    /// </summary>
    /// <param name="target"></param> The gameobject reference of the moving target
    public void SetParameters(GameObject target, float speed, string behaviourName = "", UnityEvent behaviourEvent = null)
    {
        m_target = target;
        m_isMovingTarget = true;
        m_behaviourName = behaviourName;
        m_speed = speed;
        m_behaviourEvent = behaviourEvent;
    }

    /// <summary>
    /// Sets the parameter for the behaviour. This specific overload of the function is used for wandering and patrolling
    /// </summary>
    /// <param name="target"></param> The gameobject reference of the moving target
    public void SetParameters(TargetSetter targetSetter, float speed, string behaviourName = "", UnityEvent behaviourEvent = null)
    {
        m_targetSetter = targetSetter;
        m_isMovingTarget = false;
        m_behaviourName = behaviourName;
        m_speed = speed;
        m_behaviourEvent = behaviourEvent;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="agent"></param>
    /// <returns></returns>
    public BehaviourResult UpdateBehaviour(Agent agent)
    {
        NavMeshAgent navMesh = agent.gameObject.GetComponent<NavMeshAgent>();
        if (!navMesh)
        {
            Debug.Log("Set Target Behaviour needs a nav mesh agent component");
            return BehaviourResult.ERROR;
        }

        // This is used for initialising target setter if needed
        if (!m_agent)
            m_agent = agent;

        if (!m_animator)
            m_animator = agent.animator;

        if (m_targetSetter != null)
        {
            m_destination = m_targetSetter.UpdateTarget(agent);
            if (m_targetSetter.FailureTest())
                return BehaviourResult.FAILURE;
            //Debug.Log("Destination is " + m_destination);
        }
        else if (m_isMovingTarget)
            m_destination = m_target.transform.position;

        navMesh.SetDestination(m_destination);
        navMesh.speed = m_speed;
        if (m_behaviourName == "Investigate" && agent.alerted && Vector3.SqrMagnitude(agent.transform.position - m_destination) <= 4f)
        {
            if (m_animator)
                m_animator.SetBool("Search", true);
        }

        if (m_behaviourEvent != null)
        {
            m_behaviourEvent.Invoke();
        }

        agent.prevBehaviour = m_behaviourName;
        return BehaviourResult.SUCCESS;
    }

    public void Enter()
    {
        if (m_targetSetter != null && m_agent != null)
            m_targetSetter.Initialise(m_agent);
    }

    public void Exit()
    {
        if (m_targetSetter != null)
            m_targetSetter.Reset();
    }
}
