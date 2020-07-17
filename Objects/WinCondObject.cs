using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinCondObject : MonoBehaviour {

    public SpriteRenderer colorSprite;

    public WinCondArea.WinColors myColor;
    public byte colorAlpha = 130;

    void OnValidate()
    {
        colorSprite.color = GetColorFromEnum(myColor);
    }

    void Awake()
    {
        colorSprite.color = GetColorFromEnum(myColor);
    }

    public Color32 GetColorFromEnum(WinCondArea.WinColors enumType)
    {
        if (enumType == WinCondArea.WinColors.red)
        {
            return new Color32(255, 0, 0, colorAlpha);
        }
        else if (enumType == WinCondArea.WinColors.blue)
        {
            return new Color32(0, 0, 255, colorAlpha);
        }
        else if (enumType == WinCondArea.WinColors.green)
        {
            return new Color32(0, 255, 0, colorAlpha);
        }

        return new Color32(100, 100, 100, colorAlpha);
    }
}
