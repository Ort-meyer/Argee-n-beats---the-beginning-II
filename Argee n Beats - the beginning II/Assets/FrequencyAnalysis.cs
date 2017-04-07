using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrequencyAnalysis : MonoBehaviour
{
    public GameObject m_source1;
    public GameObject m_source2;

    private float windowDuration = 0.2f;

    bool recording1;

    float threshold;

    // Use this for initialization
    void Start()
    {
        recording1 = true;
        m_source1.GetComponent<AudioSource>().clip = Microphone.Start("", false, 1, 44100);
        Invoke("Sample1", 2);
    }

    // Update is called once per frame
    void Update()
    {
        float[] data = new float[1024];
        if (recording1 == false)
        {
            m_source1.GetComponent<AudioSource>().GetSpectrumData(data, 0, FFTWindow.Rectangular);
            AnalyzeSound(data);
        }
        else
        {
            m_source2.GetComponent<AudioSource>().GetSpectrumData(data, 0, FFTWindow.Rectangular);
            AnalyzeSound(data);
        }

    }

    private void Sample1()
    {
        recording1 = false;
        m_source2.GetComponent<AudioSource>().clip = Microphone.Start("", false, 1, 44100);
        m_source1.GetComponent<AudioSource>().Play();
        Invoke("Sample2", windowDuration);
    }

    private void Sample2()
    {
        recording1 = true;
        m_source1.GetComponent<AudioSource>().clip = Microphone.Start("", false, 1, 44100);
        m_source2.GetComponent<AudioSource>().Play();
        Invoke("Sample1", windowDuration);
    }

    private void AnalyzeSound(float[] data)
    {
        float packageData = 0;
        float highest = 0;
        int highestFreq = 0;
        for (int i = 0; i < data.Length; i++)
        {
            if(data[i] > highest)
            {
                highest = data[i];
                highestFreq = i;
            }
            packageData += System.Math.Abs(data[i]);
        }
        Debug.Log(highestFreq * (21000/1024));
    }
}
