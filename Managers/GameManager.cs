using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Code written by Antoine Kenneth Odi in 2018

public class GameManager : MonoBehaviour {

    public bool canvasOpened = false;
    public bool mapActive = false;
    public bool terminalActive = false;
    public bool paused = false;
    public bool resume = false;
    private Player player = null;
    [SerializeField] private GameObject pauseMenu;
    private bool optionOpened = false;

    void Start()
    {
        player = FindObjectOfType<Player>();
    }


    void Update()
    {
        // This is used to toggle in game menu
        if ((Input.GetButtonDown("Cancel") || resume) && !optionOpened && paused == true && canvasOpened == false)
        {
            player.fpsMode = true;
            paused = false;
            pauseMenu.SetActive(false);
            resume = false;
            Time.timeScale = 1f;
        }
        else if (Input.GetButtonDown("Cancel") && !optionOpened && canvasOpened == false)
        {
            player.fpsMode = false;
            paused = true;
            pauseMenu.SetActive(true);
            Time.timeScale = 0f;
        }

        // This is used to reference if canvases are open or not
        if (mapActive || terminalActive)
            canvasOpened = true;
        else
            canvasOpened = false;

    }

    public void Resume()
    {
        resume = true;
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void OptionOpened(bool openStatus)
    {
        optionOpened = openStatus;
    }
}
