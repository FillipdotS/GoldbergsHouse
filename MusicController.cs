using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour {

    /* 
    Main menu script calls PlayMainMusic
    GameManager will call also call PlayMainMusic when it loads, and during
    build/play mode changes it will change the music
    */

    public AudioClip mainMusic;

    public AudioSource audioSource;

    // Static singleton instance
    private static MusicController instance;

    // Static singleton property
    public static MusicController Instance
    {
        // Here we use the ?? operator, to return 'instance' if 'instance' does not equal null
        // otherwise we assign instance to a new component and return that
        get { return instance ?? (instance = CreateSingleton()); }
    }

    private static MusicController CreateSingleton()
    {
        MusicController newInstance;

        GameObject newObj = Instantiate(Resources.Load("MusicController")) as GameObject;
        newInstance = newObj.GetComponent<MusicController>();
        DontDestroyOnLoad(newInstance.gameObject);
        return newInstance;
    }

    void Awake()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
        if (PlayerPrefs.HasKey("MusicVolume"))
        {
            audioSource.volume = PlayerPrefs.GetFloat("MusicVolume");
        }
        else
        {
            PlayerPrefs.SetFloat("MusicVolume", 0.75f);
            audioSource.volume = 0.75f;
        }
    }

    public void PlayMainMusic()
    {
        if (audioSource.clip != mainMusic)
        {
            audioSource.Stop();
            audioSource.clip = mainMusic;
            audioSource.Play();
        }
    }
}
