using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : BaseElectrical {

    [Space(10)]
    public bool startRight;

    public float moveAmount = 0.86f;
    public float speed = 5f;

    [Space(10)]
    public Transform platform;
    public SpriteRenderer statusSprite;
    public Sprite onSprite;
    public Sprite offSprite;

    GameManager gameManager;

    private void OnValidate()
    {
        if (startRight)
        {
            platform.localPosition = new Vector3(moveAmount, 0);
        }
        else
        {
            platform.localPosition = new Vector3(-moveAmount, 0);
        }
    }

    private void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();

        if (startRight)
        {
            platform.localPosition = new Vector3(moveAmount, 0);
        }
        else
        {
            platform.localPosition = new Vector3(-moveAmount, 0);
        }
    }

    void MoveLeft()
    {
        if (platform.localPosition.x < moveAmount)
        {
            platform.Translate(new Vector3(speed * Time.deltaTime, 0));
        }
    }

    void MoveRight()
    {
        if (platform.localPosition.x > -moveAmount)
        {
            platform.Translate(new Vector3(-speed * Time.deltaTime, 0));
        }
    }

    public override void OnAction()
    {
        if (gameManager.gameBegan)
        {
            if (startRight)
            {
                MoveRight();
            }
            else
            {
                MoveLeft();
            }
        }

        statusSprite.sprite = onSprite;
    }

    public override void OffAction()
    {
        if (gameManager.gameBegan)
        {
            if (startRight)
            {
                MoveLeft();
            }
            else
            {
                MoveRight();
            }
        }

        statusSprite.sprite = offSprite;
    }
}
