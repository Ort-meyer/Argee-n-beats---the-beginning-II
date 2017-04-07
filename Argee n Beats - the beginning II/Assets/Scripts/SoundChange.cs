using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundChange : MonoBehaviour {

    //RenderTexture renderTexture;
    Texture2D texture;
    public const int bufferSize = 32;
    Color[] colors = new Color[bufferSize * bufferSize];
    public float radius = 1.0f; // Scale for object(TODO could use scale instead if objects default to 1 in size?)

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
        for (int x = 0; x < bufferSize; x++)
        {
            for (int y = 0; y < bufferSize; y++)
            {
                float divVal = (bufferSize / 2.0f) - 0.5f;
                Vector3 pixelWPos = transform.position + new Vector3(((x / divVal) - 1.0f), 0, ((y / divVal) - 1.0f)) * radius; // -15 ->
                float length = (pos - pixelWPos).magnitude;

                // effective should come from manager
                float effectiveRange = 5;

                // This should deal with only updating upwards as well
                if (length < effectiveRange)
                {
                    // Check value
                    colors[x + bufferSize * y] = new Color(1,1,1);
                }
            }
        }

        texture.SetPixels(colors);

        texture.Apply();
    }

    void OnDestroy()
    {
        SoundChangeManager.scManager.Unregister(this);
    }
}
