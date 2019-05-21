using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Code written by Antoine Kenneth Odi in 2018


public interface IInteractable
{
    void Interact(GameObject actor);
    void Description(Text instructionText);
}
