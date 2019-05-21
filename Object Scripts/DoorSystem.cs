using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.AI;

// Code written by Antoine Kenneth Odi in 2018

[RequireComponent(typeof(AudioSource))]
public class DoorSystem : MonoBehaviour, IDetectable
{
    // List of door panels and doors
    [SerializeField] private List<DoorPanel> m_panels;
    [SerializeField] private List<Door> m_doors;

    // Door aesthetics
    [SerializeField] private List<Light> m_doorLights = new List<Light>();
    [SerializeField] private Color m_securityColour = new Color(0, 1, 0, 1);

    // Drill mechanics for Dr. Leben
    [SerializeField] private float drillTimer = 5f;
    private float drillTime = 0f;
    [SerializeField] private NavMeshObstacle m_lebenObstacle;

    private EventManager m_eventManager;

    // Type of door
    public bool m_requireCore = false;
    public int numberOfCores = 0;
    private bool m_lockedDoors = false;
    private bool m_brokenDoors = false;

    // List of door events
    public UnityEvent onDoorOpen;
    public UnityEvent onDoorClose;
    public UnityEvent onDoorLock;
    public UnityEvent onDoorUnlock;
    public UnityEvent onDoorBreak;

    #region Properties
    public bool RequireCore
    {
        get { return m_requireCore; }
        set { m_requireCore = value; }
    }

    public bool LockedDoors
    {
        get { return m_lockedDoors; }
        set {
            if (m_lockedDoors != value)
            {
                m_lockedDoors = value;
                if (m_lockedDoors)
                {
                    onDoorLock.Invoke();
                    AlertEvent();
                }
                else
                    onDoorUnlock.Invoke();
            }
		}
    }

    public bool BrokenDoors
    {
        get { return m_brokenDoors; }
        set
        {
            m_brokenDoors = value;
            if (m_brokenDoors)
            {
                foreach (Door door in m_doors)
                    door.DoorStatus(true, true);
                if (m_lebenObstacle)
                    m_lebenObstacle.enabled = false;
                onDoorBreak.Invoke();
            }
        }
    }

    #endregion


    [HideInInspector] public int m_reference = 0;

    // Use this for initialization
    void Start()
    {
        //onDoorOpen.AddListener();
        // Set up event manager
        m_eventManager = FindObjectOfType<EventManager>();
        drillTime = drillTimer;

        m_lebenObstacle = GetComponent<NavMeshObstacle>();
        if (m_lebenObstacle)
            m_lebenObstacle.enabled = true;

        m_panels.TrimExcess();
        m_doors.TrimExcess();
        m_doorLights.TrimExcess();

        // Check if a panel has been allocated then allocate the doors and system accordingly
        if (m_panels.Count > 0 && m_doors.Count > 0)
        {
            foreach (DoorPanel panel in m_panels)
            {
                panel.doors = m_doors;
                panel.SystemInstance = this;
            }
        }

        // Closes all doors at the start of the game
        if (m_doors.Count > 0)
        {
            foreach (Door door in m_doors)
            {
                //door.DoorStatus(false);
                door.SystemInstance = this;
            }
        }

        if (m_doorLights.Count > 0)
        {
            foreach (Light light in m_doorLights)
                light.color = new Color(6f / 255f, 152f / 255f, 1f, 1f);
        }

        if (m_doorLights.Count > 0 && m_requireCore)
        {
            foreach (Light light in m_doorLights)
                light.color = new Color(106f / 255f, 1f, 0f, 1f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (m_doors.Count > 0)
        {
            // Opens door when player or Dr Leben goes through normal doors
            if (other.tag == "Player" || (other.tag == "DrLeben" && !m_requireCore))
            {
                m_reference++;
                foreach (Door door in m_doors)
                    door.DoorStatus(true);
                if (!m_lockedDoors && !m_brokenDoors)
                    onDoorOpen.Invoke();
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        // Breaks door when Dr Leben is detected drill door for some time
        if (other.tag == "DrLeben" && m_panels.Count > 0 && m_lockedDoors && !m_requireCore)
        {
            if (drillTime > 0)
                drillTime -= Time.fixedDeltaTime;
            else if (drillTime <= 0)
            {
                BrokenDoors = true;
                DrLeben m_lebenScript = other.GetComponent<DrLeben>();
                if (m_lebenScript && m_requireCore)
                    m_lebenScript.FinishDrilling();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Closes door when player or Dr Leben is not on the trigger
        if (other.tag == "Player" || (other.tag == "DrLeben" && !m_requireCore))
        {
            m_reference--;
            if (!m_lockedDoors && !m_brokenDoors)
                onDoorClose.Invoke();
            if (m_requireCore)
            {
                ChangeLights(m_securityColour);
            }
        }
        if (m_doors.Count > 0 && m_reference <= 0)
        {
            foreach (Door door in m_doors)
            {
                door.DoorStatus(false);
            }
        }
    }

    // Changes door lights when door gets locked or unlocked
    public void ChangeLights(Color lightColour)
    {
        if (m_doorLights.Count > 0)
        {
            foreach (Light light in m_doorLights)
                light.color = lightColour;
        }
    }

    // Use to alert the doctor to the locked door's position
    public void AlertEvent()
    {
        if (!m_eventManager)
            Debug.Log("Can't detect event manager");
        m_eventManager.AlarmEvent(transform.position);
    }
}