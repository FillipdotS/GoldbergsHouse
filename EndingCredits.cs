using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndingCredits : MonoBehaviour {

    public float speedScroll;

    public RectTransform rect;

    public float maxHeight;

	// Use this for initialization
	void Start () {
        rect = GetComponent<RectTransform>();
    }

    private void OnValidate()
    {
        Debug.Log(rect.position.y);
    }

    // Update is called once per frame
    void Update () {

        if (rect.position.y < maxHeight)
        {
            rect.position = new Vector3(0, rect.position.y + speedScroll * Time.deltaTime);
        }
        else
        {
            SceneLoader.Instance.LoadScene(0);
        }
	}
}
