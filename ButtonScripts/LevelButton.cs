using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class LevelButton : MonoBehaviour {

    public string sceneToLoadName;
    public int gearsNeededToStart = 0;

    [Space(10)]
    public TMP_ColorGradient notDoneGradient;
    public TMP_ColorGradient oneScoreGradient;
    public TMP_ColorGradient twoScoreGradient;
    public TMP_ColorGradient threeScoreGradient;

    [HideInInspector]
    public int gearsCollected = 0;

    static TMP_ColorGradient[] grads;

    Button thisButton;
    TextMeshProUGUI num;

    void Awake()
    {
        num = transform.Find("number").GetComponent<TextMeshProUGUI>();

        if (grads == null)
        {
            grads = new TMP_ColorGradient[] { notDoneGradient, oneScoreGradient, twoScoreGradient, threeScoreGradient };
        }

        PlayerPrefs.SetInt("ReqGears_" + sceneToLoadName, gearsNeededToStart);
    }

    void OnEnable()
    {
        // Can't be bothered adding click events through the editor
        thisButton = gameObject.GetComponent<Button>();
        thisButton.onClick.AddListener(LoadSceneFromButton);

        UpdateScore();
    }

    // Called by Start and also when the "Reset Scores" button is used
    public void UpdateScore()
    {
        // If we have the required amount
        if (PlayerPrefs.GetInt("TotalGearsCollected") >= gearsNeededToStart)
        {
            thisButton.interactable = true;

            // Hide all first
            for (int i = 1; i < 4; i++)
            {
                gameObject.transform.Find("cog " + "(" + i.ToString() + ")").gameObject.SetActive(false);
            }

            int score = PlayerPrefs.GetInt("Score_" + sceneToLoadName);

            for (int i = score; i > 0; i--)
            {
                gameObject.transform.Find("cog " + "(" + i.ToString() + ")").gameObject.SetActive(true);
            }

            num.colorGradientPreset = grads[score];
        }
        else
        {
            thisButton.interactable = false;

            transform.Find("NotEnough").Find("Num").GetComponent<TextMeshProUGUI>().text = gearsNeededToStart.ToString();

            transform.Find("NotEnough").gameObject.SetActive(true);
        }
    }

    void LoadSceneFromButton()
    {
        AudioController.Instance.audioSource.PlayOneShot(AudioController.Instance.uiClick);
        SceneManager.LoadScene(sceneToLoadName);
    }
}
