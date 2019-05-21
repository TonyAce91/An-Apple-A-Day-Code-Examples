using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

// Code made by Antoine Kenneth Odi in 2018
// Some code addition by Georgia Bryant in 2018

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(AudioSource))]
public class Player : MonoBehaviour
{

    private DrLeben m_lebenScript;

    // Walk action variables
    [Header("Walking Settings")]
    [SerializeField] private bool m_walking = true;
    [SerializeField] private float m_walkSpeed = 5f;

    // Variables for footstep ties in with audio
    [SerializeField] private float m_stepInterval = 1.0f;
    private float m_stepCycle;
    private float m_nextStep;

    // Run action variables
    [Header("Running Settings")]
    [SerializeField] private float m_runSpeed = 10f;
    [SerializeField] [Range(0f, 1f)] private float m_runStepLength;

    // Jump action variables
    [Header("Jumping Settings")]
    [SerializeField] private float m_jumpSpeed = 7f;
    [SerializeField] private float m_gravityMultiplier = 2f;
    private bool m_jumpStarted;
    private bool m_airborne;
    private bool m_previouslyGrounded;

    // Player Stats
    [Header("Player Stats Settings")]
    [SerializeField] private float m_maxToxicity = 100.0f;
    [SerializeField] private Slider toxicitySlider;
    [SerializeField] private float m_toxicityReductionSpeed = 10f;
    [SerializeField] private float m_currentToxicity = 0;
    public float toxicityRatio;
    private float m_toxicityToRunRatio;

    // Pick up variables
    [Header("Pick Up Settings")]
    [SerializeField] private float m_pickUpDistance = 1000.0f;
    public bool holdingObject = false;

    // Player view
    [Header("Camera Settings")]
    [SerializeField] private Camera m_camera;
    [SerializeField] private GameObject m_deathCameraObject;
    [SerializeField] private float m_angleLimit = 85f;
    private Vector3 m_cameraStandPosition;
    [HideInInspector] public bool fpsMode = true;

    // Mouse Look Variables
    private Quaternion m_charRot;
    private Quaternion m_cameraRot;
    private float XSensitivity = 2f;
    private float YSensitivity = 2f;


    [Header("Interact Settings")]
    [SerializeField] private float m_interactRange = 20f;

    [Header("Crouch Settings")]
    [SerializeField] private float m_crouchHeight = 0.75f / 2f;
    private bool m_crouching = false;
    private CapsuleCollider m_playerCollider = null;
    private float m_originalHeight;
    private Vector3 m_originalCentre;

    [Header("Attack Settings")]
    [SerializeField] private float m_batonRange = 2f;
    [SerializeField] private int playerLayer = 10;
    [SerializeField] private float m_stunTimer = 10f;
    [SerializeField] private Material m_stunTimerMat;
    [SerializeField] private Material m_batonMat;
    private float m_stunTime;

    [Header("Vignette Settings")]
    public VignetteScript topVignette;
    public VignetteScript bottomVignette;
    public VignetteScript leftVignette;
    public VignetteScript rightVignette;
    [SerializeField] private Image boxVignette;
    [SerializeField] private Image splatImage;
    [SerializeField] private Color currentColour;

    [Header("Tutorial Settings")]
    [SerializeField] private GameObject m_warning;
    [SerializeField] private Text m_tutorialText;
    [SerializeField] private float m_tutorialTimer = 5f;
    [HideInInspector] public bool firstCore = false;
    private bool firstScarab = false;
    private bool m_warningOn = false;
    private float m_tutorialTime;

    [Header("Temporary Variable settings")]
    // Player Controller
    private CharacterController m_controller;
    private CollisionFlags m_collisionFlags;

    // Input and physics variables
    private Vector3 m_moveDir;
    private float m_stickToGroundForce = 10.0f;
    public bool m_restrictMovement = true;

    // Start variables


    private GameFader gameFader;
    private CutsceneCamera cutscene;

    private Vector2 m_input;

    public int totalCores = 0;
    public bool faceLeben = false;

    // Animator
    [SerializeField] private GameObject m_hand;
    [SerializeField] private Animator m_handAnimator;

    // Hand Location
    [SerializeField] private Vector3 m_handWalkPosition = Vector3.zero;
    [SerializeField] private Vector3 m_handRunPosition = Vector3.zero;

    public Text playerInstruction;

    //////////////////////////////////////////////////////////////////////
    //// Temporary variables
    //public Text press_E_Text;
    //public Text requires_core_Text;
    public Text coreNumber_Text;
    //public Text activate_Airlock_Text;
    /////////////////////////////////////////////////////////////////////
    public int coreCount;

    public GameObject playerBaton;
    [HideInInspector] public bool haveBaton = false;

    private EventManager m_eventManager;

    private bool m_playerCaught = false;

    [SerializeField] private float m_timer = 10f;

    ///////////////////////////////////////////////////////////////////

    ///////////////////////////////////////////////////////////////////
    // Temporary variables
    //Setting up skull icons
    public Image skullIcon_01;
    public Image skullIcon_02;
    public Image skullIcon_03;

    [Header("Baton Effects")]
    public ParticleSystem batonParticles;
    public AudioSource batonSource;
    AudioClip batonchargeClip;
    ///////////////////////////////////////////////////////////////////


    // Events
    public UnityEvent onBatonPickup;
    public UnityEvent onBatonHit;
    public UnityEvent onBatonStun;
    public UnityEvent onDeath;
    public UnityEvent collectionComplete;
    public UnityEvent attacked;
    public UnityEvent finishBatonFade;

    #region Properties
    public float CurrentToxicity
    {
        get { return m_currentToxicity; }
    }
    #endregion

    // Use this for initialization
    void Start()
    {
        skullIcon_01.enabled = true;
        skullIcon_02.enabled = false;
        skullIcon_03.enabled = false;

        // This uses character controller since you don't want physics to affect the character with its camera
        // which can cause nausea. From experience, rigidbody remembers the forces applied to it even when kinematic
        // therefore when turned off, it gets applied to the system.
        m_controller = GetComponent<CharacterController>();
        m_camera = GetComponentInChildren<Camera>();
        m_eventManager = FindObjectOfType<EventManager>();
        m_playerCollider = GetComponent<CapsuleCollider>();
        m_originalHeight = m_controller.height;
        m_originalCentre = m_controller.center;
        m_cameraStandPosition = m_camera.transform.localPosition;
        m_lebenScript = FindObjectOfType<DrLeben>();
        if (!m_hand)
            if (GetComponentInChildren<HandAnimations>())
                m_hand = GetComponentInChildren<HandAnimations>().gameObject;

        // Set the colour of the vignette to be green for scarab attacks
        currentColour = new Color(0, 120f/ 255f, 0, 0);
        //bottomVignette.color = currentColour;
        //topVignette.color = currentColour;
        //leftVignette.color = currentColour;
        //rightVignette.color = currentColour;
        boxVignette.color = currentColour;
        splatImage.color = currentColour;

        gameFader = FindObjectOfType<GameFader>();
        cutscene = FindObjectOfType<CutsceneCamera>();

        // mouse look initialisation
        m_charRot = transform.localRotation;
        m_cameraRot = m_camera.transform.localRotation;

        // Run speed ratio depending on toxicity
        m_toxicityToRunRatio = (m_runSpeed - m_walkSpeed) / m_maxToxicity;

        //press_E_Text.enabled = false;
        //playerInstruction.enabled = false;
        playerInstruction.text = "";
		//requires_core_Text.enabled = false;
        coreCount = 0;
        SetCoreText();
        //activate_Airlock_Text.enabled = false;

        if (m_handWalkPosition == Vector3.zero)
        m_handWalkPosition = m_hand.transform.localPosition;
        if (m_handRunPosition == Vector3.zero)
            m_handRunPosition = m_handWalkPosition - new Vector3(0, 0.11f, 0);

        //playerBaton.SetActive(false);

        //Set Slider starting toxicity to zero
        //toxicitySlider.value = m_currentToxicity;
    }

    private void FixedUpdate()
    {
        if (m_playerCaught)
            return;

        float speed;

        // Reads user input which determines the direction of motion
        GetInput(out speed);

        // Set character movement in reference to the player's view direction
        Vector3 desiredMove = transform.forward * m_input.y + transform.right * m_input.x;

        // Checks for the surface the character is standing on
        RaycastHit hitInfo;
        Physics.SphereCast(transform.position, m_controller.radius, Vector3.down, out hitInfo, m_controller.height / 2f);

        // Projects the movement vector on the plane where the character is standing
        desiredMove = Vector3.ProjectOnPlane(desiredMove, hitInfo.normal);
        if (desiredMove.sqrMagnitude > 0.01f)
            desiredMove.Normalize();

        // move the character in the desired direction according to specified speed
        m_moveDir.x = desiredMove.x * speed;
        m_moveDir.z = desiredMove.z * speed;

        if (m_controller.isGrounded)
        {
            // Keeps character grounded
            m_moveDir.y = -m_stickToGroundForce;

            // Start of jump, move character accordingly
            if (m_jumpStarted)
            {
                m_moveDir.y = m_jumpSpeed;
                // Jump started variable is only used for the instant input from player
                // Turns it off the moment the character leaves the ground
                m_jumpStarted = false;
                m_airborne = true;
            }
        }
        else
        {
            // Applies gravity when not grounded
            m_moveDir += Physics.gravity * m_gravityMultiplier * Time.fixedDeltaTime;
        }
        //if (Input.GetKey(KeyCode.W))
        //    m_controller.Move(transform.forward * speed * Time.fixedDeltaTime);

        if (m_restrictMovement && faceLeben && m_lebenScript != null)
        {
            gameObject.transform.LookAt(m_lebenScript.transform);
        }


        if (!m_restrictMovement)
        {
            // Move the character controller and return collision flag
            m_collisionFlags = m_controller.Move(m_moveDir * Time.fixedDeltaTime);
        }
    }
    float fadeTimer = 1;
    bool fadeStart = false;

    // Update is called once per frame
    void Update()
    {
        //MikeyTripping();
        if (m_playerCaught)
            return;
        RotateView();
        UserInteract();
        HandAnimation();
        Crouch();

        if (fadeStart && fadeTimer > 0)
            fadeTimer -= Time.deltaTime;
        else if (fadeStart)
        {
            finishBatonFade.Invoke();
            fadeStart = false;
        }


        // Checks for jump input from user if not already airborne
        if (!m_jumpStarted && !m_airborne)
            m_jumpStarted = Input.GetButtonDown("Jump");

        // Stop moving character in the y direction when player lands
        if (!m_previouslyGrounded && m_controller.isGrounded)
        {
            m_moveDir.y = 0f;
            m_airborne = false;
        }

        // Checks if the controller is not grounded and the player supposed to be
        if (!m_controller.isGrounded && !m_airborne && m_previouslyGrounded)
            m_moveDir.y = 0f;

        // Reduce toxicity over time
        if (m_currentToxicity > 0)
        {
            m_currentToxicity -= Time.fixedDeltaTime * m_toxicityReductionSpeed;
            skullIcon_01.enabled = true;
            skullIcon_02.enabled = false;
        }

        if (m_currentToxicity > 50 && m_currentToxicity <= 70)
        {
            skullIcon_01.enabled = false;
            skullIcon_03.enabled = false;
            skullIcon_02.enabled = true;
        }

        if (m_currentToxicity >= 70)
        {
            skullIcon_01.enabled = false;
            skullIcon_02.enabled = false;
            skullIcon_03.enabled = true;
        }

        if (m_currentToxicity <= 0)
        {
            m_currentToxicity = 0;
        }

        //Debug.Log ("You're stung text off");
        toxicityRatio = m_currentToxicity / m_maxToxicity;
        if (toxicitySlider)
        {
            toxicitySlider.value = (toxicityRatio) * 100;
        }
        if (toxicityRatio >= 0 && boxVignette && splatImage)
        {
            boxVignette.color = currentColour + new Color (0, 0, 0, toxicityRatio);
            splatImage.color = currentColour + new Color (0, 0, 0, toxicityRatio);
        }

        m_previouslyGrounded = m_controller.isGrounded;

        if (m_tutorialTime > 0)
            m_tutorialTime -= Time.deltaTime;
        else
        {
            m_warning.SetActive(false);
            m_tutorialText.text = "";
            m_warningOn = false;
        }

    }

    //// This is for adding forces to anything that a body hits 
    //private void OnControllerColliderHit(ControllerColliderHit hit)
    //{
    //    Rigidbody body = hit.collider.attachedRigidbody;

    //    if (m_collisionFlags == CollisionFlags.Below)
    //        return;

    //    if (body == null || body.isKinematic)
    //        return;

    //    body.AddForceAtPosition(m_controller.velocity * 0.1f, hit.point, ForceMode.Impulse);
    //}

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "DrLeben")
        {
            m_playerCaught = true;
            m_camera.gameObject.SetActive(false);
            m_deathCameraObject.SetActive(true);
            onDeath.Invoke();
            DrLeben enemy = other.GetComponent<DrLeben>();
            if (enemy)
            {
                enemy.triggered = true;
                Debug.Log("Leben detected");
            }
        }
        //if (other.gameObject.tag == "PropBaton")
        //{
        //    other.gameObject.SetActive(false);
        //    onBatonPickup.Invoke();
        //    //playerBaton.SetActive(true);
        //    haveBaton = true;
        //    Debug.Log("Baton picked");
        //}
    }



    public void SetCoreText()
    {
        coreNumber_Text.text = coreCount.ToString() + "/" + totalCores;
        if (coreCount == totalCores)
            collectionComplete.Invoke();
    }

    //   void OnCollisionEnter(Collision col){
    //	if (col.gameObject.tag == "DrLeben")
    //		SceneManager.LoadScene ("DeathScene");
    //}

    private void RotateView()
    {
        //if (fpsMode && Input.GetKeyDown(KeyCode.Escape))
        //    fpsMode = !fpsMode;
        if (!fpsMode)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            return;
        }
        else
        {
            // Lock and hide cursor for look around movement
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        // Rotate axis depending on the mouse movement
        // Mouse movement in the x direction in screen space rotates the camera around the y axis of the transform
        float yRot = Input.GetAxis("Mouse X") * XSensitivity;
        float xRot = Input.GetAxis("Mouse Y") * YSensitivity;

        m_charRot *= Quaternion.Euler(0f, yRot, 0f);
        //m_cameraRot = Quaternion.Euler(-xRot, 0f, 0f) * m_cameraRot;

        transform.localRotation = m_charRot;
        //m_camera.transform.localRotation = m_cameraRot;

        m_camera.transform.localEulerAngles -= xRot * Vector3.right;

        Vector3 cameraAngle = m_camera.transform.localEulerAngles;

        if (cameraAngle.x > 85 && cameraAngle.x < 90)
            cameraAngle.x = 85;
        else if (cameraAngle.x < (360 - 85) && cameraAngle.x >= (360 - 90))
            cameraAngle.x = 360 - 85;

        //cameraAngle.y = 0;
        //cameraAngle.z = 0;
        m_camera.transform.localEulerAngles = cameraAngle;
    }

    private void GetInput(out float speed)
    {
        // Read Input
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // Holding shift causes player to sprint
        m_walking = !Input.GetButton("Run");

        // Set the desired speed
        speed = (m_walking || m_crouching) ? m_walkSpeed : m_runSpeed;

        // Slows player down depending on player's toxicity level
        speed *= (1 - m_currentToxicity / m_maxToxicity);

        // Stores input
        m_input = new Vector2(horizontal, vertical);

        // Normalise input so that player do not exceed maximum speed
        if (m_input.sqrMagnitude > 1)
            m_input.Normalize();
    }

    public void TakeDamage(float amount)
    {
        m_currentToxicity += amount;
        if (m_currentToxicity < 0)
        {
            m_currentToxicity = 0;
        }

        attacked.Invoke();

        // Player loses when player toxicity is higher than max toxicity
        if (m_currentToxicity > m_maxToxicity)
        {
            SceneManager.LoadScene("DeathScene");
            // Death
        }
    }

    private void UserInteract()
    {
        RaycastHit hitInfo;
        int layerMask = 1 << playerLayer;
        layerMask = ~layerMask;
        if (Physics.Raycast(m_camera.transform.position, m_camera.transform.forward, out hitInfo, m_interactRange, layerMask, QueryTriggerInteraction.Ignore))
        {
            // This is only for debugging purposes
            //Debug.Log("Layer of hit: " + hitInfo.collider.gameObject.layer);
            //if (Input.GetButtonDown("Interact"))
            //    Debug.Log("Player looking at: " + hitInfo.transform.tag);
            //if (hitInfo.transform.tag == "Door Panel")
            //{
            //    if (!hitInfo.transform.GetComponent<DoorPanel>().DoorSystem.RequireCore)
            //        press_E_Text.enabled = true;
            //    //if (hitInfo.transform.GetComponent<DoorPanel>().DoorSystem.RequireCore)
            //    //    requires_core_Text.enabled = true;
            //    if (Input.GetButtonDown("Interact"))
            //        hitInfo.transform.GetComponent<DoorPanel>().ToggleLock();
            //}
            if (hitInfo.transform.tag == "Interactable" || hitInfo.transform.tag == "Grate")
            {
                IInteractable interactableObject = null;
                if ((interactableObject = hitInfo.transform.gameObject.GetComponent<IInteractable>()) != null)
                {
                    // Display instructions for the player
                    if (playerInstruction != null)
                    {
                        //Debug.Log("Interactable call");
                        interactableObject.Description(playerInstruction);
                    }

                    // Calls interact function in the interactable script
                    if (Input.GetButtonDown("Interact"))
                        interactableObject.Interact(gameObject);
                    
                }
            }
            else
            {
                if (playerInstruction != null)
                {
                    playerInstruction.enabled = false;
                    playerInstruction.text = "";
                }
            }
            //else if (hitInfo.transform.tag == "FinalAreaDoor" && coreCount < totalCore)
            //{
            //    requires_core_Text.enabled = true;
            //}

    //        else if (hitInfo.transform.name == "AirlockButton")
    //        {
    //            activate_Airlock_Text.enabled = true;
    //        }

    //        else
    //        {
    //            press_E_Text.enabled = false;
				//requires_core_Text.enabled = false;
    //            activate_Airlock_Text.enabled = false;
    //        }
        }
        else
        {
            if (playerInstruction != null)
            {
                playerInstruction.enabled = false;
                playerInstruction.text = "";
            }
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // Temporary Baton Script
        if (m_stunTime > 0)
        {
            batonParticles.Play();
            batonSource.Play();
            m_stunTime -= Time.fixedDeltaTime;
        }
            

        m_stunTimerMat.SetFloat("_StunColourPosition", 1 - (m_stunTime / m_stunTimer));

        if (m_stunTime <= 0)
        {
            m_stunTimerMat.SetColor("_Color", new Color(47 / 255f, 98f / 255f, 1f, 1f));
            m_batonMat.EnableKeyword("_EMISSION");
        }
        else
        {
            batonSource.Stop();
            m_stunTimerMat.SetColor("_Color", Color.red);
            m_batonMat.DisableKeyword("_EMISSION");
        }

        if (haveBaton && Physics.Raycast(m_camera.transform.position, m_camera.transform.forward, out hitInfo, m_batonRange, layerMask, QueryTriggerInteraction.Ignore))
        {
            if (Input.GetButtonDown("Stun") && m_stunTime <= 0)
            {
                if (hitInfo.transform.tag == "Scarab" || hitInfo.transform.tag == "Wandering Scarab")
                {
                    Scarab reference;
                    if ((reference = hitInfo.transform.gameObject.GetComponent<Scarab>()) != null)
                    {
                        reference.TakeDamage(DamageType.STUN);
                        m_stunTime = m_stunTimer;
                    }
                }
                else if (hitInfo.transform.tag == "Grate")
                {
                    hitInfo.transform.gameObject.GetComponent<GrateScript>().PlayGrateEvent();
                    m_stunTime = m_stunTimer;
                }
            }
            else if (Input.GetButtonDown("Hit"))
            {
                if (hitInfo.transform.tag == "Scarab" || hitInfo.transform.tag == "Wandering Scarab")
                {
                    Scarab reference;
                    if ((reference = hitInfo.transform.gameObject.GetComponent<Scarab>()) != null)
                    {
                        reference.TakeDamage(DamageType.KILL);
                        if (firstScarab == false)
                        {
                            WarnPlayer("Killing Scarabs alerts Dr. Leben");
                            firstScarab = true;
                        }
                    }
                }
            }
        }
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        Debug.DrawRay(m_camera.transform.position, m_camera.transform.forward * m_batonRange);
    }

    private void Crouch()
    {
        if (Input.GetButton("Crouch") && m_controller.isGrounded)
        {
            if (m_crouching)
                return;
            m_controller.height /= 2.0f;
            m_controller.center /= 2.0f;
            m_camera.gameObject.transform.localPosition /= 2.0f;

            m_crouching = true;
            //m_camera.transform.localPosition -= new Vector3(0, m_crouchHeight, 0);
            //m_playerCollider.height = (m_crouchHeight - m_playerCollider.radius) * 2;
        }
        else if (m_crouching && !Input.GetButton("Crouch"))
        {
            RaycastHit hit;

            // Ignores player's layer
            var layerMask = (1 << gameObject.layer);
            layerMask = ~layerMask;

            //Makes qa sphere cast above to check if anything overhead
            Physics.SphereCast(transform.position, m_controller.radius, Vector3.up * 0.1f, out hit, m_originalHeight + 2.0f * m_controller.radius, layerMask);

            // If an object is above that can hit the player, don't stand up
            if (hit.collider != null && hit.collider.gameObject.tag != "Player")
            {
                m_crouching = true;
                return;
            }

            // Resets height and centre of the character controller
            m_controller.height = m_originalHeight;
            m_controller.center = m_originalCentre;
            m_camera.gameObject.transform.localPosition = m_cameraStandPosition;
            m_crouching = false;
        }
    }

    private void HandAnimation()
    {
        if (!m_handAnimator || !haveBaton)
            return;

        if (!m_walking)
        {
            m_handAnimator.SetFloat("Speed", 2);
            if (m_hand)
                m_hand.transform.localPosition = m_handRunPosition;
        }
        else
        {
            m_handAnimator.SetFloat("Speed", m_moveDir.x / m_walkSpeed);
            if (m_hand)
                m_hand.transform.localPosition = m_handWalkPosition;
        }
        //else if (m_moveDir.x > 0)
        //    m_handAnimator.SetFloat("Speed", 1);
        //else
        //    m_handAnimator.SetFloat("Speed", 0);

        if (haveBaton)
        {
            if (Input.GetButtonDown("Stun"))
            {
                onBatonStun.Invoke();
            }
            else if (Input.GetButtonDown("Hit"))
                onBatonHit.Invoke();
        }
    }

    public void WarnPlayer(string message)
    {
        m_warning.SetActive(true);
        m_tutorialText.text = message;
        m_warningOn = true;
        m_tutorialTime = m_tutorialTimer;
    }

    public void BatonFade()
    {
        gameFader.FadeGame(1, true);
        fadeStart = true;
        m_restrictMovement = true;
        fadeTimer = 1;
    }

    public void FadeToPlayer()
    {
        gameFader.FadeGame(1, false);
        m_restrictMovement = false;
    }


























    ////float asasdafdefdsf = 0.5f;
    //float timer = 0f;
    //private  void MikeyTripping()
    //{
    //    if (Input.GetKey(KeyCode.PageDown))
    //    {
    //        Rigidbody dundun;
    //        if (!GetComponent<Rigidbody>())
    //        {
    //            dundun = gameObject.AddComponent<Rigidbody>();
    //        }
    //        else
    //        {
    //            dundun = GetComponent<Rigidbody>();
    //        }
    //        dundun.isKinematic = false;
    //        GetComponent<Collider>().enabled = true;
    //        GetComponent<CharacterController>().enabled = false;
    //        if (!gameObject.GetComponent<StandUpAgain>())
    //        {
    //            gameObject.AddComponent<StandUpAgain>();
    //        }
    //        else
    //        {
    //            gameObject.GetComponent<StandUpAgain>().enabled = true;
    //        }
    //        dundun.AddExplosionForce(500, transform.position + transform.forward * -1, 100);
    //        dundun.AddExplosionForce(100, transform.position + transform.right * 1, 100);

    //        this.enabled = false;


    //    }
    //}
}
