using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exit : MonoBehaviour
{
    private EventManager m_eventManager;
    [SerializeField] private InitialLeben lebenScript;

    // Use this for initialization
    void Start()
    {
        m_eventManager = FindObjectOfType<EventManager>();
        lebenScript = FindObjectOfType<InitialLeben>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && enabled == true)
        {
            if(lebenScript)
                lebenScript.playerLeaving = true;
            AlertEvent();
            enabled = false;
        }
    }

    public void AlertEvent()
    {
        m_eventManager.AlarmEvent(transform.position);
    }

}
