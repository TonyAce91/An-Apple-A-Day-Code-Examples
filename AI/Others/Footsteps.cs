using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Code written by Antoine Kenneth Odi in 2018

public class Footsteps : MonoBehaviour {

    [SerializeField] private AudioSource footsteps = null;
    [SerializeField] private List<AudioClip> footstepSounds = new List<AudioClip>();

	// Use this for initialization
	void Start () {
        footstepSounds.TrimExcess();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    // Animation calls this every step
    public void Footstep()
    {
        if (footstepSounds.Count > 0 && footsteps != null)
        {
            int footstepNumber = Random.Range(1, footstepSounds.Count);
            footsteps.PlayOneShot(footstepSounds[footstepNumber - 1]);
        }
    }

    public void Search()
    {

    }

    public void ArmExtended()
    {

    }
}
