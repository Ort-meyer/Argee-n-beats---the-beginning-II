using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantKillingObject : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(!Physics.Raycast(transform.position, Vector3.up, 20))
        {
            GetComponent<Rigidbody>().AddForce(Vector3.up * 10000 * Time.deltaTime);
        }
	}
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<Health>().TakeDamage(100000000);
        }
    }
}
