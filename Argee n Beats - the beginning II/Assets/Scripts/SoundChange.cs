using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundChange : MonoBehaviour {

    //RenderTexture renderTexture;
    Texture2D texture;
    public float radius;

	// Use this for initialization
	void Start () {
        //renderTexture = new RenderTexture(32, 32, 0, RenderTextureFormat.ARGB32);
        //renderTexture.Create();
        SoundChangeManager.scManager.Register(this);

        texture = new Texture2D(32,32);
        GetComponent<Renderer>().material.mainTexture = texture;
    }
	
	// Update is called once per frame
	void Update () {

    }

    public void ColorByPosition(Vector3 pos)
    {
        for (int x = 0; x < 32; x++)
        {
            for (int y = 0; y < 32; y++)
            {
                int val = Random.Range(0, 10);
                if (val < 5)
                {
                    texture.SetPixel(0, 0, Color.blue);
                }
                else
                {
                    texture.SetPixel(0, 0, Color.red);
                }
            }
        }


        texture.Apply();
    }

    void OnDestroy()
    {
        SoundChangeManager.scManager.Unregister(this);
    }
}
