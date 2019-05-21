using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;


public enum DamageType
{
    STUN,
    KILL,
    ERROR
}

[RequireComponent(typeof(SphereCollider))]
public class Scarab : Agent, IDetectable
{

    [HideInInspector] public Vector3 destination;

    // Death sequence variables
    [Header("Damage Settings")]
    [SerializeField] private float stunTimer = 5f;
    private float stunTime = 0;
    [SerializeField] private float m_angularDrag = 0.5f;
    [SerializeField] private float m_drag = 1f;
    [SerializeField] private float m_forceModifier = 100f;
    [SerializeField] private float m_colliderRadius = 1.3f;
    [SerializeField] private Vector3 m_colliderCentre = new Vector3(0, -0.3f, 0);
    private SphereCollider m_sphereCollider = null;


    // Attack sequence variables
    [Header("Attack Settings")]
    [SerializeField] private float m_attackRange = 10f;
    [SerializeField] private float m_damage = 10f;
    [SerializeField] private float m_sightRange = 12f;
    [SerializeField] private GameObject m_eyeLocation;
    [SerializeField] private float m_attackTimer = 10f;
    [SerializeField] private float m_rotationSpeed = 15f;

    [HideInInspector] public bool stopAttacking = false;
    [HideInInspector] public float attackTime = 0;
    [HideInInspector] public VignetteScript vignetteRef = null;

    // Chase sequence variables
    [Header("Chase Settings")]
    [SerializeField] private float m_chaseRange = 20f;
    [SerializeField] private float m_chaseSpeed = 7f;

    // Investigate sequence variables
    //[Header("Door Drilling Settings")]

    // Investigate sequence variables
    [Header("Wander Settings")]
    [SerializeField] private float m_wanderSpeed = 2f;

    //[HideInInspector] public NavMeshAgent navAgent;
    //private GameObject player;

	AudioSource scarabAudio;
	//public AudioClip scarabAttackSound;

    [HideInInspector] public bool attacked = false;

    [SerializeField] private float m_animationSpeed = 20f;

    public UnityEvent onDeath;
    public UnityEvent onStun;
    public UnityEvent onAttack;
    public UnityEvent chasing;
    public UnityEvent wandering;

    private EventManager m_eventManager;
    private bool m_disabled = false;

    // Use this for initialization
    void Start()
    {
        gameObject.tag = "Scarab";
		scarabAudio = GetComponent <AudioSource> ();
        eventManager = FindObjectOfType<EventManager>();
        navAgent = GetComponent<NavMeshAgent>();
        player = FindObjectOfType<Player>();
        body = GetComponent<Rigidbody>();
        triggerTag = "Player";

        m_sphereCollider = GetComponent<SphereCollider>();

        if (!animator)
        {
            animator = GetComponentInChildren<Animator>();
        }
        //m_animator = GetComponent<Animator>();

        //// Setting layers for Dr Leben's parts
        //foreach (Transform child in transform)
        //    child.gameObject.layer = gameObject.layer;

        // ---------------------------------------------------
        // The Death Sequence

        // Set up the 
        IsScarabAttacked deathCheck = new IsScarabAttacked();

        Sequence deathSequence = new Sequence();
        deathSequence.addBehaviour(deathCheck);

        //----------------------------------------------------
        // The Attack Sequence

        //// Set up condition for the attack sequence
        //Triggered attackCondition = new Triggered();

        // Set up within range condition for chase sequence
        WithinRange withinAttack = new WithinRange();
        withinAttack.SetParameters(player.gameObject, m_attackRange);

        // Set Up Continue attack condition
        ContinueAttack continueAttackCondition = new ContinueAttack();

        // Set up the attack behaviour
        ScarabAttack attackBehaviour = new ScarabAttack();
        attackBehaviour.SetParameters(player, m_damage, m_attackTimer, m_rotationSpeed, onAttack);

        // Set up line of sight condition for chase sequence
        LineOfSight inSight = new LineOfSight();
        inSight.SetParameters(player.gameObject, m_sightRange, m_eyeLocation);

        //// Set up component check enabled
        //ComponentEnabled navAgentCheck = new ComponentEnabled();
        //navAgentCheck.SetParameters(navMesh);

        // Set up attack sequence
        Sequence attackSequence = new Sequence();
        attackSequence.addBehaviour(continueAttackCondition);
        //attackSequence.addBehaviour(navAgentCheck);
        //attackSequence.addBehaviour(attackCondition);
        attackSequence.addBehaviour(withinAttack);
        attackSequence.addBehaviour(inSight);
        //attackSequence.addBehaviour(facePlayer);
        attackSequence.addBehaviour(attackBehaviour);

        //----------------------------------------------------
        // The Face Target Sequence

        // Set up face target behaviour
        FaceTarget facePlayer = new FaceTarget();
        facePlayer.SetParameters(player.gameObject, m_rotationSpeed);

        // Set up attack sequence
        Sequence faceTargetSequence = new Sequence();
        faceTargetSequence.addBehaviour(continueAttackCondition);
        faceTargetSequence.addBehaviour(withinAttack);
        faceTargetSequence.addBehaviour(facePlayer);

        //----------------------------------------------------
        // The Chase Sequence

        // Set up the chase behaviour
        SetTargetBehaviour chasePlayer = new SetTargetBehaviour();
        chasePlayer.SetParameters(player.gameObject, m_chaseSpeed, "", chasing);

        // Set up within range condition for chase sequence
        WithinRange withinChase = new WithinRange();
        withinChase.SetParameters(player.gameObject, m_chaseRange);

        // Set up chase sequence
        Sequence chaseSequence = new Sequence();
        chaseSequence.addBehaviour(continueAttackCondition);
        //chaseSequence.addBehaviour(navAgentCheck);
        chaseSequence.addBehaviour(withinChase);
        chaseSequence.addBehaviour(chasePlayer);

        //----------------------------------------------------
        // The Wander Sequence
        WanderTarget wanderSetter = new WanderTarget();

        SetTargetBehaviour wanderBehaviour = new SetTargetBehaviour();
        wanderBehaviour.SetParameters(wanderSetter, m_wanderSpeed, "", wandering);

        // Set up wander sequence
        Sequence wanderSequence = new Sequence();
        //wanderSequence.addBehaviour(navAgentCheck);
        wanderSequence.addBehaviour(wanderBehaviour);

        ////----------------------------------------------------
        //// The Attack Selector

        //// Set up main selector
        //Selector attackSelector = new Selector();
        //attackSelector.addBehaviour(attackSequence);
        //attackSelector.addBehaviour(chaseSequence);

        //----------------------------------------------------
        // The Sequence Selector


        // Set up main sequence
        Selector mainSelector = new Selector();
        mainSelector.addBehaviour(deathSequence);
        mainSelector.addBehaviour(attackSequence);
        mainSelector.addBehaviour(chaseSequence);
        mainSelector.addBehaviour(wanderSequence);

        // Add all sequences to the behaviour list
        m_behaviours.Add(mainSelector);

        // correct position so it always sits on navmesh
        NavMeshHit closestHit;

        if (NavMesh.SamplePosition(gameObject.transform.position, out closestHit, 500f, NavMesh.AllAreas))
            gameObject.transform.position = closestHit.position;
        else
            Debug.LogError("Could not find position on NavMesh!");
    }

    private void FixedUpdate()
    {
        UpdateBehaviours();
        internalDeltaTime = Time.fixedDeltaTime;
    }

    // Update is called once per frame
    void Update()
    {
        Timers();
        Animation();

    }

    void OnTriggerEnter(Collider other)
    {

		if (other.tag == triggerTag) {
			triggered = true;
		}
    }

    private void Timers()
    {
        if (stopAttacking)
            attackTime += Time.fixedDeltaTime;

        // Resets time
        if (attackTime >= m_attackTimer)
        {
            stopAttacking = false;
            attackTime = 0;
        }

        if (stunTime > 0)
            stunTime -= Time.fixedDeltaTime;
        else if (!m_disabled)
        {
            stunTime = 0;
            navAgent.enabled = true;
            attacked = false;
            animator.SetBool("Stunned", false);
        }
        
    }

    public void TakeDamage(DamageType damage)
    {
        // This is to double check if nav mesh agent is set on the scarab and then disable it
        if (navAgent)
            navAgent.enabled = false;

        // Checks what type of damage
        if (damage == DamageType.KILL && !m_disabled)
        {
            onDeath.Invoke();
            animator.SetBool("Dead", true);
            m_disabled = true;
            attacked = true;
            navAgent.enabled = false;

            // Adding and setting rigidbody for when scarab dies
            Rigidbody rb = gameObject.AddComponent<Rigidbody>();
            rb.angularDrag = m_angularDrag;
            rb.drag = m_drag;
            rb.AddForce(player.transform.forward * m_forceModifier);
            if (m_sphereCollider)
                m_sphereCollider.radius = m_colliderRadius;

            // Calls alert event
            AlertEvent();
        }
        else if (damage == DamageType.STUN && !m_disabled)
        {
            onStun.Invoke();
            //m_disabled = true;
            attacked = true;
            stunTime = stunTimer;
            animator.SetBool("Stunned", true);
            animator.SetBool("Attack Player", false);
        }
    }

    private void Animation()
    {
        if (!animator)
            return;

        if (navAgent)
            animator.SetFloat("Speed", (navAgent.velocity.magnitude) * m_animationSpeed * Time.deltaTime);
    }

    public void AlertEvent()
    {
        eventManager.AlarmEvent(transform.position);
    }

    public void ApplyDamage()
    {
        player.TakeDamage(m_damage);
        m_wanderSpeed = 5f;
        if (navAgent)
            navAgent.ResetPath();
        if (vignetteRef != null)
        {
            vignetteRef.Flash();
            vignetteRef = null;
        }
    }

    public void StopAttacking()
    {
        stopAttacking = true;
        gameObject.layer = 16;
        animator.SetBool("Attack Player", false);
    }


}
