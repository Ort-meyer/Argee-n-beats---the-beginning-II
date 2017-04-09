using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundChangeManager : MonoBehaviour {

    public static SoundChangeManager scManager = null;

    List<SoundChange> soundChangers = new List<SoundChange>();
    int counter = 0;

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

        int counter = 0;
        foreach (var item in soundChangers)
        {
            float rangeV = 3.0f;
            if (CheckInRange(item, rangeV))
            {
                item.ColorByPositionFull(transform.position, rangeV - (rangeV * 0.2f), rangeV);
                counter++;
            }
        }
        //Debug.Log(counter);
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
