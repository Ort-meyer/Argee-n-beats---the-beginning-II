using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoiceIndicator : MonoBehaviour
{
    public GameObject indicator;
    float maxFrequency = 1000.0f; // TODO get from global
    float minAmplitude = 0.01f;
    FrequencyAnalysis freqAnalysis;

    float minX = 0.0f;
    float maxX = 0.0f;

    public enum IndicatorType
    {
        Frequency,
        Amplitude
    }

    public IndicatorType type = IndicatorType.Frequency;


    // Use this for initialization
    void Start()
    {
        freqAnalysis = GetComponent<FrequencyAnalysis>();
        GameObject background = indicator.transform.GetChild(0).gameObject;
        //background.GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width * 0.9f, background.GetComponent<RectTransform>().sizeDelta.y);

        minX = indicator.transform.GetChild(1).gameObject.GetComponent<RectTransform>().anchoredPosition.x;
        maxX = indicator.transform.GetChild(1).gameObject.GetComponent<RectTransform>().anchoredPosition.x + 700; // deal with it
    }

    // Update is called once per frame
    void Update()
    {
        float val = 0;

        switch (type)
        {
            case IndicatorType.Frequency:
                float freq = freqAnalysis.m_currentFrequency;
                freq /= maxFrequency;
                val = minX + (maxX - minX) * freq;
                break;
            case IndicatorType.Amplitude:
                float amp = freqAnalysis.m_currentAmplitude;
                val = minX + (maxX - minX) * amp;
                break;
            default:
                break;
        }



        GameObject background = indicator.transform.GetChild(0).gameObject;
        GameObject indic = indicator.transform.GetChild(1).gameObject;


        //-386.28
        RectTransform recTrans = indic.GetComponent<RectTransform>();
        Vector3 oldPos = indic.GetComponent<RectTransform>().anchoredPosition;
        indic.GetComponent<RectTransform>().anchoredPosition = new Vector3(val, oldPos.y, oldPos.z);

        //indic.GetComponent<RectTransform>().localPosition = new Vector2(-Screen.width*0.25f + (Screen.width* 0.5f) * freq, recTrans.localPosition.y);
    }
}
