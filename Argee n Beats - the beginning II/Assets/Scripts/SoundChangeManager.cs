using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundChangeManager : MonoBehaviour {

    public static SoundChangeManager scManager = null;

    List<SoundChange> soundChangers = new List<SoundChange>();
    int counter = 0;

    void Awake()
    {
        if (scManager)
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

    bool CheckInRange(SoundChange sc)
    {
        return true;
    }

	// Update is called once per frame
	void Update () {

        // Update current
        if (counter < soundChangers.Count)
        {
            SoundChange sc = soundChangers[counter];

            // check if in range
            if (CheckInRange(sc))
            {
                sc.ColorByPosition(transform.position);
            }
        }
        

        counter++;
        if (counter > soundChangers.Count)
        {
            counter = 0;
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
