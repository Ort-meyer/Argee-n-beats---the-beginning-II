using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowNavigationAgent : MonoBehaviour {
    public GameObject navigation;
    public float acceleration; // Units/second
    private Vector3 totalForce;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        // Find the direction vector
        Vector3 direction = navigation.transform.position - this.transform.position;
        float distance = direction.magnitude;
        direction.Normalize();
        if (direction != Vector3.zero)
        {
            totalForce += direction * acceleration * Time.deltaTime;
        }

	}

    void FixedUpdate()
    {
        GetComponent<Rigidbody>().AddForce(totalForce, ForceMode.Acceleration);
    }
}
