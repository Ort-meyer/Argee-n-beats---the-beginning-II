using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundAnalysisNotPlayer : MonoBehaviour {

    public GameObject m_playerObject;
    public float m_currentFrequency = 0;
    public float m_currentAmplitude = 0;
    public float m_maxAmplitude;

	// Use this for initialization
	void Start ()
    {
        m_maxAmplitude = m_playerObject.GetComponent<FrequencyAnalysis>().m_maxAmplitude;	
	}
	
	// Update is called once per frame
	void Update ()
    {
        AudioSource audio = GetComponent<AudioSource>();
        if(Input.GetKeyUp(KeyCode.Y))
        {
            //audio.clip = Instantiate(m_playerObject.GetComponent<FrequencyAnalysis>().m_recordedClip);
            //audio.clip = Instantiate(m_playerObject.GetComponent<AudioSource>().clip);
            audio.clip = m_playerObject.GetComponent<FrequencyAnalysis>().m_recordedClip;
            audio.loop = true;
            audio.Play();


        }

        float[] data = new float[1024];
        float[] amplitudeData = new float[1024];
        audio.GetSpectrumData(data, 0, FFTWindow.Rectangular);
        audio.GetOutputData(amplitudeData, 0);
        AnalyzeSound(data);
        AnalyzeAmplitude(amplitudeData);
    }

    private void AnalyzeSound(float[] data)
    {
        float packageData = 0;
        float highest = 0;
        int highestFreq = 0;
        for (int i = 0; i < data.Length; i++)
        {
            if (data[i] > highest)
            {
                highest = data[i];
                highestFreq = i;
            }
            packageData += System.Math.Abs(data[i]);
        }
        m_currentFrequency = highestFreq * (21000 / 1024);
        //Debug.Log(m_currentFrequency);
    }

    private void AnalyzeAmplitude(float[] data)
    {
        float sum = 0;
        for (int i = 0; i < data.Length; i++)
        {
            sum += System.Math.Abs(data[i]);
        }
        m_currentAmplitude = sum / m_maxAmplitude;
        if (m_currentAmplitude > 1)
            m_currentAmplitude = 1;
        //Debug.Log(m_currentAmplitude);
        //Debug.Log(sum);
    }

}
