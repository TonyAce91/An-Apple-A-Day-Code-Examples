using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class IncreaseVignette : MonoBehaviour {

    public PostProcessVolume volume;
    Vignette vignette = null;
    Player player;

	// Use this for initialization
	void Start () {
        player = GetComponent<Player>();
        volume.profile.TryGetSettings(out vignette);
    }

    // Update is called once per frame
    void Update () {
        ChangeVignette();
	}

    public void ChangeVignette()
    {
        vignette.intensity.value = player.toxicityRatio;
    }
}
