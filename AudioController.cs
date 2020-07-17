using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour {

    /* 
    Main menu script calls PlayMainMusic
    GameManager will call also call PlayMainMusic when it loads, and during
    build/play mode changes it will change the music
    */

    public AudioSource audioSource;

    public AudioClip uiClick;

    public static AudioController Instance
    {
        get { return instance ?? (instance = CreateSingleton()); }
    }

    private static AudioController instance;

    private static AudioController CreateSingleton()
    {
        AudioController newInstance;

        GameObject newObj = Instantiate(Resources.Load("AudioController")) as GameObject;
        newInstance = newObj.GetComponent<AudioController>();
        DontDestroyOnLoad(newInstance.gameObject);
        return newInstance;
    }

    void Awake()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
        if (PlayerPrefs.HasKey("EffectVolume"))
        {
            audioSource.volume = PlayerPrefs.GetFloat("EffectVolume");
        }
        else
        {
            PlayerPrefs.SetFloat("EffectVolume", 0.75f);
            audioSource.volume = 0.75f;
        }
    }

    public void PlaySound(AudioClip clipToPlay)
    {
        audioSource.PlayOneShot(clipToPlay);
    }
}
