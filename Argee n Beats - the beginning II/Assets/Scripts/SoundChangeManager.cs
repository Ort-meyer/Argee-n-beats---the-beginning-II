using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundChangeManager : MonoBehaviour {

    public static SoundChangeManager scManager = null;

    List<SoundChange> soundChangers = new List<SoundChange>();
    int counter = 0;

    float maxAmplitudeRange = 15.0f;
    float fallofDistance = 5.0f;

    void Awake()
    {
        if (scManager == null)
        {
            scManager = this;
        }
        else
        {
            Debug.LogError("MULTIPLE SINGLETON");
        }
    }

	// Use this for initialization
	void Start () {
		
	}

    bool CheckInRange(SoundChange sc, float range)
    {
        if ((new Vector3(sc.transform.position.x, 0.0f, sc.transform.position.z) - new Vector3(transform.position.x, 0.0f, transform.position.z)).magnitude < (range + sc.radius))
        {
            return true;
        }
        return false;
    }

	// Update is called once per frame
	void Update () {

        //// Update current
        //while (counter < soundChangers.Count)
        //{
        //    SoundChange sc = soundChangers[counter];

        //    // check if in range
        //    if (CheckInRange(sc, 6.0f))
        //    {
        //        sc.ColorByPositionFull(transform.position, 3.0f, 6.0f);
        //        //sc.ColorByPositionSelected(transform.position, 3.0f, 6.0f);
        //        break;
        //    }
        //    counter++;
        //}


        //counter++;
        //if (counter > soundChangers.Count)
        //{
        //    counter = 0;
        //}

        FrequencyAnalysis freqAn = GetComponent<FrequencyAnalysis>();
        float rangeMax = freqAn.m_currentAmplitude*maxAmplitudeRange;

        float rangeMin = Mathf.Max(rangeMax - fallofDistance, 0.0f);
        int counter = 0;
        foreach (var item in soundChangers)
        {

            if (CheckInRange(item, rangeMax))
            {
                item.ColorByPositionFull(transform.position, rangeMin, rangeMax);
                counter++;
            }
        }
    }

    public void Register(SoundChange sc)
    {
        soundChangers.Add(sc);
    }

    public void Unregister(SoundChange sc)
    {
        soundChangers.Remove(sc);
    }
}
