using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour {
    public float startingHealth;
    public bool respawnOnDeath = false;
    GameObject youDiedObj;

    // Use this for initialization
    void Start () {
		if(gameObject.tag.Equals("Player"))
        {
            youDiedObj = GameObject.Find("YOUDIED");
        }
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
                youDiedObj.gameObject.GetComponent<Text>().enabled = true;
                Invoke("DisableHealthGUI", 3.0f);
                AllEnemyTracker.manager.KilledPlayer(gameObject);
            }
        }
    }

    void DisableHealthGUI()
    {
        youDiedObj.gameObject.GetComponent<Text>().enabled = false;
    }
}
