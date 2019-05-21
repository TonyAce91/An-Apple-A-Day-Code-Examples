using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Code made by Georgia Bryant in 2018
// Modified some sections by Antoine Kenneth Odi in 2018

public class ActivateTerminal : MonoBehaviour
{

    [SerializeField] private int playerLayer = 10;
    [SerializeField] private Camera m_camera;
    [SerializeField] private float m_interactRange = 5f;

    [SerializeField] private Player player;

    private GameManager manager;


    bool activateMonitor = false;
    bool mapCanvasOn = false;

    // Use this for initialization
    void Start()
    {
        player = gameObject.GetComponent<Player>();
        manager = FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        InteractTerminal();
        if (activateMonitor)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }

    private void InteractTerminal()
    {
        RaycastHit hitInfo;
        int layerMask = 1 << playerLayer;
        layerMask = ~layerMask;
        if (Physics.Raycast(m_camera.transform.position, m_camera.transform.forward, out hitInfo, m_interactRange, layerMask)){
            if (hitInfo.transform.tag == "Terminal")
            {
                if (Input.GetButtonDown("Interact") && !activateMonitor)
                {
                    player.fpsMode = false;
                    activateMonitor = true;
                    manager.terminalActive = true;
                    Monitor monitorScript = hitInfo.transform.GetComponent<Monitor>();
                    if (monitorScript)
                    {
                        Debug.Log("Show story canvas");
                        monitorScript.revealText();
                        //monitorObjs.revealLocation(true);
                    }
                    else
                        Debug.Log("No monitor script");
                }
                else if (activateMonitor && Input.GetButtonDown("Cancel"))
                {
                    player.fpsMode = true;
                    activateMonitor = false;
                    manager.terminalActive = false;
                    Monitor monitorObjs = hitInfo.transform.GetComponent<Monitor>();
                    if (monitorObjs)
                    {
                        monitorObjs.hideText();
                        //monitorObjs.revealLocation(false);
                    }
                    else
                        Debug.Log("No monitor script");
                }
            }
        }
    }
}
