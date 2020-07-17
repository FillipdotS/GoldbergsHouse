using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinCondArea : MonoBehaviour {

    [HideInInspector]
    public bool conditionTrue;

    public int amountNeededToWin = 1;

    int amountInside = 0;
    SpriteRenderer sr;
    GameObject currentCorrectObj;

	public enum WinColors
    {
        uncolored,
        red,
        blue,
        green
    }

    public WinColors myColor;

    void OnValidate()
    {
        sr = transform.Find("coloring_tile").GetComponent<SpriteRenderer>();
        sr.color = GetColorFromEnum(myColor);
    }

    void Awake()
    {
        sr = transform.Find("coloring_tile").GetComponent<SpriteRenderer>();
        sr.color = GetColorFromEnum(myColor);

        CheckWin wincontr = GameObject.FindGameObjectWithTag("WinController").GetComponent<CheckWin>();
        wincontr.winConds.Add(this);
    }

    public Color32 GetColorFromEnum(WinColors enumType)
    {
        if (enumType == WinColors.red)
        {
            return new Color32(255, 0, 0, 130);
        }
        else if (enumType == WinColors.blue)
        {
            return new Color32(0, 0, 255, 130);
        }
        else if (enumType == WinColors.green)
        {
            return new Color32(0, 255, 0, 130);
        }

        return new Color32(100, 100, 100, 130);
    }

    bool CheckCondition()
    {
        if (amountInside >= amountNeededToWin)
        {
            return true;
        }
        return false;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        WinCondObject wco = other.GetComponent<WinCondObject>();
        if (wco != null)
        {
            if (wco.myColor == myColor)
            {
                amountInside++;
                conditionTrue = CheckCondition();
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        WinCondObject wco = other.GetComponent<WinCondObject>();
        if (wco != null)
        {
            if (wco.myColor == myColor)
            {
                amountInside--;
                conditionTrue = CheckCondition();
            }
        }
    }
}
