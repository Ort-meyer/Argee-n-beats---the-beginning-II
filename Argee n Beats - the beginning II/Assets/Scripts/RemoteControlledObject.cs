using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoteControlledObject : MonoBehaviour {
    public Transform start;
    public Transform stop;
    public float speed;
    float fromStart = 0;
    bool triggered;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        if (triggered)
        {
            fromStart += speed * Time.deltaTime;
            fromStart = Mathf.Min(1, fromStart);
            transform.position = Vector3.Lerp(start.position, stop.position, fromStart);
        }
        else
        {
            fromStart -= speed * Time.deltaTime;
            fromStart = Mathf.Max(0, fromStart);
            transform.position = Vector3.Lerp(start.position, stop.position, fromStart);
        }
	}

    public void Enter()
    {
        triggered = true;
    }

    public void Exit()
    {
        triggered = false;
    }
}
