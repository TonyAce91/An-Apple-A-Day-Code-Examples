using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayLebenVoiceClips : MonoBehaviour {

    [System.Serializable]
    public struct AudioFolder
    {
        public string audioTag;
        public AudioClip[] potentialClips;
    }

    public AudioFolder[] audioFolders;
	AudioSource audioSource;
    [SerializeField] private AudioSource m_breathing;

    public float timeBetweenVoices;
    bool coolingDown;
    float timer;

	void Start(){
        audioSource = GetComponent<AudioSource>();
	}


    private void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
            if (!coolingDown)
            {
                coolingDown = true;
            }
        }
        else
        {
            if (coolingDown)
                coolingDown = false;
        }
    }

    void StartCoolDown(float time)
    {
        timer = time + timeBetweenVoices;
    }
    public void ChooseVoiceLineFrom(string audioT)
    {
        if (!audioSource.isPlaying && !coolingDown)
        {
            m_breathing.Stop();
            foreach (AudioFolder af in audioFolders)
            {
                if (af.audioTag == audioT && af.potentialClips.Length > 0)
                {
                    AudioClip chosenClip = af.potentialClips[Random.Range(0, af.potentialClips.Length)];
                    audioSource.clip = chosenClip;
                    audioSource.PlayOneShot(chosenClip);
                    StartCoolDown(chosenClip.length);
                    return;
                }
            }
        }
        else if (!audioSource.isPlaying && coolingDown && m_breathing)
        {
            m_breathing.Play();
        }
    }

}
