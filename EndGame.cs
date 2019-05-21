using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class EndGame : MonoBehaviour {
    [SerializeField] private float m_fadeTimer = 10;
    private float m_fadeTime;
    private GameFader m_fader;
    private bool finishGame = false;

    // Use this for initialization
    void Start()
    {
        m_fader = FindObjectOfType<GameFader>();
    }

    private void FixedUpdate()
    {
        // Loads the win scene after fade timer
        if (m_fadeTime > 0)
            m_fadeTime -= Time.fixedDeltaTime;
        if (finishGame && m_fadeTime <= 0)
            SceneManager.LoadSceneAsync("WinScene");
    }


    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            // Calls the game fader to fade into black
            m_fader.FadeGame(m_fadeTimer, true);
            m_fadeTime = m_fadeTimer;
            finishGame = true;
        }
    }
}
