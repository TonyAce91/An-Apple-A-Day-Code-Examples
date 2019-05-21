using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Code written by Antoine Kenneth Odi in 2018

public class AntidoteScript : MonoBehaviour, IInteractable
{

    [SerializeField] private List<Light> m_appleLights = new List<Light>();
    [SerializeField] private GameObject m_hologramApple;

    public UnityEvent onAntidotePickup;

    void Start()
    {
        gameObject.tag = "Interactable";
        m_appleLights.TrimExcess();
    }

    // This is called when player looks at it
    public void Description(Text instructionText)
    {
        instructionText.enabled = true;
        instructionText.text = "'E' to pick up antidote, 'F' to use when picked up";
    }

    // This is called when player interacts with the item
    public void Interact(GameObject actor)
    {
        CollectAntidote collectionInstance = actor.GetComponent<CollectAntidote>();
        if (collectionInstance)
        {
            // Unity events
            onAntidotePickup.Invoke();
            collectionInstance.UIUpdate();

            // Turns off lights when antidotes are collected
            foreach (Light appleLight in m_appleLights)
                appleLight.enabled = false;

            // Turns off hologram when collected
            m_hologramApple.SetActive(false);

            Collider collider;
            if ((collider = gameObject.GetComponent<Collider>())!= null)
                collider.enabled = false;
        }
    }
}