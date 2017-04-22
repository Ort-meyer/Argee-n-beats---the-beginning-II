using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveScript : MonoBehaviour {
    float val;
	// Use this for initialization
	void Start () {
        val = transform.position.x;
    }
	
	// Update is called once per frame
	void Update () {
        if (GetComponent<Rigidbody>().velocity.x < 2)
        {
            GetComponent<Rigidbody>().AddForce(new Vector3(1, 0, 0), ForceMode.VelocityChange);
        }

	}
}
