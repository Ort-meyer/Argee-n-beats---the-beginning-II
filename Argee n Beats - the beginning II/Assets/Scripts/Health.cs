using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour {
    public float startingHealth;
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
            Destroy(gameObject);
        }
    }
}
