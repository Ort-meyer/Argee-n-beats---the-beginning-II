using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrequencyAnalysis : MonoBehaviour
{
    private bool didOnce;
    private AudioSource m_source;
    // Use this for initialization
    void Start()
    {
        didOnce = false;
        AudioSource source = GetComponent<AudioSource>();
        source.clip = Microphone.Start("", false, 10, 44100);

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.G))
        {
            ////GetComponent<AudioSource>().Play();
            //float[] spectrum = new float[256];
            //GetComponent<AudioSource>().GetSpectrumData(spectrum, 0, FFTWindow.Rectangular);
            //
            //float max = 0;
            //for (int i = 1; i < spectrum.Length - 1; i++)
            //{
            //    if (spectrum[i - 1] > max)
            //        max = spectrum[i - 1];
            //
            //
            //
            //    Debug.DrawLine(new Vector3(i - 1, spectrum[i] + 10, 0), new Vector3(i, spectrum[i + 1] + 10, 0), Color.red,5);
            //    Debug.DrawLine(new Vector3(i - 1, Mathf.Log(spectrum[i - 1]) + 10, 2), new Vector3(i, Mathf.Log(spectrum[i]) + 10, 2), Color.cyan,5);
            //    Debug.DrawLine(new Vector3(Mathf.Log(i - 1), spectrum[i - 1] - 10, 1), new Vector3(Mathf.Log(i), spectrum[i] - 10, 1), Color.green,5);
            //    Debug.DrawLine(new Vector3(Mathf.Log(i - 1), Mathf.Log(spectrum[i - 1]), 3), new Vector3(Mathf.Log(i), Mathf.Log(spectrum[i]), 3), Color.blue,5);
            //}
            //print(max);
            if (didOnce == false)
            {
                InvokeRepeating("Check", 0, 1);
                didOnce = true;
            }
        }
    }

    private void Check()
    {
        float[] spectrum = new float[256];
        GetComponent<AudioSource>().GetSpectrumData(spectrum, 0, FFTWindow.Rectangular);
        float max = 0;
        for (int i = 0; i < spectrum.Length; i++)
        {
            if (spectrum[i] > max)
                max = spectrum[i - 1];
        }
        print(max*10000);
    }

}
