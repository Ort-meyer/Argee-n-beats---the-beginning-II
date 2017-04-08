using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pusher : MonoBehaviour {
    [System.NonSerialized]
    public bool activated = false;

    public float pushForce = 100;
    public float maxVel = 100;
	// Use this for initialization
	void Start () {
        activated = false;
	}
	
    void OnTriggerStay(Collider c)
    {
        if (!activated)
            return;

        Rigidbody o_r = c.GetComponent<Rigidbody>();

        if(o_r != null)
        {
            o_r.AddForce(transform.up * pushForce * Time.deltaTime, ForceMode.Force);
        }
    }

    public void Toggle(bool b)
    {
        activated = b;
    }
}
