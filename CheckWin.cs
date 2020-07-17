using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckWin : MonoBehaviour {

	public List<WinCondArea> winConds = new List<WinCondArea>();

    GameManager gm;

    void Awake()
    {
        gm = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
    }

    void Update()
    {
        for (int i = 0; i < winConds.Count; i++)
        {
            if (winConds[i].conditionTrue == false)
            {
                return;
            }
        }

        gm.LevelWin();
    }
}
