using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateTutorial : MonoBehaviour {

    public AnimationClip mouseClip;
    public AnimationClip boxClip;

    public Animator mouseAnim;
    public Animator boxAnim;

    private void Start()
    {
        mouseAnim.Play(mouseClip.name);
        boxAnim.Play(boxClip.name);
    }
}
