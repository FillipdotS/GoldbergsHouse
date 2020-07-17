using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableOnStart : MonoBehaviour {

    public GameObject[] willDisable;

    void Awake()
    {
        GameManager gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        gameManager.objectsToEnable.Add(gameObject); // Add ourselves to objectsToEnable

        // Disable this game object. The game manager will enable us later
        gameObject.SetActive(false);
    }

    void Update()
    {
        for (int i = 0; i < willDisable.Length; i++)
        {
            willDisable[i].SetActive(false);
        }
        this.enabled = false;
    }
}
