using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnNoHealth : MonoBehaviour {

    [HideInInspector]
    public Vector3 respawnPos;

	// Use this for initialization
	void Start () {
        respawnPos = transform.position;
	}

    public void Respawn()
    {
        transform.position = respawnPos;
    }
}
