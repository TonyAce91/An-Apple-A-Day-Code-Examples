using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextColour : MonoBehaviour {

	public Text text;
	private Color color = Color.white;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		color.r = Mathf.Sin (5*Time.time);
		color.g = Mathf.Sin (10*Time.time);
		color.b = Mathf.Sin (15*Time.time);
		text.color = color;
			
	}
}
