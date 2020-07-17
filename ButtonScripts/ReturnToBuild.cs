using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnToBuild : MonoBehaviour {

	public void ReturnToBuildMode()
    {
        //GameObject buildModePanel = GameObject.Find("Canvas").transform.Find("BuildModePanel").gameObject;
        AudioController.Instance.audioSource.PlayOneShot(AudioController.Instance.uiClick);

        GameManager gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        gameManager.BackToBuildMode();
    }
}
