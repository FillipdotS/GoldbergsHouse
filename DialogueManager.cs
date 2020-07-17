using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour {

    // Very basic and hard to expand, but we don't need anything complicated
    // ¯\_(ツ)_/¯

    [TextArea(3, 10)]
    public string[] sentences;

    int currentSentence = 0;

    TextMeshProUGUI text;
    GameObject tutorialToEnable;

    void Awake()
    {
        text = transform.Find("Dialogue").Find("Text").gameObject.GetComponent<TextMeshProUGUI>();
        text.text = sentences[currentSentence];

        if (GameObject.Find("Tutorial"))
        {
            tutorialToEnable = GameObject.Find("Tutorial");
            tutorialToEnable.SetActive(false);
        }
    }

    public void NextSentence()
    {
        currentSentence++;
        //Debug.Log(currentSentence + " " + sentences.Length);

        if (currentSentence < sentences.Length)
        {
            text.text = sentences[currentSentence];
        }
        else
        {
            FinishDialogue();
        }
    }

    void FinishDialogue()
    {
        if (tutorialToEnable != null)
        {
            tutorialToEnable.SetActive(true);
        }

        Destroy(gameObject);
    }
}
