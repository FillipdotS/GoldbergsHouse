using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveObject : MonoBehaviour {

	public bool draggable;
	public bool physicsWillEnable;

	[HideInInspector]
	public InvItem originItemSlot;
	[HideInInspector]
	public bool placedByPlayer = false;

	// If physicsWillEnable is set to false, this will always be null
	Rigidbody2D rb;

	void Awake() {
		if (gameObject.tag != "InteractiveObject") {
			Debug.LogError (gameObject.name + " (interactive object) has the wrong tag set. This will break certain things.");
		}

		if (physicsWillEnable) {
			rb = gameObject.GetComponent<Rigidbody2D> ();
			rb.isKinematic = true;
		}
	}

	public void BeginGame() {
		if (physicsWillEnable) {
			rb.isKinematic = false;
		}
	}
}
