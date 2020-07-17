using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spring : MonoBehaviour {

    public AudioClip springSound;

	void OnCollisionEnter2D(Collision2D col) {
        AudioController.Instance.audioSource.PlayOneShot(springSound);
    }
}
