using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class is used to check if the target and agent are within the set range
/// 
/// Code written by Antoine Kenneth Odi in 2017.
/// </summary>
public class LineOfSight : Condition
{

    private GameObject m_target;
    private GameObject m_eyes;
    private float m_range = 0;
    private bool m_alertLeben = false;

    // Sets the target of the agent and the required range for the condition to be true
    public void SetParameters(GameObject target, float range, GameObject eyes)
    {
        m_target = target;
        m_range = range;
        m_eyes = eyes;
    }

    // Updates the behaviour and returns success if the agent and target is within the set range
    public override BehaviourResult UpdateBehaviour(Agent agent)
    {
        if (m_target == null)
            return BehaviourResult.ERROR;

        // Set up local variables needed for the raycast
        Vector3 direction =  m_target.transform.position - m_eyes.transform.position;
        RaycastHit hitInfo;

        //if (Vector3.Dot(direction, m_eyes.transform.forward) < 0.1f)
        //    return BehaviourResult.FAILURE;

        // Set a layer mask so that agent don't detect itself
        int layerMask = ~(1 << agent.gameObject.layer);

        // Raycast from the agent to the target and if the target is hit then it is in line of sight
        if (Physics.Raycast(m_eyes.transform.position, direction.normalized, out hitInfo, m_range, layerMask, QueryTriggerInteraction.Ignore))
        {
            if (Vector3.Dot(m_eyes.transform.forward, direction) > 0.9f && hitInfo.transform.gameObject == m_target)
            {
                Debug.DrawRay(m_eyes.transform.position, direction.normalized * m_range);
                //hitInfo.transform.gameObject.GetComponent<Renderer>().material.SetColor("_Color", Color.red);
                //if (hitInfo.collider.tag == "Player")
                //    Debug.Log("Player found");
                agent.locationMemory = m_target.transform.position;
                //Debug.Log("Player Location is suppose to be: " + agent.locationMemory);
                m_alertLeben = true;
                return BehaviourResult.SUCCESS;
            }

        }
        if (m_alertLeben)
        {
            m_alertLeben = false;
            agent.memoryValid = true;
        }
            
        return BehaviourResult.FAILURE;
    }
}
