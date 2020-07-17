using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnToMainMenu : MonoBehaviour {

	public void ReturnToMenu()
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

        SceneManager.LoadScene(0);
    }
}
