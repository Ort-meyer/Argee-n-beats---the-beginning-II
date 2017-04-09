using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour {
    public float startingHealth;
    public bool respawnOnDeath = false;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void TakeDamage(float damage)
    {
        startingHealth -= damage;
        if (startingHealth <= 0.0f)
        {
            if (respawnOnDeath)
            {
                transform.position = CheckpointManager.manager.activeCheckpoint.transform.position;
                transform.rotation = CheckpointManager.manager.activeCheckpoint.transform.rotation;
            }
            else
            {
                Destroy(gameObject);
            }
            if (gameObject.tag == "Player")
            {
                AllEnemyTracker.manager.KilledPlayer(gameObject);
            }
        }
    }
}
