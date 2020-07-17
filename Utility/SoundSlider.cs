using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundSlider : MonoBehaviour {

    // "MusicVolume" - player pref for music volume
    // "EffectVolume" - player pref for fx volume

    // true -> set initial value to that of MusicVolume pref
    public bool isMusicSlider;

    Slider slider;
   
    void Awake()
    {
        slider = gameObject.GetComponent<Slider>();

        if (isMusicSlider)
        {
            slider.value = PlayerPrefs.GetFloat("MusicVolume");
            Debug.Log(slider.value = PlayerPrefs.GetFloat("MusicVolume"));
        }
        else
        {
            slider.value = PlayerPrefs.GetFloat("EffectVolume");
        }
    }

    public void UpdateMusicVolume()
    {
        float value = slider.value;
        MusicController.Instance.audioSource.volume = value;
        PlayerPrefs.SetFloat("MusicVolume", value);
    }

    public void UpdateEffectVolume()
    {
        float value = slider.value;
        AudioController.Instance.audioSource.volume = value;
        PlayerPrefs.SetFloat("EffectVolume", value);
    }
}
