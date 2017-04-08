using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AttackBase : MonoBehaviour {
    public GameObject objectToAttack;
    public float damage;
    [Tooltip("Attacks per second")]
    public float attackSpeed;
    public float attackDistance;

    protected float attackTimer;
	// Use this for initialization
	protected void BaseStart () {
        attackTimer = 1/attackSpeed;
	}
	
	// Update is called once per frame
	protected void BaseUpdate() {
        attackTimer -= Time.deltaTime;
    }
}
