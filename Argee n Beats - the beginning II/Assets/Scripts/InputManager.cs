using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour {

    FrequencyAnalysis freqAn;

    List<SoundAnalysisNotPlayer> soundAffectors = new List<SoundAnalysisNotPlayer>();
    float range = 2.0f;
    LayerMask collisionMask = LayerMask.GetMask("SoundAffectors");

	// Use this for initialization
	void Start () {
        freqAn = GetComponent<FrequencyAnalysis>();
	}

    void GetAllCloseSoundeffectors()
    {
        soundAffectors.Clear();
        Collider[] col = Physics.OverlapBox(transform.position, new Vector3(range, range, range), Quaternion.identity, collisionMask);

        foreach (var item in col)
        {
            SoundAnalysisNotPlayer sanp = item.GetComponent<SoundAnalysisNotPlayer>();
            if (sanp)
            {
                soundAffectors.Add(sanp);
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
        if(freqAn.IsKeyRecording() == false)
        {
            // Particle effect the close ones
            GetAllCloseSoundeffectors();
        }

        // Update keycodes from input
        if (Input.GetKeyDown(KeyCode.G) && freqAn.IsKeyRecording() == false)
        {
            // Check if we have any Sound object close()
            freqAn.StartRecording();

            // Apply affect to objects

        }
		
	}
}
