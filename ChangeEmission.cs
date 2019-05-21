using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeEmission : MonoBehaviour {

    //public List<GameObject> rendObjects;

    public Color unlockedColour = new Color(0,1,0,1);
    public Color lockColour = new Color(1, 0, 0, 1);
    Renderer rend;

    // Use this for initialization
    void Start () {
        //rendObjects.TrimExcess();
        //foreach (GameObject objects in rendObjects)
        //{
        //    rend = GetComponent<Renderer>();
        //}
        rend = GetComponent<Renderer>();
    }


    // Update is called once per frame
    void Update () {
		
	}

    public void LockPanelColor()
    {
        //foreach (GameObject objects in rendObjects)
            rend.material.SetColor("_EmissionColor", lockColour);
    }

    public void UnlockPanelColor()
    {
        //foreach (GameObject objects in rendObjects)
            rend.material.SetColor("_EmissionColor", unlockedColour);
    }
}
