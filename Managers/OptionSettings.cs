using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;

// Code written by Antoine Kenneth Odi in 2018

public class OptionSettings : MonoBehaviour {


    PostProcessVolume postProcessVolume;
    [SerializeField] private Slider gammaSlider;
    [SerializeField] private Slider contrastSlider;
    [SerializeField] private Slider brightnessSlider;

    ColorGrading colorGradingLayer = null;
    private float gammaContrastRatio;

    // Use this for initialization
    void Start () {
        postProcessVolume = GetComponent<PostProcessVolume>();
        if (!postProcessVolume)
            Debug.Log("Post Processing Volume not found");
        else
            Debug.Log("Post processing found");
        postProcessVolume.profile.TryGetSettings(out colorGradingLayer);
        
        // Set the slider's value to match what is on the current settings
        if (colorGradingLayer)
        {
            if (gammaSlider)
                gammaSlider.value = colorGradingLayer.gamma.value.w;
            if (contrastSlider)
                contrastSlider.value = colorGradingLayer.contrast.value;
            if (brightnessSlider)
            {
                brightnessSlider.value = colorGradingLayer.postExposure.value;
            }
        }


    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public AudioMixer audioMixer;


    public void SetGamma(float gammaValue)
    {
        if (colorGradingLayer)
        {
            // copy what we've got, and add our gamma value
            colorGradingLayer.gamma.value.w = gammaValue;
            //colorGradingLayer.contrast.value = 10* gammaValue - 20;
        }
    }

    public void SetContrast(float contrastValue)
    {
        if (colorGradingLayer)
        {
            // copy what we've got, and add our gamma value
            colorGradingLayer.contrast.value = contrastValue;
        }
    }
    public void SetBrightness(float brightnessValue)
    {
        if (colorGradingLayer)
        {
            // copy what we've got, and add our gamma value
            colorGradingLayer.postExposure.value = brightnessValue;
        }
    }

    public void SetMasterVolume (float volume)
    {
        audioMixer.SetFloat("Master Volume", volume);
    }
    public void SetMusicVolume (float volume)
    {
        audioMixer.SetFloat("Music Volume", volume);
    }
    public void SetSoundEffectsVolume (float volume)
    {
        audioMixer.SetFloat("Sound Effects Volume", volume);
    }
    public void SetSpeechVolume (float volume)
    {
        audioMixer.SetFloat("Speech Volume", volume);
    }

}
