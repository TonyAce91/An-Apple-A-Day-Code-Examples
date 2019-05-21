using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Code written by Antoine Kenneth Odi in 2018


public class CoreScript : MonoBehaviour, IDetectable, IInteractable {

    private EventManager m_eventManager;
    private Player m_player;
    [SerializeField] private List<DoorSystem> securityDoors = new List<DoorSystem>();
    private CoreBaseScript m_baseScript;
    [SerializeField] private Light m_coreLight;
    [SerializeField] private List<Light> m_roomLights = new List<Light>();

    //public UnityEvent onCorePickup;

	// Use this for initialization
	void Start () {
        m_eventManager = FindObjectOfType<EventManager>();
        m_player = FindObjectOfType<Player>();
        securityDoors.TrimExcess();
        m_roomLights.TrimExcess();
        gameObject.tag = "Interactable";
        m_baseScript = GetComponentInParent<CoreBaseScript>();
    }
	
    public void AlertEvent()
    {
        m_eventManager.AlarmEvent(transform.position);
    }

    public void Interact(GameObject actor)
    {
        Player playerInstance = actor.GetComponent<Player>();
        if (playerInstance)
        {
            // Updates player core count
            playerInstance.coreCount++;
            playerInstance.SetCoreText();

            // Tutorial part of the script
            if (playerInstance.firstCore == false)
            {
                playerInstance.firstCore = true;
                playerInstance.WarnPlayer("Removing cores will alert him, be careful when you take one out");
            }

            // Sets alert for Dr Leben
            AlertEvent();

            // Sets the event for core pickup on the base script
            if (m_baseScript)
                m_baseScript.onCorePickup.Invoke();

            // Turns off light coming from the core
            if (m_coreLight)
                m_coreLight.gameObject.SetActive(false);

            if (m_roomLights.Count > 0)
                foreach(Light roomLight in m_roomLights)
                {
                    roomLight.intensity /= 2f;
                }

            // Turns off the game object when picked up
            gameObject.SetActive(false);

            // Breaks all security doors
            foreach (DoorSystem securityDoor in securityDoors)
                securityDoor.BrokenDoors = true;
        }
    }

    public void Description(Text instructionText)
    {
        instructionText.enabled = true;
        instructionText.text = "Press 'E' to pick up";
    }
}
