using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableOnBegin : MonoBehaviour {

    void Awake()
    {
        GameManager gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        gameManager.objectsToEnable.Add(gameObject); // Add ourselves to objectsToEnable

        // Disable this game object. The game manager will enable us later
        gameObject.SetActive(false);
    }
}
