using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimitExplosionTime : MonoBehaviour {

    public float timer = 0.1f;

    void Update()
    {
        timer -= Time.deltaTime;
        if (timer < 0)
        {
            PointEffector2D effector = gameObject.GetComponent<PointEffector2D>();
            effector.enabled = false;
        }
    }
}
