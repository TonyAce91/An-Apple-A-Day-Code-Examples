using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayScarabVoiceClips : MonoBehaviour {

    [System.Serializable]
    public struct AudioFolder
    {
        public string audioTag;
        public AudioClip[] potentialClips;
    }

    //[SerializeField] private AudioSource m_walking;

    public AudioFolder[] audioFolders;
	AudioSource audioSource;

    public float timeBetweenVoices;
    bool coolingDown;
    float timer;

    void Start(){
        audioSource = GetComponent<AudioSource>();
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


    private void Update()
    {
        
    }

    void StartCoolDown(float time)
    {
        timer = time + timeBetweenVoices;
    }
    public void ChooseVoiceLineFrom(string audioT)
    {
        if (!audioSource.isPlaying && !coolingDown)
        {
            //m_walking.Stop();
            foreach (AudioFolder af in audioFolders)
            {
                if (af.audioTag == audioT && af.potentialClips.Length > 0)
                {
                    AudioClip chosenClip = af.potentialClips[Random.Range(0, af.potentialClips.Length)];
                    audioSource.clip = chosenClip;
                    audioSource.PlayOneShot(chosenClip);
                    StartCoolDown(chosenClip.length);
                    //Debug.Log("New scarab noise playing");
                    return;
                }
            }
        }
        //else if (!audioSource.isPlaying && m_walking)
        //{
        //    m_walking.Play();
        //}
    }

}
