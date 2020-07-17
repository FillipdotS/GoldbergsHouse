using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModifyMethods : MonoBehaviour {

    public AudioClip moveSound;

	public int DegreesToRotate = 60;
    [HideInInspector]
    public GameObject selectedObject;

    public void RotateSelectedRight()
    {
        AudioController.Instance.audioSource.PlayOneShot(moveSound);
        selectedObject.transform.Rotate(Vector3.forward * -DegreesToRotate);
    }

    public void RotateSelectedLeft()
    {
        AudioController.Instance.audioSource.PlayOneShot(moveSound);
        selectedObject.transform.Rotate(Vector3.forward * DegreesToRotate);
    }
}
