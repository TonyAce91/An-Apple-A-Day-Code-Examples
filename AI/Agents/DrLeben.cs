using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using UnityEngine.UI;

/// <summary>
/// This is the main class used to control Dr Leben
/// This AI is using Unity's navmesh instead of steering forces
/// 
/// Code written by Antoine Kenneth Odi in 2018
/// </summary>


[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]
public class DrLeben : Agent {

    [HideInInspector] public Vector3 destination;
    [HideInInspector] public Vector3 investigatePosition;
    [SerializeField] private float m_animationSpeed = 12f;

    [Header("Door Drilling Settings")]
    [HideInInspector] public List<GameObject> panels = new List<GameObject>();
    [HideInInspector] public bool drillingStart = false;
    [HideInInspector] public bool securityDrilling = false;
    [HideInInspector] public GameObject closestPanelReference;
    [SerializeField] private float m_seekSpeed = 2f;

    [Header("Search Settings")]
    [SerializeField] private float m_searchRange = 10f;
    [SerializeField] private float m_searchSpeed = 7f;
    [SerializeField] private float m_rotationSpeed = 2f;
    
    [Header("Chase Settings")]
    [SerializeField] private float m_sightRange = 20f;
    [SerializeField] private GameObject m_eyeLocation;
    [SerializeField] private float m_chaseSpeed = 7f;

    [Header("Investigate Settings")]
    [SerializeField] private float m_investigateSpeed = 7f;
    private SetTargetBehaviour m_investigateArea = new SetTargetBehaviour();
    [SerializeField] private Vector3 m_lastPlayerLocation;

    [Header("Patrol Settings")]
    [SerializeField] private List<Transform> m_waypoints = new List<Transform>();
    [SerializeField] private float m_patrolSpeed = 7f;

    // Unity Events
    public UnityEvent onAttack;
    public UnityEvent onProximityDetection;
    public UnityEvent searching;
    public UnityEvent chasing;
    public UnityEvent patrolling;
    public UnityEvent onDoorDrilling;

    // Player Death
    [Header("Player Death Settings")]
    public UnityEvent onDeath;
    [SerializeField] private List<Image> uiImages = new List<Image>();
    private float vignetteAlpha = 0;
    private bool playerDrilled = false;


    // Use this for initialization
    void Start () {
        eventManager = FindObjectOfType<EventManager>();
        navAgent = GetComponent<NavMeshAgent>();
        player = FindObjectOfType<Player>();
        body = GetComponent<Rigidbody>();
        triggerTag = "Player";
        m_waypoints.TrimExcess();
        Debug.Log("Start target tag: " + triggerTag);

        animator = GetComponent<Animator>();

        // Detects if eye location is set
        if (!m_eyeLocation)
            Debug.Log("Eye Location is not set");

        // Detects if waypoints are set
        if (m_waypoints.Count < 1)
            Debug.Log("Waypoints are not set");

        //----------------------------------------------------
        // The Attack Sequence

        // Set triggers to turn off
        List<string> offTriggers = new List<string>();
        offTriggers.Add("Search");
        offTriggers.Add("Chase");

        // Set up the attack behaviour
        PlayAnimation attackAnimation = new PlayAnimation();
        attackAnimation.SetParameters(animator, "DeathSequence", offTriggers);

        // Set up condition for the attack sequence
        Triggered attackCondition = new Triggered();

        // Set up attack sequence
        Sequence attackSequence = new Sequence();
        attackSequence.addBehaviour(attackCondition);
        attackSequence.addBehaviour(attackAnimation);

        // ---------------------------------------------------
        // The Drill Sequence

        // Set up the drill condition
        DrillBehaviour drillBehaviour = new DrillBehaviour();
        drillBehaviour.SetParameters(false);

        // Set up drill animation
        PlayAnimation drillAnimation = new PlayAnimation();
        drillAnimation.SetParameters(animator, "DrillSequence", null, onDoorDrilling);

        // Set up drill sequence
        Sequence drillSequence = new Sequence();
        drillSequence.addBehaviour(drillBehaviour);
        //drillSequence.addBehaviour(seekPanel);
        drillSequence.addBehaviour(drillAnimation);

        // ---------------------------------------------------
        // The Rage Sequence

        // Set up the rage condition
        DrillBehaviour rageBehaviour = new DrillBehaviour();
        rageBehaviour.SetParameters(true);

        // Set up rage animation
        PlayAnimation rageAnimation = new PlayAnimation();
        rageAnimation.SetParameters(animator, "RageSequence");

        // Set up rage sequence
        Sequence rageSequence = new Sequence();
        rageSequence.addBehaviour(rageBehaviour);
        rageSequence.addBehaviour(rageAnimation);

        //----------------------------------------------------
        // The Proximity Sequence

        // Set up within range condition for search sequence
        WithinRange withinSearch = new WithinRange();
        withinSearch.SetParameters(player.gameObject, m_searchRange);

        // Set up line of sight condition for search sequence
        LineOfSight inSight = new LineOfSight();
        inSight.SetParameters(player.gameObject, m_sightRange, m_eyeLocation);

        // Set up the close chase behaviour
        SetTargetBehaviour closeChase = new SetTargetBehaviour();
        closeChase.SetParameters(player.gameObject, m_searchSpeed, "CloseChase", onProximityDetection);

        // Set up arm extended animation
        PlayAnimation armExtended = new PlayAnimation();
        armExtended.SetParameters(animator, "ArmExtended");

        // Set up chase sequence
        Sequence closeChaseSequence = new Sequence();
        closeChaseSequence.addBehaviour(withinSearch);
        closeChaseSequence.addBehaviour(inSight);
        closeChaseSequence.addBehaviour(closeChase);
        closeChaseSequence.addBehaviour(armExtended);

        //----------------------------------------------------
        // The Search Sequence

        // Turn towards the player
        FaceTarget facePlayer = new FaceTarget();
        facePlayer.SetParameters(player.gameObject, m_rotationSpeed);

        // Set up arm extended animation
        PlayAnimation searchAnimation = new PlayAnimation();
        searchAnimation.SetParameters(animator, "Search", null, searching);

        // Set up search sequence
        Sequence searchSequence = new Sequence();
        searchSequence.addBehaviour(withinSearch);
        searchSequence.addBehaviour(facePlayer);
        searchSequence.addBehaviour(searchAnimation);

        //----------------------------------------------------
        // The Chase Sequence

        // Set up the chase behaviour
        SetTargetBehaviour chasePlayer = new SetTargetBehaviour();
        chasePlayer.SetParameters(player.gameObject, m_chaseSpeed, "Chase", chasing);

        // Set up within range condition for chase sequence
        WithinRange withinChase = new WithinRange();
        withinChase.SetParameters(player.gameObject, m_sightRange);

        // Set up chase sequence
        Sequence chaseSequence = new Sequence();
        chaseSequence.addBehaviour(withinChase);
        chaseSequence.addBehaviour(inSight);
        chaseSequence.addBehaviour(chasePlayer);

        //----------------------------------------------------
        // The Investigate Sequence

        // Set up alert condition
        AlertCondition alertCondition = new AlertCondition();

        // Set up investigate sequence
        Sequence investigateSequence = new Sequence();
        investigateSequence.addBehaviour(alertCondition);
        investigateSequence.addBehaviour(m_investigateArea);


        //----------------------------------------------------
        // The Patrol Sequence
        // Set up the patrol targets
        Patrol patrolDestination = new Patrol();
        patrolDestination.SetParameters(m_waypoints);

        // Set up the patrol behaviour
        SetTargetBehaviour patrolBehaviour = new SetTargetBehaviour();
        patrolBehaviour.SetParameters(patrolDestination, m_patrolSpeed,"Patrol", patrolling);

        // Look for nearest waypoint then continue with the patrol from there

        //----------------------------------------------------
        // The Main Selector

        // Set up main selector
        Selector mainSelector = new Selector();
        mainSelector.addBehaviour(attackSequence);
        mainSelector.addBehaviour(drillSequence);
        mainSelector.addBehaviour(rageSequence);
        mainSelector.addBehaviour(closeChaseSequence);
        mainSelector.addBehaviour(searchSequence);
        mainSelector.addBehaviour(chaseSequence);
        mainSelector.addBehaviour(investigateSequence);
        mainSelector.addBehaviour(patrolBehaviour);

        // Add all sequences to the behaviour list
        m_behaviours.Add(mainSelector);
    }

    private void FixedUpdate()
    {
        UpdateBehaviours();
        internalDeltaTime = Time.fixedDeltaTime;
        NavAnimCoupling();
    }

    // Update is called once per frame
    void Update () {
        if (memoryValid)
        {
            Alerted(locationMemory);
            Debug.Log("location memory: " + locationMemory);
            memoryValid = false;
        }

        if (playerDrilled && vignetteAlpha < 1 && uiImages.Count > 0)
        {
            // Only solution I could think of for death vignette
            foreach (Image uiImage in uiImages)
            {
                uiImage.color = new Color(80f / 255f, 0, 0, vignetteAlpha);
            }
            vignetteAlpha += 0.05f;
        }

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == triggerTag)
        {
            triggered = true;
            navAgent.ResetPath();
        }

        // Initiate drilling door
        if (other.tag == "DoorObject")
        {
            Door doorInstance = null;
            DoorSystem system = null;
            if ((doorInstance = other.gameObject.GetComponent<Door>()) != null)
            {
                system = doorInstance.SystemInstance;
            }
            if (system != null && system.LockedDoors && !system.BrokenDoors)
            {
                if (!system.RequireCore)
                    drillingStart = true;
                else
                    securityDrilling = true;
            }
        }
    }

    // Once the animation drilling finishes load death scene
    public void LoseState()
    {
        SceneManager.LoadScene("DeathScene");

    }

    // This is called by unity events when alert events happen such as opening door
    public void Alerted(Vector3 alarmPosition)
    {
        alerted = true;
        m_investigateArea.SetParameters(alarmPosition, m_investigateSpeed, "Investigate");
    }

    Vector2 smoothDeltaPosition = Vector2.zero;
    Vector2 velocity = Vector2.zero;

    private void NavAnimCoupling()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("DoorDrillAnimation"))
            navAgent.updatePosition = false;
        else
            navAgent.updatePosition = true;

        float navSpeed = navAgent.velocity.magnitude;
        Vector3 deltaPos = navAgent.nextPosition - transform.position;
        float dy = Vector3.Dot(transform.right, deltaPos.normalized);

        animator.SetFloat("Turn", -dy);


        animator.SetFloat("Speed", (navSpeed) * m_animationSpeed);
        float animSpeed = (navSpeed) * m_animationSpeed;
        animSpeed = Mathf.Max(animSpeed, 1.0f);
        animator.SetFloat("AnimationSpeed", animSpeed);

    }

    // Use this for initialization
    void PlayerCaught()
    {
        onDeath.Invoke();
        uiImages.TrimExcess();
        playerDrilled = true;
    }

    public void FinishInvestigation()
    {
        Debug.Log("Suppose to turn off alert mode");
        alerted = false;
        animator.SetBool("Search", false);
    }
    public void FinishDrilling()
    {
        drillingStart = false;
        securityDrilling = false;
        animator.SetBool("DrillSequence", false);
        animator.SetBool("RageSequence", false);
        navAgent.updatePosition = true;
    }

    public void StopRaging()
    {
        securityDrilling = false;
        animator.SetBool("RageSequence", false);
        navAgent.updatePosition = true;
    }

    //public void StartDrilling()
    //{
    //    drillingStart = true;
    //    navAgent.updatePosition = false;
    //}


    // Error messages to notify developer about missing variables needed by script
    private void VariableCheck()
    {
#if UNITY_EDITOR
        if (UnityEditor.EditorApplication.isPlaying)
        {

        }

#endif
    }

    //private void OnAnimatorMove()
    //{
    //    transform.position = navAgent.nextPosition;
    //    //Vector3 position = animator.rootPosition;
    //    //position.y = navAgent.nextPosition.y;
    //    //transform.position = position;
    //}
}