using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterPuller : MonoBehaviour
{
    public float m_maxFrequency = 700;
    public float m_threshold = 300;
    public float m_fireArc = 0.1f;
    public float m_fireForce = 30;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        FrequencyAnalysis freqAnalys = GetComponent<FrequencyAnalysis>();
        //float fireFactor = freqAnalys.m_currentFrequency / m_maxFrequency;
        //fireFactor -= 0.5f;
        //fireFactor *= 2;
        //Debug.Log(fireFactor);
        //Debug.Log(freqAnalys.m_currentFrequency);

        GameObject[] movables = GameObject.FindGameObjectsWithTag("DynamicObject");
        foreach (GameObject obj in movables)
        {
            Vector3 lineBetween = (obj.transform.position - transform.position).normalized;
            Vector3 cameraTarget = Camera.current.transform.forward.normalized;

            int fireFactor = 0;
            float dot = Vector3.Dot(lineBetween, cameraTarget);
            float derp = freqAnalys.m_currentAmplitude;
            //Debug.Log(derp);
            //Debug.Log(dot);
            if (freqAnalys.m_currentFrequency > m_threshold)
            {
                //Debug.Log("high");
                fireFactor = 1;
            }
            else if (freqAnalys.m_currentFrequency <= m_threshold && freqAnalys.m_currentFrequency > 40)
            {
                //Debug.Log("low");
                fireFactor = -1;
            } 

            if (Vector3.Dot(lineBetween, cameraTarget) > 1-m_fireArc)
            {
                if(freqAnalys.m_currentFrequency > 40)
                obj.GetComponent<Rigidbody>().AddForce(lineBetween * m_fireForce * fireFactor * freqAnalys.m_currentAmplitude);
            }

        }
    }
}
