using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Code written by Antoine Kenneth Odi in 2018

public class BatonScript : MonoBehaviour, IInteractable
{
    private void Start()
    {
        gameObject.tag = "Interactable";
    }

    // This is called when the player looks at the item
    public void Description(Text instructionText)
    {
        instructionText.enabled = true;
        instructionText.text = "Press 'E' to pick up";
    }

    // This is called when player interacts with it
    public void Interact(GameObject actor)
    {
        Player playerInstance = actor.GetComponent<Player>();
        if (playerInstance)
        {
            playerInstance.onBatonPickup.Invoke();
            playerInstance.haveBaton = true;
            gameObject.SetActive(false);
        }
    }
}