using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// Code made by Antoine Kenneth Odi in 2018

public class HandAnimations : MonoBehaviour {

    private Animator m_handAnimator;
    private Player m_player;
    private bool startDone = false;
    private GameFader m_fader;
    [SerializeField] private float m_fadeTime = 5f;

    public UnityEvent onStartAnimation;

    private void Start()
    {
        m_handAnimator = GetComponent<Animator>();
        m_player = FindObjectOfType<Player>();
        m_fader = FindObjectOfType<GameFader>();

    }

    private void Update()
    {
        if (m_fadeTime > 0)
            m_fadeTime -= Time.deltaTime;

        if (!startDone && m_fadeTime <= 0)
        {
            Debug.Log("Animation called");
            onStartAnimation.Invoke();
            startDone = true;
        }
    }

    // Calls fade to fade the game
    public void CallFader()
    {
        if (m_fader)
            m_fader.FadeGame(5, false);
    }

    // Turns off the arm game object when baton is not acquired
    public void TurnOffArm()
    {
        m_handAnimator.SetBool("Start", false);
        m_player.m_restrictMovement = false;
        gameObject.SetActive(false);
    }
}
