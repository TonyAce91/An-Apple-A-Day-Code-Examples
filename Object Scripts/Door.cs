using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Code written by Antoine Kenneth Odi in 2018

[RequireComponent(typeof(AudioSource))]
public class Door : MonoBehaviour
{
    [SerializeField] private Vector3 closeTranslation = new Vector3(0, 0, -3f);
    [SerializeField] private float movementSpeed = 2;
    [SerializeField] private Texture m_lockedTexture;
    [SerializeField] private Texture m_unlockedTexture;

    // Position of the door when it is open and when it is closed
    /*[SerializeField] */
    private Vector3 m_openPosition;
    /*[SerializeField] */
    private Vector3 m_closedPosition;

    [SerializeField] private bool m_isLocked = false;

    private Material m_doorMaterial;


    public bool brokenDoor = false;

    private Vector3 m_targetPosition;
    private bool m_targetReached = true;


    private DoorSystem m_systemInstance = null;
    private bool toggleMemory = false;

    #region Properties
    public DoorSystem SystemInstance
    {
        get { return m_systemInstance; }
        set { m_systemInstance = value; }
    }


    #endregion



    // Use this for initialization
    void Awake()
    {
        // Set up the open and closed position of the doors
        m_openPosition = transform.localPosition;
        m_closedPosition = m_openPosition;
        m_closedPosition += closeTranslation;

        // Automatically closes all doors
        m_targetPosition = m_closedPosition;
        m_targetReached = false;

        // Set up door material and checking for any textures
        m_doorMaterial = GetComponent<Renderer>().material;

        if (m_unlockedTexture && m_lockedTexture)
            m_doorMaterial.SetTexture("_EmissionMap", m_unlockedTexture);

        // Setting door object tag on doors
        gameObject.tag = "DoorObject";
    }


    private void FixedUpdate()
    {

        if (brokenDoor && m_targetPosition == m_openPosition && m_targetReached)
            enabled = false;

        // Move the door smoothly between open and closed position
        // It slows down as it comes to a halt
        if (!m_targetReached)
            transform.localPosition = Vector3.Lerp(transform.localPosition, m_targetPosition, movementSpeed * Time.fixedDeltaTime);

        // This detects whether it reach its target position
        if ((transform.localPosition - m_targetPosition).sqrMagnitude <= 0.001f)
            m_targetReached = true;

    }

    public void DoorStatus(bool openDoor = false, bool breakDoor = false)
    {
        if (!brokenDoor && (!m_isLocked || breakDoor))
        {
            // Toggles between open and closed position depending on whether it is already open or not
            m_targetPosition = openDoor ? m_openPosition : m_closedPosition;
            m_targetReached = false;
            //Debug.Log("Door status called and break door is: " + breakDoor);
        }

        // Checks if door is broken and set desired status if not
        if (!brokenDoor)
            brokenDoor = breakDoor;
    }


    public void ToggleLock()
    {
        //toggleMemory = true;
        if (!m_isLocked)
        {
            m_targetPosition = m_closedPosition;
            if (m_lockedTexture)
            {
                m_doorMaterial.SetTexture("_EmissionMap", m_lockedTexture);
                m_systemInstance.ChangeLights(new Color(1f, 42f / 255f, 6f / 255f, 1f));
            }
            //Debug.Log("Doors are locking now");
        }
        else if (m_unlockedTexture)
        {
            m_doorMaterial.SetTexture("_EmissionMap", m_unlockedTexture);
            m_systemInstance.ChangeLights(new Color(6f / 255f, 152f / 255f, 1f, 1f));
        }
        m_targetReached = false;
        m_isLocked = !m_isLocked;
        //toggleMemory = false;
    }
}
