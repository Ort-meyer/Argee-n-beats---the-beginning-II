﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrequencyAnalysis : MonoBehaviour
{
    public AudioClip m_recordedClip;
    
    private int recordingDuration = 5;

    public int m_currentFrequency;
    public float m_minFreqStength = 0.01f;
    public int m_momentaryFrequency;
    public float m_currentAmplitude;
    public float m_amplitudeThreshold = 0.2f;
    public float m_amplitudeIncreaseDecreaseValue = 0.05f;
    public float m_momentaryAmplitude;
    public float m_maxAmplitude = 100; // This value should probably be a bit higher

    public int m_keyPressRecordDuration = 2;
    private bool m_keyPressRecording = false;

    private float[] m_amplitudeValues = new float[m_framesToAvrage];
    private float[] m_freqValues = new float[m_framesToAvrage];

    const int m_framesToAvrage = 10;
    int m_frameIter = 0;

    // Use this for initialization
    void Start()
    {
        //m_maxAmplitude = 4; FOr some reason it appears as though this needs to be set here...
        //InvokeRepeating("ResetMic", 0, recordingDuration);
        AudioSource audio = GetComponent<AudioSource>();
        audio.clip = Microphone.Start(null, true, recordingDuration, 44100);
        audio.loop = true;
        while (!(Microphone.GetPosition(null) > 0)) { }
        Microphone.GetPosition(null);
        audio.Play();
    }

    public bool IsKeyRecording()
    {
        return m_keyPressRecording;
    }

    public void StartRecording(int time)
    {
        AudioSource audio = GetComponent<AudioSource>();
        // User wants to record a sound
        m_keyPressRecording = true;
        audio.Stop();
        audio.clip = Microphone.Start(null, false, time, 44100);
    }

    // Update is called once per frame
    void Update()
    {
        m_frameIter++;
        if (m_frameIter >= m_framesToAvrage)
            m_frameIter = 0;


        AudioSource audio = GetComponent<AudioSource>();

        //// User wants to record a sound
        //if (Input.GetKeyUp(KeyCode.G))
        //{
        //    m_keyPressRecording = true;
        //    audio.Stop();
        //    audio.clip = Microphone.Start(null, false, 2, 44100);
        //    Invoke("FinishKeyPressRecording", 2 + 0.1f);
        //}
        // Stop analyzing when we're recording from key press
        if (!m_keyPressRecording)
        {
            float[] data = new float[1024];
            float[] amplitudeData = new float[1024];
            audio.GetSpectrumData(data, 0, FFTWindow.Rectangular);
            audio.GetOutputData(amplitudeData, 0);
            AnalyzeSound(data);
            AnalyzeAmplitude(amplitudeData);
        }

        //Debug.Log(m_currentFrequency);
    }

    public void FinishKeyPressRecording()
    {
        m_keyPressRecording = false;
        // Hope this copy works
        m_recordedClip = GetComponent<AudioSource>().clip;
        ResetMic();
    }

    private void ResetMic()
    {
        if (!m_keyPressRecording)
        {
            AudioSource audio = GetComponent<AudioSource>();
            audio.clip = Microphone.Start(null, true, recordingDuration, 44100);
            audio.loop = true;
            while (!(Microphone.GetPosition(null) > 0)) { }
            Microphone.GetPosition(null);
            audio.Play();
        }
    }

    private void AnalyzeSound(float[] data)
    {
        float packageData = 0;
        float highest = 0;
        int highestFreq = 0;
        for (int i = 0; i < data.Length; i++)
        {
            if (data[i] > highest && data[i] > m_minFreqStength)
            {
                highest = data[i];
                highestFreq = i;
            }
            packageData += System.Math.Abs(data[i]);
        }

        m_freqValues[m_frameIter] = highestFreq * (21000 / 1024);
        m_momentaryFrequency = (int)m_freqValues[m_frameIter];
        float avrage = 0;
        // Get the avrage of the past number of frames
        for (int i = 0; i < m_framesToAvrage; i++)
        {
            avrage += m_freqValues[i];
        }

        avrage /= m_framesToAvrage;
        m_currentFrequency = (int)avrage;

        //m_currentFrequency = highestFreq * (21000 / 1024);
        //Debug.Log(m_currentFrequency);
    }

    private void AnalyzeAmplitude(float[] data)
    {
        float sum = 0;
        for (int i = 0; i < data.Length; i++)
        {
            sum += System.Math.Abs(data[i]);
        }
        float ampl = 0;
        //Debug.Log(sum);
        ampl = sum/ m_maxAmplitude;
        if (ampl > 1)
            ampl = 1;
        m_amplitudeValues[m_frameIter] = ampl;
        m_momentaryAmplitude = m_amplitudeValues[m_frameIter];

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

    public void LowerAmplitudeThreshold()
    {
        m_amplitudeThreshold -= m_amplitudeIncreaseDecreaseValue;
        m_amplitudeThreshold = Mathf.Max(0, m_amplitudeThreshold);
    }

    public void IncreaseAmplitudeThreshold()
    {
        m_amplitudeThreshold += m_amplitudeIncreaseDecreaseValue;
        m_amplitudeThreshold = Mathf.Min(1, m_amplitudeThreshold);
    }

}
