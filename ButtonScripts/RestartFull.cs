using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartFull : MonoBehaviour {

	public void RestartFullScene() {

        GameManager gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        gameManager.PrepareForNewLevel();

        //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        SceneLoader.Instance.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
