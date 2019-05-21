using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// 
/// </summary>
public class Agent : MonoBehaviour {

    // List of all behaviours that the agent have
    protected List<IBehaviour> m_behaviours = new List<IBehaviour>();

    // Physics
    protected Vector3 m_force;
    public float maxSpeed = 10.0f;

    [HideInInspector] public Rigidbody body;
    [HideInInspector] public float internalDeltaTime = 0f;
    [HideInInspector] public bool triggered = false;
    [HideInInspector] public string triggerTag = "";
    [HideInInspector] public bool alerted = false;
    [HideInInspector] public NavMeshAgent navAgent;
    [HideInInspector] public string prevBehaviour;
    [HideInInspector] public Vector3 locationMemory = Vector3.zero;
    [HideInInspector] public bool memoryValid = false;
    
    // Animator
    public Animator animator;


    // Reference to specific instances in the scene
    protected Player player;
    protected EventManager eventManager;

    protected void UpdateBehaviours ()
    {
        // Resets all the forces on the agent
        m_force = Vector3.zero;

        // Updates all behaviours of the agent
        foreach (IBehaviour behaviour in m_behaviours)
            behaviour.UpdateBehaviour(this);
        
        // Add all the forces from the behaviours and add them into Unity's physics
        // This makes sure that it doesn't teleport
        if (body)
            body.AddForce(m_force);
    }

    // Add all the forces applied on the agent
    public void AddForce (Vector3 force)
    {
        m_force += force;
    }

}
