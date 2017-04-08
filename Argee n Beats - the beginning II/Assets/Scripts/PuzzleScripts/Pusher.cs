using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pusher : MonoBehaviour {
    public bool activated = false;
    public bool pushAway = false;
    public ForceMode forceMode = ForceMode.Force;

    public float pushForce = 100;
	// Use this for initialization
	void Start () {
	}
	
    void OnTriggerStay(Collider c)
    {
        if (!activated)
            return;

        Rigidbody o_r = c.GetComponent<Rigidbody>();

        if(o_r != null)
        {
            if(pushAway)
            {
                Vector3 tSameY = new Vector3(transform.position.x, c.transform.position.y, transform.position.z);
                Vector3 dir = (c.transform.position - tSameY).normalized;
                o_r.AddForce(dir * pushForce * Time.deltaTime, forceMode);
            }
            else
            {
                o_r.AddForce(transform.up * pushForce * Time.deltaTime, forceMode);
            }
        }
    }

    public void Toggle(bool b)
    {
        activated = b;
    }
}
