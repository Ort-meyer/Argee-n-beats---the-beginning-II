using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animation))]
public class AnimationManagement : MonoBehaviour {
    [Tooltip("Should be IDLE, RUNNING, FIGHTING in that order")]
    public AnimationClip[] clips;
    public enum ClipType
    {
        IDLE,
        RUNNING,
        FIGHTING,
    }
    string activeClip;
    string queuedClip;
	// Use this for initialization

    void Start () {
        activeClip = clips[0].name;
    }
	
	// Update is called once per frame
	void Update () {
        if (queuedClip != "" && !GetComponent<Animation>().isPlaying)
        {
            activeClip = queuedClip;
            queuedClip = "";
            GetComponent<Animation>().Play(activeClip);
        }
    }

    public void ChangeCurrentAnimation(ClipType animation)
    {
        if (activeClip == clips[(int)ClipType.FIGHTING].name)
        {
            queuedClip = clips[(int)animation].name;
        }
        else
        {
            activeClip = clips[(int)animation].name;
            GetComponent<Animation>().Play(activeClip);
        }
    }

    
}
