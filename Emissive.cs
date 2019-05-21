using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Emissive : MonoBehaviour {

    Color currentColor;
    float brightness;
    public float speed = 10f;
    float offset = 0.2f;
    private Material matInstance;

	// Use this for initialization
	void Start () {
        offset = Random.Range(0, 100)/100f;
        matInstance = new Material(GetComponent<Renderer>().material);
        gameObject.GetComponent<Renderer>().material = matInstance;
	}
	
	// Update is called once per frame
	void Update () {
        brightness = Mathf.Sin(Time.time * speed) * 1.6f + offset;
        currentColor = new Color(brightness, brightness, brightness);


        matInstance.SetColor("_EmissionColor", currentColor);

    }
}
