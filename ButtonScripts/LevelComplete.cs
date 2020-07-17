using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelComplete : MonoBehaviour {

    // This script's methods are called by the different buttons
    // in the level complete panel.

    public AudioClip levelEndSound;
    
    List<GameObject> collectibleProgress = new List<GameObject>();

    void Awake()
    {
        AudioController.Instance.PlaySound(levelEndSound);

        // We place this in Awake() as otherwise there is a risk ShowAcquiredCollectibles()
        // will be called before Start(), meaning collectibleProgress will be empty
        foreach (Transform child in transform.Find("CollectiblesParent"))
        {
            collectibleProgress.Add(child.gameObject);
        }
    }

    public void ReturnToMainMenu()
    {
        AudioController.Instance.audioSource.PlayOneShot(AudioController.Instance.uiClick);

        GameManager gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        gameManager.PrepareForNewLevel();

        // "Send" a message to the menu
        GameObject ourMsg = new GameObject
        {
            name = "menumsg_showLevels"
        };
        DontDestroyOnLoad(ourMsg);

        // Assuming main menu is scene index 0, which it should be
        SceneManager.LoadScene(0);
    }

    public void NextLevel()
    {
        AudioController.Instance.audioSource.PlayOneShot(AudioController.Instance.uiClick);

        GameManager gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        gameManager.PrepareForNewLevel();

        int total = PlayerPrefs.GetInt("TotalGearsCollected");

        int nextLevelBI = SceneManager.GetActiveScene().buildIndex + 1;

        string scenePath = SceneUtility.GetScenePathByBuildIndex(nextLevelBI);
        var sceneNameStart = scenePath.LastIndexOf("/", System.StringComparison.Ordinal) + 1;
        var sceneNameEnd = scenePath.LastIndexOf(".", System.StringComparison.Ordinal);
        var sceneNameLength = sceneNameEnd - sceneNameStart;
        string xyz = scenePath.Substring(sceneNameStart, sceneNameLength);

        int nextReq = PlayerPrefs.GetInt("ReqGears_" + xyz);

        if (total >= nextReq)
        {
            // Load next scene
            SceneLoader.Instance.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        else
        {
            // "Send" a message to the menu
            GameObject ourMsg = new GameObject
            {
                name = "menumsg_notEnough"
            };
            DontDestroyOnLoad(ourMsg);

            SceneManager.LoadScene(0);
        }
    }

    public void ShowAcquiredCollectibles(int collectedAmount, int oldScore)
    {
        for (int i = collectedAmount; i > 0; i--)
        {
            collectibleProgress[i - 1].SetActive(true);
        }

        // Highscore text
        if (collectedAmount > oldScore)
        {
            transform.Find("NewHighScore").gameObject.SetActive(true);
        }
    }
}
