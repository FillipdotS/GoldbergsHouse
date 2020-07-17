using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosive : MonoBehaviour {

    public AudioClip explosionSound;
    public float timer;
    public GameObject explosion;

    void Update()
    {
        timer -= Time.deltaTime;
        if (timer < 0)
        {
            AudioController.Instance.PlaySound(explosionSound);

            Vector3 selfPos = gameObject.transform.position;
            GameObject instantiatedExplosion = Instantiate(explosion, selfPos, Quaternion.identity);
            Destroy(instantiatedExplosion, 5f);
            Destroy(gameObject.transform.parent.gameObject);
        }
    }
}
