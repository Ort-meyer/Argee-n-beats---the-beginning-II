using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputManager : MonoBehaviour {

    FrequencyAnalysis freqAn;

    List<SoundAnalysisNotPlayer> soundAffectors = new List<SoundAnalysisNotPlayer>();
    float range = 2.0f;
    LayerMask collisionMask;

    GameObject recordObj;

	// Use this for initialization
	void Start () {
        collisionMask = LayerMask.GetMask("SoundAffectors");
        freqAn = GetComponent<FrequencyAnalysis>();

        GameObject canvas = GameObject.Find("Canvas");
        for (int i = 0; i < canvas.transform.childCount; i++)
        {
            if (canvas.transform.GetChild(i).name.Equals("RECORDING"))
            {
                recordObj = canvas.transform.GetChild(i).gameObject;
            }
        }
	}

    void GetAllCloseSoundeffectors()
    {
        soundAffectors.Clear();
        Collider[] col = Physics.OverlapBox(transform.position, new Vector3(range, range, range), Quaternion.identity, collisionMask);

        foreach (var item in col)
        {
            SoundAnalysisNotPlayer sanp = item.GetComponent<SoundAnalysisNotPlayer>();
            // We look in current and parent for a soundAnalysis!
            if (sanp)
            {
                soundAffectors.Add(sanp);
            }
            else
            {
                sanp = item.transform.parent.GetComponent<SoundAnalysisNotPlayer>();
                if (sanp)
                {
                    soundAffectors.Add(sanp);
                }
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
        if(freqAn.IsKeyRecording() == false)
        {
            // Previous
            List<SoundAnalysisNotPlayer> soundAffectorsOld = new List<SoundAnalysisNotPlayer>(soundAffectors);

            // Particle effect the close ones
            GetAllCloseSoundeffectors();

            // Stop and start new
            foreach (var item in soundAffectors)
            {
                if (soundAffectorsOld.Contains(item))
                {
                    soundAffectorsOld.Remove(item);
                    // Nothing
                }
                else
                {
                    // Start
                    item.GetComponent<ParticleSystemManager>().Activate(0);
                }
            }

            foreach (var item in soundAffectorsOld)
            {
                item.GetComponent<ParticleSystemManager>().Deactivate(0);
            }
        }

        // Update keycodes from input
        if (Input.GetKeyDown(KeyCode.G) && freqAn.IsKeyRecording() == false && soundAffectors.Count > 0)
        {
            // Start show recording
            recordObj.GetComponent<Text>().enabled = true;

            Debug.Log("Recording");
            // Check if we have any Sound object close()
            freqAn.StartRecording(2);

            // Apply affect to objects
            Invoke("FinishRecording", 2 + 0.1f);


        }
        if (Input.GetKeyDown(KeyCode.KeypadMinus))
        {
            freqAn.LowerAmplitudeThreshold();
        }
        if (Input.GetKeyDown(KeyCode.KeypadPlus))
        {
            freqAn.IncreaseAmplitudeThreshold();
        }
    }

    void FinishRecording()
    {
        Debug.Log("Finish Recording: " + soundAffectors.Count);
        freqAn.FinishKeyPressRecording();

        // Save Place recording
        foreach (var item in soundAffectors)
        {
            item.StartPlaying();

            // Activate keepAliver
            ParticleBySound pbs = item.GetComponent<ParticleBySound>();
            if (pbs)
            {
                pbs.KeepAliveParticleByAmplitude(1);
            }
            else
            {
                item.GetComponent<ParticleSystemManager>().Activate(1);
            }
        }

        // Stop recording gui
        recordObj.GetComponent<Text>().enabled = false;
    }
}
