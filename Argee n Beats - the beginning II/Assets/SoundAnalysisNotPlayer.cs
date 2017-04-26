using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundAnalysisNotPlayer : MonoBehaviour
{

    public GameObject m_playerObject;
    public float m_currentFrequency = 0;
    public float m_currentAmplitude = 0;
    private float m_amplitudeThreshold; // set by player script
    public float m_maxAmplitude;

    private float[] m_amplitudeValues = new float[m_framesToAvrage];
    private float[] m_freqValues = new float[m_framesToAvrage];

    const int m_framesToAvrage = 10;
    int m_frameIter = 0;

    AudioSource audioData;
    AudioSource audioOutput;
    // Use this for initialization
    void Start()
    {
        m_playerObject = GameObject.FindGameObjectWithTag("Player"); // TODO change if we have multiplayer :D
        FrequencyAnalysis freAn = m_playerObject.GetComponent<FrequencyAnalysis>();
        m_amplitudeThreshold = freAn.m_amplitudeThreshold;
        m_maxAmplitude = freAn.m_maxAmplitude;
        AudioSource[] audioS = GetComponents<AudioSource>();

        if (audioS[0].enabled)
        {
            audioOutput = audioS[0];
            audioData = audioS[1];
            audioData.enabled = true;
        }
        else
        {
            audioData = audioS[1];
            audioOutput = audioS[0];
            audioData.enabled = true;
        }

    }

    // Update is called once per frame
    void Update()
    {
        m_frameIter++;
        if (m_frameIter >= m_framesToAvrage)
            m_frameIter = 0;

        FrequencyAnalysis freAn = m_playerObject.GetComponent<FrequencyAnalysis>();
        m_amplitudeThreshold = freAn.m_amplitudeThreshold;

        float[] data = new float[1024];
        float[] amplitudeData = new float[1024];
        audioData.GetSpectrumData(data, 0, FFTWindow.Rectangular);
        audioData.GetOutputData(amplitudeData, 0);
        AnalyzeSound(data);
        AnalyzeAmplitude(amplitudeData);
    }

    public void StartPlaying()
    {
        audioData.clip = m_playerObject.GetComponent<FrequencyAnalysis>().m_recordedClip;
        audioData.loop = true;
        audioData.Play();

        audioOutput.clip = m_playerObject.GetComponent<FrequencyAnalysis>().m_recordedClip;
        //float[] data = new float[ audioOutput.clip.samples];
        //audioOutput.clip.GetData(data,0);

        //for (int i = 0; i < data.Length; i++)
        //{
        //    // Get the avrage of the past number of frames
        //    float avrage = 0;
        //    for (int a = i; a < i + m_framesToAvrage; a++)
        //    {
        //        if (a < data.Length)
        //        {
        //            avrage += data[a];
        //        }
        //    }
        //    avrage /= m_framesToAvrage;

        //    if (avrage < m_amplitudeThreshold)
        //    {
        //        data[i] = 0;
        //    }
        //}
        //audioOutput.clip.SetData(data, 0);
        
        audioOutput.loop = true;
        audioOutput.Play();
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

        m_freqValues[m_frameIter] = highestFreq * (21000 / 1024);

        float avrage = 0;
        // Get the avrage of the past number of frames
        for (int i = 0; i < m_framesToAvrage; i++)
        {
            avrage += m_freqValues[i];
        }

        avrage /= m_framesToAvrage;
        m_currentFrequency = (int)avrage;
    }

    private void AnalyzeAmplitude(float[] data)
    {
        float sum = 0;
        for (int i = 0; i < data.Length; i++)
        {
            sum += System.Math.Abs(data[i]);
        }

        float ampl = 0;
        ampl = sum / m_maxAmplitude;
        if (ampl > 1)
            ampl = 1;
        m_amplitudeValues[m_frameIter] = ampl;

        // Get the avrage of the past number of frames
        float avrage = 0;
        for (int i = 0; i < m_framesToAvrage; i++)
        {
            avrage += m_amplitudeValues[i];
        }

        avrage /= m_framesToAvrage;
        m_currentAmplitude = avrage > m_amplitudeThreshold ? avrage : 0;
        //Debug.Log(sum);
    }

}
