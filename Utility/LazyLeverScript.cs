using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LazyLeverScript : MonoBehaviour {

	public void EndOfLeverAnim()
    {
        GetComponent<Animator>().SetBool("isFullyDown", true);
    }
}
