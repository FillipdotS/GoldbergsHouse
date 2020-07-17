using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileShadows : MonoBehaviour {

    TilemapRenderer srShadow;
    GameObject shadowObj;

    static bool alreadyCreatedShadows = false;

    void Start()
    {
        if (alreadyCreatedShadows == true)
        {
            alreadyCreatedShadows = false;
            return;
        }

        alreadyCreatedShadows = true;

        TilemapRenderer tmr = gameObject.GetComponent<TilemapRenderer>();
        Tilemap tm = gameObject.GetComponent<Tilemap>();
        if (tmr == null)
        {
            return;
        }

        shadowObj = Instantiate(gameObject, transform.parent);
        Destroy(shadowObj.GetComponent<TilemapCollider2D>());
        Destroy(shadowObj.GetComponent<CompositeCollider2D>());
        Destroy(shadowObj.GetComponent<Rigidbody2D>());
        shadowObj.transform.SetParent(transform.parent);

        shadowObj.name = "shadow";

        srShadow = shadowObj.GetComponent<TilemapRenderer>();
        Tilemap srTilemap = shadowObj.GetComponent<Tilemap>();

        srTilemap.tileAnchor = new Vector3(0.6f, 0.4f);

        srShadow.sortingLayerName = tmr.sortingLayerName;
        srShadow.sortingOrder = tmr.sortingOrder - 1;
        srTilemap.color = new Color32(0, 0, 0, 120);
    }
}
