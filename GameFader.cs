using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

// Code written by Antoine Kenneth Odi in 2018

public class GameFader : MonoBehaviour {

    [SerializeField] private Image m_blackImage = null;
    [SerializeField] private bool m_startFade = false;
    private bool m_isFadeOut = false;

    [SerializeField] private float m_fadeTimer = 0;
    [SerializeField] private float m_fadeWaitDuration = 0;

    float currentRatio = 0;

    public UnityEvent afterFadeIn;
    public UnityEvent afterFadeOut;

    private float m_fadeTime = 0;
    private float m_fadeWaitTime = 0;
    private Color m_targetColour = Color.white;
    private Color m_currentColour = Color.black;
    private float m_lerpTime = 0;

	// Use this for initialization
	void Start () {
        //FadeGame(m_fadeTimer, false, m_fadeWaitDuration);
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (m_startFade)
        {
            if(m_fadeWaitTime > 0)
            {
                m_fadeWaitTime -= Time.fixedDeltaTime;
                return;
            }
            m_lerpTime -= Time.fixedDeltaTime/m_fadeTimer;
            m_blackImage.color = Color.Lerp(m_targetColour, m_currentColour, m_lerpTime);
            if (m_lerpTime <= 0)
            {
                m_startFade = false;
                if (m_isFadeOut)
                    afterFadeOut.Invoke();
                else
                    afterFadeIn.Invoke();
            }
        }
    }

    public void FadeGame(float fadeTimer = 10f, bool isFadeOut = false, float fadeWaitTime = 0f)
    {
        m_startFade = true;
        m_isFadeOut = isFadeOut;
        m_fadeTime = fadeTimer;
        m_fadeTimer = fadeTimer;
        m_fadeWaitTime = fadeWaitTime;
        m_currentColour = m_blackImage.color;
        m_targetColour = isFadeOut ? Color.black : new Color (1, 1, 1, 0);
        m_lerpTime = m_fadeTime / m_fadeTimer;
    }

}
