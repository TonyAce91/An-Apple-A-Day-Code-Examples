using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//////////////////////////////////////////////////////////
// Leben's AI during the tutorial scripted event
//
//
// Code written by Antoine Kenneth Odi in 2018
//////////////////////////////////////////////////////////
public class InitialLeben : Agent {

    [SerializeField] private Transform initialWaypoint;

    [SerializeField] private float speedRotation = 1.0f;

    [HideInInspector] public bool searchPlayer = false;
	[HideInInspector] public bool playerLeaving = false;

    [HideInInspector] public Vector3 destination;
    [HideInInspector] public Vector3 investigatePosition;
    [SerializeField] private float m_animationSpeed = 12f;

    [Header("Door Drilling Settings")]
    [HideInInspector] public List<GameObject> panels = new List<GameObject>();
    [HideInInspector] public bool drillingStart = false;
    [HideInInspector] public bool securityDrilling = false;
    [HideInInspector] public GameObject closestPanelReference;
    [SerializeField] private float m_seekSpeed = 2f;

    [Header("Investigate Settings")]
    [SerializeField] private float m_investigateSpeed = 7f;
    private SetTargetBehaviour m_investigateArea = new SetTargetBehaviour();
    [SerializeField] private Vector3 m_lastPlayerLocation;

    //[HideInInspector] public bool playerEntering = false;
    private DrLeben drScript;

	private bool soundPlayed = false;

    private Vector3 previousRotation;
    private Vector3 nextRotation;


    // Use this for initialization
    void Start () {
        drScript = GetComponent<DrLeben>();
        if (drScript)
            drScript.enabled = false;
		//lebenAudio = GetComponent<AudioSource> ();

        eventManager = FindObjectOfType<EventManager>();
        navAgent = GetComponent<NavMeshAgent>();
        player = FindObjectOfType<Player>();
        navAgent.SetDestination(transform.position);
        animator = GetComponent<Animator>();

        // ---------------------------------------------------
        // The Drill Sequence

        // Set up the drill condition
        InitialDrill drillBehaviour = new InitialDrill();
        drillBehaviour.SetParameters(false);

        // Set up drill animation
        PlayAnimation drillAnimation = new PlayAnimation();
        drillAnimation.SetParameters(animator, "DrillSequence");

        // Set up drill sequence
        Sequence drillSequence = new Sequence();
        drillSequence.addBehaviour(drillBehaviour);
        //drillSequence.addBehaviour(seekPanel);
        drillSequence.addBehaviour(drillAnimation);

        // ---------------------------------------------------
        // The Rage Sequence

        // Set up the rage condition
        InitialDrill rageBehaviour = new InitialDrill();
        rageBehaviour.SetParameters(true);

        // Set up rage animation
        PlayAnimation rageAnimation = new PlayAnimation();
        rageAnimation.SetParameters(animator, "RageSequence");

        // Set up rage sequence
        Sequence rageSequence = new Sequence();
        rageSequence.addBehaviour(rageBehaviour);
        rageSequence.addBehaviour(rageAnimation);

        //----------------------------------------------------
        // The Investigate Sequence

        // Set up alert condition
        AlertCondition alertCondition = new AlertCondition();

        // Set up investigate sequence
        Sequence investigateSequence = new Sequence();
        investigateSequence.addBehaviour(alertCondition);
        investigateSequence.addBehaviour(m_investigateArea);



        //----------------------------------------------------
        // The Main Selector

        // Set up main selector
        Selector mainSelector = new Selector();
        mainSelector.addBehaviour(drillSequence);
        mainSelector.addBehaviour(rageSequence);
        mainSelector.addBehaviour(investigateSequence);

        // Add all sequences to the behaviour list
        m_behaviours.Add(mainSelector);

        //if (!batonReference)
        //    Debug.Log("Baton Reference not set on Dr. Leben");
    }

    // Update is called once per frame
    void FixedUpdate () {
        UpdateBehaviours();
        internalDeltaTime = Time.fixedDeltaTime;
        NavAnimCoupling();
        if (playerLeaving)
        {
            //PlaySound(comeBackHere);
            drScript.enabled = true;
            enabled = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            drScript.enabled = true;
            enabled = false;
        }

        if (other.tag == "DoorObject")
        {
            Debug.Log("Trigger detected");
            Door doorInstance = null;
            DoorSystem system = null;
            transform.LookAt(other.transform);
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

    private void NavAnimCoupling()
    {
        float navSpeed = navAgent.velocity.magnitude;

        // How far his next position is, the faster the animation to compensate for this distance
        animator.SetFloat("Speed", (navSpeed) * m_animationSpeed);
        float animSpeed = (navSpeed) * m_animationSpeed;
        animSpeed = Mathf.Max(animSpeed, 1.0f);
        animator.SetFloat("AnimationSpeed", animSpeed);
    }

    // Function for event calls which alerts Dr Leben to the alert position
    public void Alerted(Vector3 alarmPosition)
    {
        alerted = true;
        m_investigateArea.SetParameters(alarmPosition, m_investigateSpeed, "Investigate");
    }

    public void SearchPlayer()
    {
        Alerted(initialWaypoint.position);
    }

    public void FinishInvestigation()
    {
        Debug.Log("Suppose to turn off alert mode");
        alerted = false;
        animator.SetBool("Search", false);
    }

    // Function called after drilling animation finishes
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
}
