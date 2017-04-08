using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformGonner : MonoBehaviour {
    public Rigidbody o_rigidbody;
    public float force = 100;

    public Transform directionObject;
	// Use this for initialization
	void Start () {
		if(o_rigidbody == null)
        {
            o_rigidbody = GetComponent<Rigidbody>();
        }
	}

    public void FlyAway()
    {
        o_rigidbody.isKinematic = false;
        foreach(Collider col in o_rigidbody.transform.GetComponentsInChildren<Collider>())
        {
            col.isTrigger = true;
        }

        Vector3 dir = directionObject.position - o_rigidbody.transform.position.normalized;

        o_rigidbody.AddForce(force * dir, ForceMode.Impulse);
    }
}
