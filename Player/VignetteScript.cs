using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VignetteScript : MonoBehaviour {

    public Color temporaryColor = new Color(0, 1, 0, 1);
    Image image;
    [SerializeField] float flashTimer = 0.5f;
    private float flashTime = 0;

    // Use this for initialization
    void Start () {
        image = GetComponent<Image>();
	}
	
	// Update is called once per frame
	void Update () {
        if (flashTime > 0)
        {
            flashTime -= Time.deltaTime;
            temporaryColor.a = flashTime;
            image.color = temporaryColor;
        }
	}

    public void Flash()
    {
        flashTime = flashTimer;
    }
}
