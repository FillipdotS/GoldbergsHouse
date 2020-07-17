using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class LevelText : MonoBehaviour {

    public TextMeshProUGUI text;

	void Start () {
        int levelIndex = SceneManager.GetActiveScene().buildIndex;
        text.text = "Level " + levelIndex;
	}
}
