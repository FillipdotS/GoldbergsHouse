using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    // Script used in the main menu. Different buttons call different methods.

    public AudioClip clickSound;
    [Space(10)]

    // Different panels, set active/inactive by this script
    public GameObject MainMenuPanel;
    public GameObject OptionsPanel;
    public GameObject LevelsPanel;
    [Space(10)]

    public MenuBackgroundSwitch backSwitch;
    public GameObject resetConfirmationPanel;
    public GameObject notEnoughPanel;

    void Start()
    {
        MusicController.Instance.PlayMainMusic();

        // Read any "messages"
        GameObject msg = GameObject.Find("menumsg_showLevels");
        if (msg != null)
        {
            OpenLevels();
            Destroy(msg);
        }

        msg = GameObject.Find("menumsg_notEnough");
        if (msg != null)
        {
            OpenLevels();
            ShowNotEnoughGears();
            Destroy(msg);
        }
    }

    public void OpenLevels()
    {
        backSwitch.ShowEmpty();

        AudioController.Instance.audioSource.PlayOneShot(clickSound);
        MainMenuPanel.SetActive(false);
        LevelsPanel.SetActive(true);
    }

    public void OpenOptions()
    {
        backSwitch.ShowEmpty();

        AudioController.Instance.audioSource.PlayOneShot(clickSound);
        MainMenuPanel.SetActive(false);
        OptionsPanel.SetActive(true);
    }

    public void BackToMainMenu()
    {
        backSwitch.ShowMain();

        AudioController.Instance.audioSource.PlayOneShot(clickSound);
        OptionsPanel.SetActive(false);
        LevelsPanel.SetActive(false);

        MainMenuPanel.SetActive(true);
    }

    public void QuitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    public void ShowNotEnoughGears()
    {
        notEnoughPanel.SetActive(true);
    }

    public void HideNotEnoughGears()
    {
        notEnoughPanel.SetActive(false);
    }

    public void ShowResetScore()
    {
        resetConfirmationPanel.SetActive(true);
    }

    public void HideResetScore()
    {
        resetConfirmationPanel.SetActive(false);
    }

    public void ResetAllScores()
    {
        AudioController.Instance.audioSource.PlayOneShot(clickSound);
        for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            // Messy, but easy to understand
            string scenePath = SceneUtility.GetScenePathByBuildIndex(i);
            var sceneNameStart = scenePath.LastIndexOf("/", System.StringComparison.Ordinal) + 1;
            var sceneNameEnd = scenePath.LastIndexOf(".", System.StringComparison.Ordinal);
            var sceneNameLength = sceneNameEnd - sceneNameStart;
            string xyz = scenePath.Substring(sceneNameStart, sceneNameLength);

            //Debug.Log(xyz);
            PlayerPrefs.SetInt("Score_" + xyz, 0);
        }

        PlayerPrefs.SetInt("TotalGearsCollected", 0);

        HideResetScore();
    }
}
