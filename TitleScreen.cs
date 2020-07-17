using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TitleScreen : MonoBehaviour {

    public string levelTitle;

    TextMeshProUGUI title;
    GameObject tutorialToEnable;

    private void Awake()
    {
        title = transform.Find("Title").GetComponent<TextMeshProUGUI>();
        title.text = '"' + levelTitle + '"';

        if (GameObject.Find("Tutorial"))
        {
            tutorialToEnable = GameObject.Find("Tutorial");
            tutorialToEnable.SetActive(false);
        }
    }

    public void OnEndAnimation()
    {
        if (tutorialToEnable != null)
        {
            tutorialToEnable.SetActive(true);
        }

        if (transform.parent.Find("DialoguePanel"))
        {
            transform.parent.Find("DialoguePanel").gameObject.SetActive(true);
        }

        Destroy(gameObject);
    }
}
