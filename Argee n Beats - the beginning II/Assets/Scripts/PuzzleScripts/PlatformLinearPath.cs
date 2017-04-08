using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformLinearPath : MonoBehaviour {
    public Rigidbody o_rigidbody;
    public float speedF = 100;

    public Transform[] paths_t;
    Transform currPath = null;
    int currIndex = 0;
	// Use this for initialization
	void Start () {
        if (o_rigidbody == null)
        {
            o_rigidbody = GetComponent<Rigidbody>();
        }
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        if(Vector3.Distance(o_rigidbody.position, currPath.position) < Time.fixedDeltaTime * speedF)
        {
            return;
        }
        Vector3 dir = (currPath.position - o_rigidbody.position).normalized;
        o_rigidbody.MovePosition(o_rigidbody.position + dir * Time.fixedDeltaTime * speedF); //flytta den från den gamla positionen
	}

    public void MoveTo(int index)
    {
        currIndex = index;
        currPath = paths_t[currIndex];
    }
}
