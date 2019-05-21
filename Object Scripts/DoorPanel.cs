using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Code written by Antoine Kenneth Odi in 2018

[RequireComponent(typeof(AudioSource))]
public class DoorPanel : MonoBehaviour, IInteractable
{
    public List<Door> doors = new List<Door>();
    [SerializeField] private float toggleTimer = 0.5f;
	private float toggleTime = 0;
    private DoorSystem m_systemInstance = null;
    [SerializeField] private bool m_lockAtStart = false;

    public UnityEvent onDoorPanelLock;
    public UnityEvent onDoorPanelUnlock;

    #region Properties
    public DoorSystem SystemInstance
    {
        get { return m_systemInstance; }
        set { m_systemInstance = value; }
    }
    #endregion

    private void Start()
    {
        doors.TrimExcess();
        gameObject.tag = "Interactable";
    }

    // Update is called once per frame
    private void FixedUpdate()
    {

        if (m_lockAtStart)
        {
            if (!m_systemInstance.LockedDoors)
                ToggleLock();
            m_lockAtStart = false;
        }

        if (toggleTime > 0)
            toggleTime -= Time.fixedDeltaTime;

    }

    public void ToggleLock()
    {
        if (!m_systemInstance.BrokenDoors && toggleTime <= 0)
        {
            m_systemInstance.LockedDoors = !m_systemInstance.LockedDoors;
            if (m_systemInstance.LockedDoors)
                onDoorPanelLock.Invoke();
            else
                onDoorPanelUnlock.Invoke();

            // Checks if any entity on trigger then automatically opens on unlock accordingly
            bool openDoors = (!m_systemInstance.LockedDoors && m_systemInstance.m_reference > 0);
            foreach (Door door in doors)
            {
                door.ToggleLock();
                if (openDoors)
                    door.DoorStatus(true);
            }
            toggleTime = toggleTimer;
        }
    }

    public void Interact(GameObject actor)
    {
        if (!SystemInstance.RequireCore)
            ToggleLock();
    }

    public void Description(Text instructionText)
    {
        if (!SystemInstance.RequireCore)
        {
            instructionText.enabled = true;
            instructionText.text = (m_systemInstance.LockedDoors) ? "Press 'E' to unlock door" : "Press 'E' to lock door";
        }
    }
}
