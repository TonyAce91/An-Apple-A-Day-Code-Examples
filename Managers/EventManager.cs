using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// Code written by Antoine Kenneth Odi in 2018

public class EventManager : MonoBehaviour {

    [System.Serializable]
    public class AlertEvent : UnityEvent<Vector3>
    {

    }

    [HideInInspector] public bool alertedState = false;
    [HideInInspector] public Vector3 alertPosition;
    public AlertEvent alertEvent;

    private DrLeben m_lebenScript;

	// Use this for initialization
	void Start () {
        m_lebenScript = FindObjectOfType<DrLeben>();
	}

    public void AlarmEvent(Vector3 position)
    {
        alertEvent.Invoke(position);
    }

}
