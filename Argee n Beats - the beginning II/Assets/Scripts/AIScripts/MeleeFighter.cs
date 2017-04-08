using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeFighter : AttackBase {

	// Use this for initialization
	void Start () {
        base.BaseStart();
	}
	
	// Update is called once per frame
	void Update () {
        base.BaseUpdate();
        if (attackTimer <= 0 && (transform.position - objectToAttack.transform.position).magnitude <= attackDistance)
        {
            attackTimer = 1/attackSpeed;
            MakeAttack();
        }
	}

    private void MakeAttack()
    {
        Health attackObjectHealth = objectToAttack.GetComponent<Health>();
        if (attackObjectHealth != null)
        {
            attackObjectHealth.TakeDamage(damage);
        }
        
    }
}
