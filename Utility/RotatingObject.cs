using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingObject : MonoBehaviour {

    public float rotationSpeed = 100;

    private static int startingRotation = 0;

	void Start () {
        gameObject.transform.Rotate(Vector3.forward * startingRotation);
        startingRotation += 90;
    }

	void Update () {
        gameObject.transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
    }
}
