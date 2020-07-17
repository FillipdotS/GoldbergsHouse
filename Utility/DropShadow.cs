using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropShadow : MonoBehaviour {

    Vector2 shadowOffset = new Vector2(0.1f, -0.1f);

    SpriteRenderer srShadow;
    GameObject shadowObj;

    void Awake()
    {
        SpriteRenderer sr = gameObject.GetComponent<SpriteRenderer>();
        if (sr == null)
        {
            Debug.LogError("DropShadow placed on an object without a sprite renderer.");
            return;
        }

        srShadow = sr;

        shadowObj = new GameObject();
        shadowObj.transform.SetParent(transform);
        shadowObj.transform.rotation = transform.rotation; // Need to do this atleast once
        shadowObj.transform.localPosition = shadowOffset;
        shadowObj.transform.localScale = Vector3.one;

        shadowObj.AddComponent<SpriteRenderer>();
        shadowObj.name = "shadow";

        srShadow = shadowObj.GetComponent<SpriteRenderer>();

        srShadow.sortingLayerName = sr.sortingLayerName;
        srShadow.sortingOrder = -50;
        srShadow.color = new Color32(0, 0, 0, 80);

        srShadow.sprite = sr.sprite;
    }

    void LateUpdate()
    {
        shadowObj.transform.position = new Vector2(transform.position.x + shadowOffset.x, transform.position.y + shadowOffset.y);
    }
}
