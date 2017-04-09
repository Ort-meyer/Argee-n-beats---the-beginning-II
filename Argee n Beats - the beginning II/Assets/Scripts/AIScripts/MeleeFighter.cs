using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeFighter : AttackBase {

	// Use this for initialization
	void Start () {
        base.BaseStart();
	}

    void OnEnable()
    {
        base.BaseOnEnable();
    }

    // Update is called once per frame
    void Update () {
        base.BaseUpdate();
        if (base.CanIAttackTarget())
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
