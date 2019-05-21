using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Code made by Georgia Bryant and Antoine Kenneth Odi in 2018

public class CollectAntidote : MonoBehaviour {

    public List<Image>needleImages;
    public int numberOfNeedles;
    private Player player;
    public Text antidoteText;
    public float textDuration = 1f;
    float textTimer = 1f;


	// Use this for initialization
	void Start () {
        needleImages.TrimExcess();
        antidoteText.text = "";
        foreach (Image needle in needleImages)
        {
            Color needleColour = needle.color;
            needleColour.a = 0;
            needle.color = needleColour;
        }
        player = GetComponent<Player>();
	}
	
	// Update is called once per frame
	void Update () {
        UseAntidote();
        if (textTimer > 0)
        {
            textTimer -= Time.deltaTime;
        }
        else
            antidoteText.text = "";
    }

    public void UIUpdate()
    {
        Color needleColour = needleImages[numberOfNeedles].color;
        needleColour.a = 255;
        needleImages[numberOfNeedles].color = needleColour;
        numberOfNeedles++;
    }

    void OnTriggerEnter(Collider other)
    {
        //if (other.gameObject.tag == "Antidote" && numberOfNeedles < needleImages.Count)
        //{
        //    Color needleColour = needleImages[numberOfNeedles].color;
        //    needleColour.a = 255;
        //    needleImages[numberOfNeedles].color = needleColour;
        //    numberOfNeedles++;
        //    Destroy(other.gameObject);
        //}
        
    }

    void UseAntidote()
    {
        if (Input.GetButtonDown("Use Antidote"))
        {
            textTimer = textDuration;

            if (numberOfNeedles > 0)
            {
                if (player.CurrentToxicity > 0f)
                {
                    numberOfNeedles--;
                    Color needleColour = needleImages[numberOfNeedles].color;
                    needleColour.a = 0;
                    needleImages[numberOfNeedles].color = needleColour;
                    player.TakeDamage(-100);
                    return;
                }

                if (antidoteText != null)
                {
                    antidoteText.text = "No traces of toxins found";
                    textTimer = textDuration;
                }
            }
            else
            {
                if (antidoteText != null)
                {
                    antidoteText.text = "No antidote to use";
                    textTimer = textDuration;
                }
            }
        }
        //else if (Input.GetButtonDown("Use Antidote") && numberOfNeedles > 0 && player.CurrentToxicity <= 0f)
        //{
        //    if (antidoteText != null)
        //    {
        //        antidoteText.text = "No toxins detected";
        //    }
        //}

        //else if (Input.GetButtonDown("Use Antidote") && numberOfNeedles == 0)
        //{
        //    if (antidoteText != null)
        //    {
        //        if (textTimer > 0)
        //        {
        //            antidoteText.text = "No Health Kits left";
        //            textTimer -= Time.deltaTime / textDuration;   
        //        }
        //        if (textTimer <= 0)
        //        {
        //            antidoteText.text = "";
        //        }
        //    }
            
        //}
        //antidoteText.text = "";
    }
}

