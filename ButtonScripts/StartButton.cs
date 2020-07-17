using UnityEngine;

public class StartButton : MonoBehaviour {

    // Used by the button which starts the game

    GameManager gameManager;
    GameObject playModePanel;

	void Start () {
        gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        playModePanel = GameObject.Find("Canvas").transform.Find("PlayModePanel").gameObject;
    }
	
	public void StartGame()
    {
        AudioController.Instance.audioSource.PlayOneShot(AudioController.Instance.uiClick);

        gameManager.StartGame();
        playModePanel.SetActive(true);
        gameObject.transform.parent.gameObject.SetActive(false);
    }
}
