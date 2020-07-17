using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleItem : MonoBehaviour {

    public GameObject collectedParticles;

    bool alreadyCollected = false;

    GameManager gameManager;

    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (gameManager.gameBegan)
        {
            if (col.gameObject.tag == "InteractiveObject" && !alreadyCollected)
            {
                // Physics object
                if (col.transform.GetComponent<InteractiveObject>().physicsWillEnable)
                {
                    ItemCollected();
                    alreadyCollected = true;
                }
            }
        }
    }

    void ItemCollected()
    {
        gameManager.AddCollectible();

        ParticleSystem particleSystem = collectedParticles.GetComponent<ParticleSystem>();
        float particleLifetime = particleSystem.main.duration + particleSystem.main.startLifetimeMultiplier;
        GameObject instantiatedParticles = Instantiate(collectedParticles, transform.position, Quaternion.identity);
        Destroy(instantiatedParticles, particleLifetime);

        Destroy(gameObject);
    }
}
