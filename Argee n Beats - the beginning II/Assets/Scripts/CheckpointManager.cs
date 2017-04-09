using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointManager : MonoBehaviour {
    public static CheckpointManager manager = null;
    public GameObject activeCheckpoint;
	// Use this for initialization
	void Start () {
        if (manager != null)
        {
            Debug.LogError("Two instances of Checkpointmanager in scene");
        }
        manager = this;
    }

    // Update is called once per frame
    public void ChangeActiveCheckpoint(GameObject newCheckpoint)
    {
        activeCheckpoint = newCheckpoint;
    }
}
