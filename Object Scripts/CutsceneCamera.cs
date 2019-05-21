using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


// Code written by Antoine Kenneth Odi in 2018


public class CutsceneCamera : MonoBehaviour {

    private int movementNumber = 0;
    [SerializeField] private float fadeTimer = 5f;
    [SerializeField] private float translateTimer = 5f;
    [SerializeField] private float zoomTimer = 5f;
    [SerializeField] private float rotationTimer = 4f;
    [SerializeField] private float rotationSpeed = 2f;

    [SerializeField] private Vector3 originalPos = new Vector3(0.55f, 1, 20);
    [SerializeField] private Vector3 translatePos = new Vector3(0.55f, 1f, 18.7f);
    [SerializeField] private Vector3 zoomPos = new Vector3(-1, 1.85f, 18.7f);
    [SerializeField] private Vector3 relativeRotation = new Vector3(0, 180, 0);
    private float m_lerpTime;
    private float m_currentTimer;
    private GameFader gameFader = null;
    private Quaternion targetRot;
    private Quaternion originalRot;
    private float step;
    [SerializeField] private Transform lebenMiddle;
    private Player player;

    public UnityEvent afterCutscene;

    // Use this for initialization
    void OnEnable() {
        //m_currentTimer
        gameFader = FindObjectOfType<GameFader>();
        gameFader.FadeGame(fadeTimer, false);
        m_lerpTime = 1;
        targetRot = Quaternion.LookRotation(relativeRotation);
        originalRot = transform.rotation;
        player = FindObjectOfType<Player>();
	}
	
	// Update is called once per frame
	void Update () {
        if (movementNumber == 0)
        {
            m_lerpTime -= Time.deltaTime / translateTimer;
            transform.position = Vector3.Lerp(translatePos, originalPos, m_lerpTime);
            if (m_lerpTime <= 0)
            {
                movementNumber++;
                m_lerpTime = 1;
                originalPos = transform.position;
            }
        }
        else if (movementNumber == 1)
        {
            m_lerpTime -= Time.deltaTime / zoomTimer;
            transform.position = Vector3.Lerp(zoomPos, originalPos, m_lerpTime);
            if (m_lerpTime <= 0)
            {
                movementNumber++;
                m_lerpTime = rotationTimer;
                originalPos = transform.position;
            }
        }
        else if (movementNumber == 2)
        {
            m_lerpTime -= Time.deltaTime;
            step = rotationSpeed * Time.deltaTime;
            Quaternion targetRot = Quaternion.LookRotation(lebenMiddle.transform.position - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, step);
            if (m_lerpTime <= 0)
            {
                movementNumber++;
                m_lerpTime = 0.5f;
            }
        }
        else if (movementNumber == 3)
        {
            gameFader.FadeGame(0.5f, true);
            m_lerpTime -= Time.deltaTime;
            if (m_lerpTime <= 0)
            {
                movementNumber++;
            }
        }
        else if (movementNumber == 4)
        {
            player.FadeToPlayer();
            afterCutscene.Invoke();
            gameObject.SetActive(false);
        }
    }
}
