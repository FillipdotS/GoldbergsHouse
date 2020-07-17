using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lamp : BaseElectrical {

    public Sprite onSprite;
    public Sprite offSprite;

    SpriteRenderer sr;
    GameObject lightObject;

    public override void Init()
    {
        base.Init();
        lightObject = transform.parent.Find("Point light").gameObject;
        sr = transform.parent.GetComponent<SpriteRenderer>();
    }

    public override void OnAction()
    {
        lightObject.SetActive(true);
        sr.sprite = onSprite;
    }

    public override void OffAction()
    {
        lightObject.SetActive(false);
        sr.sprite = offSprite;
    }
}
