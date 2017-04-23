using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    [RequireComponent(typeof(PlatformLinearPath))] 
public class PlatformContinousMove : MonoBehaviour {
    PlatformLinearPath linearPath;
    int paths;
    int currentPath = 0;
    bool active = false;
	// Use this for initialization
	void Start () {
        linearPath = GetComponent<PlatformLinearPath>();
        paths = linearPath.paths_t.Length;
	}
	
	// Update is called once per frame
	void Update () {
        if (active)
        {
            if (linearPath.ReachedCurrentPath())
            {
                currentPath++;
                if (currentPath >= paths)
                {
                    currentPath = 0;
                }
                linearPath.MoveTo(currentPath);
            }
        }
	}

    public void Enter()
    {
        if (!active)
        {
            linearPath.MoveTo(currentPath);
        }
        active = true;
    }

    public void Exit()
    {
        if (active)
        {
            linearPath.StopMove();
        }
        active = false;
    }
}
