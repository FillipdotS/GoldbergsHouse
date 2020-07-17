using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class HighscoreText : MonoBehaviour {

    void Start()
    {
        int highscore = PlayerPrefs.GetInt("Score_" + SceneManager.GetActiveScene().name);
        TextMeshProUGUI text = gameObject.GetComponent<TextMeshProUGUI>();
        text.text = "Highscore: " + highscore.ToString();
    }
}
