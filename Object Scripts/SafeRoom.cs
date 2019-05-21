using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafeRoom : MonoBehaviour {

    [SerializeField] private List<DoorSystem> securityDoors = new List<DoorSystem>();
    [SerializeField] private float safeTimer = 5f;
    private float safeTime = 0;

	// Use this for initialization
	void Start () {
        safeTime = safeTimer;
        securityDoors.TrimExcess();
    }

    // Update is called once per frame
    void Update () {
        if (safeTime <= 0 && securityDoors.Count > 0)
        {
            foreach (DoorSystem door in securityDoors)
                door.LockedDoors = true;
            Debug.Log("Security doors have been secured");
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player" && safeTime > 0)
            safeTime -= Time.deltaTime;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            safeTime = safeTimer;
            if (securityDoors.Count > 0)
                foreach (DoorSystem door in securityDoors)
                    door.LockedDoors = false;
        }
    }
}
