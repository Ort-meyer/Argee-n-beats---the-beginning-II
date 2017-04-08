using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundChange : MonoBehaviour {

    //RenderTexture renderTexture;
    Texture2D texture;
    public int bufferSize = 32;
    Color[] colors;
    public float radius = 0.5f; // Scale for object(TODO could use scale instead if objects default to 1 in size?)

	// Use this for initialization
	void Start () {
        //renderTexture = new RenderTexture(32, 32, 0, RenderTextureFormat.ARGB32);
        //renderTexture.Create();
        colors = new Color[bufferSize * bufferSize];
        SoundChangeManager.scManager.Register(this);

        texture = new Texture2D(bufferSize, bufferSize);
        Renderer rend = GetComponent<Renderer>();
        //rend.material.mainTexture = texture;
        rend.material.SetTexture("_SoundMap", texture);
        rend.material.SetVector("_SoundMapPosition", new Vector4(transform.position.x, transform.position.y, transform.position.z, 0.0f));
        rend.material.SetFloat("_SoundMapWidth", radius);

        for (int x = 0; x < bufferSize; x++)
        {
            for (int z = 0; z < bufferSize; z++)
            {
                // Check value
                colors[x + bufferSize * z] = new Color(0, 0, 0);
            }
        }

        texture.SetPixels(colors);
        texture.Apply();
    }
	
	// Update is called once per frame
	void Update () {

    }

    public void ColorByPositionFull(Vector3 pos, float minRange, float maxRange)
    {
        float range = maxRange - minRange;

        for (int x = 0; x < bufferSize; x++)
        {
            for (int z = 0; z < bufferSize; z++)
            {
                float divVal = (bufferSize / 2.0f) - 0.5f;
                Vector3 pixelWPos = transform.position + new Vector3(((x / divVal) - 1.0f), 0, ((z / divVal) - 1.0f)) * radius; // -15 ->
                float length = (pos - pixelWPos).magnitude;


                // This should deal with only updating upwards as well
                if (length < maxRange)
                {

                    float val = Mathf.Min(((maxRange - length) / range), 1.0f); // 1 -> 0, 

                    // Check value
                    if (val > colors[x + bufferSize * z].r)
                    {
                        colors[x + bufferSize * z] = new Color(val, val, val);
                    }
                }
            }
        }

        texture.SetPixels(colors);

        texture.Apply();
    }

    public void ColorByPositionSelected(Vector3 pos, float minRange, float maxRange)
    {
        float range = maxRange - minRange;

        bool shouldApply = false;
        for (int x = 0; x < bufferSize; x++)
        {
            for (int z = 0; z < bufferSize; z++)
            {
                float divVal = (bufferSize / 2.0f) - 0.5f;
                Vector3 pixelWPos = transform.position + new Vector3(((x / divVal) - 1.0f), 0, ((z / divVal) - 1.0f)) * radius; // -15 ->
                float length = (pos - pixelWPos).magnitude;

                // This should deal with only updating upwards as well
                if (length < maxRange)
                {

                    float val = Mathf.Min(((maxRange - length) / range), 1.0f); // 1 -> 0, 

                    // Check value
                    if (val > colors[x + bufferSize * z].r)
                    {
                        colors[x + bufferSize * z] = new Color(val, val, val);
                        texture.SetPixel(x, z, colors[x + bufferSize * z]);
                        shouldApply = true;
                    }
                }
            }
        }

        if (shouldApply)
        {

            texture.Apply();
        }
    }


    void OnDestroy()
    {
        SoundChangeManager.scManager.Unregister(this);
    }
}
