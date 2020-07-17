using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuBackgroundSwitch : MonoBehaviour {

    public Texture emptyTexture;
    Texture mainTexture;

    RawImage ri;

	void Awake () {
        ri = GetComponent<RawImage>();
        mainTexture = ri.texture;
	}
	
	public void ShowMain()
    {
        ri.texture = mainTexture;
    }

    public void ShowEmpty()
    {
        ri.texture = emptyTexture;
    }
}
