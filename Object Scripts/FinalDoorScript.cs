using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FinalDoorScript : MonoBehaviour {

    public FinalDoorCheck finalTrigger;
    public Vector3 closedPos;
    public Vector3 openPos;
    private float timer = 0;
    public float transitionTime = 3f;
    public bool closeDoor = false;


    [SerializeField] private Vector3 closeTranslation = new Vector3(0, 0, -3f);
    [SerializeField] private float movementSpeed = 2;

    // Position of the door when it is open and when it is closed
    /*[SerializeField] */
    private Vector3 m_openPosition;
    /*[SerializeField] */
    private Vector3 m_closedPosition;

    [SerializeField] private bool m_isLocked = false;

    private Vector3 m_targetPosition;
    private bool m_targetReached = true;


    private DoorSystem m_systemInstance = null;
    private bool toggleMemory = false;

    private void Start()
    {
        finalTrigger = GetComponentInParent<FinalDoorCheck>();

        // Set up the open and closed position of the doors
        m_openPosition = transform.localPosition;
        m_closedPosition = m_openPosition;
        m_closedPosition += closeTranslation;

        // Automatically closes all doors
        m_targetPosition = m_closedPosition;
        m_targetReached = false;
    }

    private void FixedUpdate()
    {
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
        if (!m_isLocked)
        {
            // Toggles between open and closed position depending on whether it is already open or not
            m_targetPosition = openDoor ? m_openPosition : m_closedPosition;
            m_targetReached = false;
            //Debug.Log("Door status called and break door is: " + breakDoor);
        }
    }





}
