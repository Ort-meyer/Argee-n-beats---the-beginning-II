using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleBySound : MonoBehaviour {

    int id = 0;
    bool alive = false;
    
    SoundAnalysisNotPlayer soundAn = null;
    ParticleSystemManager pman = null;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        if (alive)
        {
            if(soundAn.m_currentAmplitude > 0.2f)
            {
                if (pman.IsActive(id) == false)
                {
                    pman.Activate(id);
                }
            }
            else
            {
                if (pman.IsActive(id))
                {
                    pman.Deactivate(id);
                }
            }
        }
	}

    public void KeepAliveParticleByAmplitude(int inid)
    {
        alive = true;
        id = inid;

        soundAn = GetComponent<SoundAnalysisNotPlayer>();
        pman = GetComponent<ParticleSystemManager>();
        if (soundAn == null || pman == null)
        {
            alive = false;
        }
    }
}
