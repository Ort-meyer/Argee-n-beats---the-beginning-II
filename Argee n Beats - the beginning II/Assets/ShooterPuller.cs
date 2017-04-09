using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterPuller : MonoBehaviour
{
    public float m_maxFrequency = 700;
    public float m_threshold = 300;
    public float m_fireArc = 0.1f;
    public float m_fireForce = 30;
    public int sucking;
    public bool m_pullingIntoOrbit = false;
    public bool m_shootingFromOrbit = false;
    public int m_fireMode = 1;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        m_pullingIntoOrbit = false;
        m_shootingFromOrbit = false;
        FrequencyAnalysis freqAnalys = GetComponent<FrequencyAnalysis>();
        sucking = 0;
        if (freqAnalys.m_currentFrequency > m_threshold)
        {
            sucking = 1;
        }
        else if (freqAnalys.m_currentFrequency <= m_threshold && freqAnalys.m_currentFrequency > 40)
        {
            sucking = -1;

        }

        GameObject[] movables = GameObject.FindGameObjectsWithTag("DynamicObject");
        // Normal fire mode
        if (m_fireMode == 1)
        {
            foreach (GameObject obj in movables)
            {
                Vector3 lineBetween = (obj.transform.position - transform.position).normalized;
                Vector3 cameraTarget = Camera.current.transform.forward.normalized;

                float dot = Vector3.Dot(lineBetween, cameraTarget);
                float derp = freqAnalys.m_currentAmplitude;


                if (Vector3.Dot(lineBetween, cameraTarget) > 1 - m_fireArc)
                {
                    obj.GetComponent<Rigidbody>().AddForce(lineBetween * m_fireForce * sucking);
                }
            }
        }
        // Charge gun
        else if (m_fireMode == 2)
        {
            if (sucking == -1 || sucking == 1)
            {
                m_pullingIntoOrbit = true;
            }
            if(sucking == 1)
            {
                m_shootingFromOrbit = true;
            }
        }
    }
    void HandleInput()
    {
        if(Input.GetKeyUp(KeyCode.Alpha1))
        {
            m_fireMode = 1;
        }
        else if(Input.GetKeyUp(KeyCode.Alpha2))
        {
            m_fireMode = 2;
        }
    }
}
