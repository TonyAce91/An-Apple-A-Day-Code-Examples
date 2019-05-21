using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Code written by Antoine Kenneth Odi in 2018

public class GrateScript : MonoBehaviour, IInteractable {

    public UnityEvent grateEffects;
    AudioSource audioSource;

    // Play events when grate is destroyed
    public void PlayGrateEvent()
    {
        grateEffects.Invoke();
    }

    public void Interact(GameObject actor)
    {
    }

    // Shows a description as to what to do with grates when it's still close
    public void Description(Text instructionText)
    {
        instructionText.enabled = true;
        instructionText.text = "Use baton's electricity to turn off lasers";
    }
}
