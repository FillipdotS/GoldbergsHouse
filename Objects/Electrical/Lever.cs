using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : MonoBehaviour {

    [Space(10)]
    public AudioClip leverDownSound;

    //public BaseElectrical leverConnectionPoint;
    public ConnectionPoint conPointOne;
    public ConnectionPoint conPointTwo;

    public Animator leverAnimator;

    bool leverDown = false;
    GameManager gameManager;

    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (!gameManager.gameBegan)
        {
            return;
        }

        if (col.GetComponent<Rigidbody2D>())
        {
            leverDown = true;

            AudioController.Instance.PlaySound(leverDownSound);

            // Play animation
            leverAnimator.SetBool("isDown", true);
        }
    }

    void Update()
    {
        if (leverDown)
        {
            Debug.Log(conPointOne.isTransferringPower + " " + conPointTwo.isTransferringPower);

            if (conPointOne.GetWireStatus())
            {
                conPointTwo.isTransferringPower = true;
            }

            if (conPointTwo.GetWireStatus())
            {
                conPointOne.isTransferringPower = true;
            }
        }
    }
}
