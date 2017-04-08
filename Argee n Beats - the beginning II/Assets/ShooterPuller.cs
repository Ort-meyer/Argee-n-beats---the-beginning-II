using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterPuller : MonoBehaviour
{
    public float m_maxFrequency = 1000;
    public float m_fireArc = 0.1;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        FrequencyAnalysis freqAnalys = GetComponent<FrequencyAnalysis>();
        float fireFactor = freqAnalys.m_currentFrequency / m_maxFrequency;
        fireFactor -= 0.5f;
        fireFactor *= 2;

        GameObject[] movables = GameObject.FindGameObjectsWithTag("DynamicObject");
        foreach (GameObject obj in movables)
        {

        }
    }
}
