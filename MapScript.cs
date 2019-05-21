using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapScript : MonoBehaviour, IInteractable {

    public GameObject mapObjs;
    private bool mapActive = false;
    [SerializeField] private Player player;
    [SerializeField] private GameObject m_MapLocation;
    private GameManager manager = null;

    void Start()
    {
        gameObject.tag = "Interactable";
        player = FindObjectOfType<Player>();
        manager = FindObjectOfType<GameManager>();
    }

    void Update()
    {
        if (mapActive && Input.GetButtonDown("Cancel"))
        {
            player.fpsMode = true;
            mapObjs.SetActive(false);
            mapActive = false;
            revealLocation(false);
            manager.mapActive = false;
            Debug.Log("Manager Canvas Status: " + manager.canvasOpened);
        }
    }

    public void Description(Text instructionText)
    {
        if (!mapActive)
        {
            instructionText.enabled = true;
            instructionText.text = "Press 'E' to open map";
        }
        else
            instructionText.text = "";
    }

    public void Interact(GameObject actor)
    {
        Debug.Log("Interact function called");
        if (mapObjs != null)
        {
            Debug.Log("Map reference is set");
            mapObjs.SetActive(true);
            mapActive = true;
            player.fpsMode = false;
            revealLocation(true);
            manager.mapActive = true;
            Debug.Log("Manager Canvas Status: " + manager.canvasOpened);
        }
    }

    public void revealLocation(bool status = true)
    {
        if (m_MapLocation)
            m_MapLocation.SetActive(status);
        else
            Debug.Log("Map Location not set");
    }

}
