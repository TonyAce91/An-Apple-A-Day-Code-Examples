using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomClipPlayer : MonoBehaviour {

	[SerializeField] private List<AudioClip> scarabDeathclips;
	[SerializeField] private List<AudioClip> scarabAttackclips;
	[SerializeField] private List<AudioClip> scarabStunclips;

    AudioSource audioSource;

    void Start()
    {
        scarabDeathclips.TrimExcess();
        scarabAttackclips.TrimExcess();
        scarabStunclips.TrimExcess();
        audioSource = GetComponent<AudioSource>();
    }

        public void PlayRandomDeathClip()
	{
        if (!audioSource.isPlaying)
        {
            int index = Random.Range(0, scarabDeathclips.Count - 1);
            audioSource.PlayOneShot(scarabDeathclips[index]);
        }
	}

	public void PlayRandomAttackClip()
	{
        if (!audioSource.isPlaying)
        {
            int index = Random.Range(0, scarabAttackclips.Count - 1);
            audioSource.PlayOneShot(scarabAttackclips[index]);
        }
	}

	public void PlayRandomStunClip()
	{
        if (!audioSource.isPlaying)
        {
            int index = Random.Range(0, scarabStunclips.Count - 1);
            audioSource.PlayOneShot(scarabStunclips[index]);
        }
	}


}
