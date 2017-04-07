using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundChange : MonoBehaviour {

    RenderTexture renderTexture;
    Texture2D texture;

	// Use this for initialization
	void Start () {
        //renderTexture = new RenderTexture(32, 32, 0, RenderTextureFormat.ARGB32);
        //renderTexture.Create();
        texture = new Texture2D(32,32);
        GetComponent<Renderer>().material.mainTexture = texture;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
