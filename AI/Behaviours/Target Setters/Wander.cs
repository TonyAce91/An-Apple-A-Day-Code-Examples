using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WanderTarget : TargetSetter {

    private float m_radius = 10;
    private float m_jitter = 0.1f;
    private float m_distance = 1f;
    private float m_sampleDistance = 1;
    private float m_timer = 2f;
    private float m_time = 0;
    private Vector3 m_currentTarget;

    // Set parameters for wander state
    public void SetParamaters(float radius = 10.0f, float jitter = 0.1f, float distance = 1.0f, float sampleDistance = 10.0f)
    {
        m_radius = radius;
        m_jitter = jitter;
        m_distance = distance;
        m_sampleDistance = sampleDistance;
    }

    public override Vector3 UpdateTarget(Agent agent)
    {
        m_time -= agent.internalDeltaTime;
        if (!((agent.transform.position - m_currentTarget).sqrMagnitude < 10f || m_time <= 0))
        {
            return m_currentTarget;
        }

        Vector2 randomTarget = Random.insideUnitCircle * m_radius;

        // Random jitter
        Vector2 randomJitter = Random.insideUnitCircle;
        randomJitter = randomJitter.normalized * m_jitter;

        randomTarget += randomJitter;
        randomTarget = randomTarget.normalized * m_radius;

        // Calculates a new wander target destination
        Vector3 target = (agent.transform.forward * m_distance + new Vector3(randomTarget.x, agent.transform.position.y, randomTarget.y)) + agent.transform.position;

        m_time = m_timer;
        m_currentTarget = target;
        return m_currentTarget;
    }

    public override bool FailureTest()
    {
        return false;
    }
}
