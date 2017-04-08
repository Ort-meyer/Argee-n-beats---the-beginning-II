using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformLinearPath : MonoBehaviour {
    public Rigidbody o_rigidbody;
    public float speedF = 100;

    public Transform[] paths_t;
    Transform currPath = null;
	// Use this for initialization
	void Start () {
        if (o_rigidbody == null)
        {
            o_rigidbody = GetComponent<Rigidbody>();
        }
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        o_rigidbody.MovePosition( Vector3.Lerp(o_rigidbody.position, currPath.position, Time.fixedDeltaTime * speedF));
	}

    public void MoveTo(int index)
    {
        currPath = paths_t[index];
    }
}
