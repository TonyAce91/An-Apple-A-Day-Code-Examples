using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

// Code written by Antoine Kenneth Odi in 2018

public class ScarabAttack : IBehaviour {

    private Player m_player = null;
    private Scarab m_scarab = null;
    private float m_damage = 1f;
    private float m_timer = 5f;
    private float m_time = 0;
    private float m_rotationSpeed = 20f;
    private UnityEvent m_behaviourEvent = null;
    private Color temporaryColour = new Color (0, 1, 0, 1);

    public void SetParameters(Player player, float damage, float timer, float rotationSpeed = 20f, UnityEvent behaviourEvent = null)
    {
        m_player = player;
        m_damage = damage;
        m_rotationSpeed = rotationSpeed;
        m_behaviourEvent = behaviourEvent;
    }

    public BehaviourResult UpdateBehaviour(Agent agent)
    {
        if (!m_scarab)
            m_scarab = agent as Scarab;
        if (!m_player)
            return BehaviourResult.FAILURE;

        if (!m_scarab && (m_scarab = agent as Scarab) == null)
        {
            Debug.Log("Agent is not a scarab");
            return BehaviourResult.ERROR;
        }

        agent.navAgent.ResetPath();

        //agent.transform.LookAt(m_player.transform, agent.transform.up);
        Vector3 lookVector = m_player.transform.position - agent.transform.position;

        Quaternion lookAtTarget = Quaternion.LookRotation(lookVector, agent.transform.up);
        agent.transform.rotation = Quaternion.Slerp(agent.transform.rotation, lookAtTarget, agent.internalDeltaTime * m_rotationSpeed);

        lookVector.y = agent.transform.forward.y;

        float lookEvaluation = Vector3.Dot(agent.transform.forward, lookVector);

        if (m_scarab.attackTime <= 0 && !m_scarab.stopAttacking && lookEvaluation > 0.9f)
        {
            //////////////////////////////////////////////////////////////////////////////////////
            // This is used to set up the vignette whenever the player gets attacked

            // Checks relative location of scarab to where the player is
            Vector3 relativeLocation = m_player.transform.position - m_scarab.transform.position;

            // Checks relative position displacement to where the player is facing
            // and turn on the side accordingly. This part checks forward and backward
            float relativeForward = Vector3.Dot(m_player.transform.forward, relativeLocation);
            if (relativeForward > 0.25f)
                m_scarab.vignetteRef = m_player.bottomVignette;
            else if (relativeForward < -0.25f)
                m_scarab.vignetteRef = m_player.topVignette;

            // This part checks the left and right relative position
            float relativeRight = Vector3.Dot(m_player.transform.right, relativeLocation);
            if (relativeRight > 0.25f)
                m_scarab.vignetteRef = m_player.rightVignette;
            else if (relativeRight < -0.25f)
                m_scarab.vignetteRef = m_player.leftVignette;
            ///////////////////////////////////////////////////////////////////////////////////////


            m_scarab.animator.SetBool("Attack Player", true);

            //if (agent.prevBehaviour != "Attack")
            //{
            //    m_scarab.onAttack.Invoke();
            //    agent.prevBehaviour = "Attack";
            //}
            if (m_behaviourEvent != null)
                m_behaviourEvent.Invoke();
            m_time = m_timer;
        }
        else if (lookEvaluation < 0.9f)
            return BehaviourResult.SUCCESS;
        else
        {
            m_time -= agent.internalDeltaTime;
        }

        return BehaviourResult.SUCCESS;
    }
    public void Exit() {
        m_scarab.triggered = false;
        //if (m_player)
        //{
        //    m_player.topVignette.enabled = false;
        //    m_player.bottomVignette.enabled = false;
        //    m_player.leftVignette.enabled = false;
        //    m_player.rightVignette.enabled = false;
        //}
    }
}
