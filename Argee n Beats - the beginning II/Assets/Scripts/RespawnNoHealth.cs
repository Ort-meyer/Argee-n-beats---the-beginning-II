using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnNoHealth : MonoBehaviour {

    [HideInInspector]
    public Vector3 respawnPos;
    Rigidbody rigidBody;

	// Use this for initialization
	void Start () {
        respawnPos = transform.position;
        rigidBody = GetComponent<Rigidbody>();
    }

    public void Respawn()
    {
        transform.position = respawnPos;
        rigidBody.velocity = Vector3.zero;
    }
}
