using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerPlayer : MonoBehaviour {

    public UnityEvent onTrigger;
    private Player m_player;
    

	// Use this for initialization
	void Start () {
        m_player = FindObjectOfType<Player>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "DrLeben")
        {
            onTrigger.Invoke();
            m_player.m_restrictMovement = false;
            m_player.faceLeben = false;
        }
    }
}
