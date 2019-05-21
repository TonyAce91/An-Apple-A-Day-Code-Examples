using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    private EventManager m_eventManager;
    private DrLeben m_lebenScript;
    private bool m_lebenTeleported = false;
    [SerializeField] private Vector3 m_teleportLocation;
    [SerializeField] private Transform m_teleportLocationTransform;

    // Use this for initialization
    void Start()
    {
        m_eventManager = FindObjectOfType<EventManager>();
        m_lebenScript = FindObjectOfType<DrLeben>();
        if (m_teleportLocationTransform)
            m_teleportLocation = m_teleportLocationTransform.position;
    }


    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && !m_lebenTeleported)
        {
            Debug.Log("Teleport called");
            m_lebenTeleported = m_lebenScript.navAgent.Warp(m_teleportLocation);
            AlertEvent();
        }
    }
    public void AlertEvent()
    {
        m_eventManager.AlarmEvent(transform.position);
    }
}
