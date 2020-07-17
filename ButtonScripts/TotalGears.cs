using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TotalGears : MonoBehaviour {

    public TextMeshProUGUI text;

    void OnEnable()
    {
        if (!PlayerPrefs.HasKey("TotalGearsCollected"))
        {
            PlayerPrefs.SetInt("TotalGearsCollected", 0);
        }

        text.text = "TOTAL GEARS: " + PlayerPrefs.GetInt("TotalGearsCollected");
    }
}
