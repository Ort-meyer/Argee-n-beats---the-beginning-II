using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSystemManager : MonoBehaviour {

    public GameObject[] particleSystems;

	// Use this for initialization
	void Start () {
        DeactivateAll();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Activate(string particleName)
    {
        GameObject obj = null;
        foreach (var item in particleSystems)
        {
            if (item.name.Equals(particleName))
            {
                obj = item;
            }
        }

        if (obj)
        {
            ParticleSystem sys = obj.GetComponent<ParticleSystem>();
            if (sys)
            {
                //sys.Simulate(0.0f, true, true);
                sys.Play();
            }
        }
    }

    public void Activate(int id)
    {
        if (id < particleSystems.Length)
        {
            GameObject obj = particleSystems[id];

            if (obj)
            {
                ParticleSystem sys = obj.GetComponent<ParticleSystem>();
                if (sys)
                {
                    //sys.Simulate(0.0f, true, true);
                    sys.Play();
                }
            }
        }
    }

    public void Deactivate(string particleName)
    {
        GameObject obj = null;
        foreach (var item in particleSystems)
        {
            if (item.name.Equals(particleName))
            {
                obj = item;
            }
        }

        if (obj)
        {
            ParticleSystem sys = obj.GetComponent<ParticleSystem>();
            if (sys)
            {
                //sys.Simulate(0.0f, true, true);
                sys.Stop();
            }
        }
    }

    public void Deactivate(int id)
    {
        if (id < particleSystems.Length)
        {
            GameObject obj = particleSystems[id];

            if (obj)
            {
                ParticleSystem sys = obj.GetComponent<ParticleSystem>();
                if (sys)
                {
                    //sys.Simulate(0.0f, true, true);
                    sys.Stop();
                }
            }
        }
    }

    public void DeactivateAll()
    {
        foreach (var item in particleSystems)
        {
            ParticleSystem sys = item.GetComponent<ParticleSystem>();
            if (sys)
            {
                sys.Stop();
            }
        }
    }

    public void KeepParticleSystemAliveBySound()
    {

    }
}
