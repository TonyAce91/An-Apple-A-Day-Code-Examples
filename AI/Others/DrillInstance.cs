using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Code written by Antoine Kenneth Odi in 2018

public class DrillInstance : MonoBehaviour {

    public GameObject doorSparks;
    public GameObject bloodSprays;
    public GameObject drillEnd;
    [SerializeField] private AudioSource drillSource = null;
    [SerializeField] private List<AudioClip> drillSounds = new List<AudioClip>();

    // Use this for initialization
    void Start () {
        drillSounds.TrimExcess();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void DrillingDoor()
    {
        if (doorSparks != null && drillEnd != null)
        {
            Debug.Log("Sparks meant to show up");
            GameObject sparkInstance = Instantiate(doorSparks);
            sparkInstance.transform.position = drillEnd.transform.position;
            Destroy(sparkInstance, 2);
        }

        if (drillSounds.Count > 0 && drillSource != null)
        {
            Debug.Log("Drill sounds meant to play");
            int drillSoundNumber = Random.Range(1, drillSounds.Count);
            drillSource.PlayOneShot(drillSounds[drillSoundNumber - 1]);
        }
    }

    public void DrillingCharacter()
    {
        if (bloodSprays != null && drillEnd != null)
        {
            GameObject bloodInstance = Instantiate(bloodSprays);
            bloodInstance.transform.position = drillEnd.transform.position;
            Destroy(bloodInstance, 3);
        }
    }
}
