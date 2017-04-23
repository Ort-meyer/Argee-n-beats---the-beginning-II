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

    protected void BaseOnEnable()
    {
        GameObject navigation = GetComponent<FollowNavigationAgent>().navigation;
        if (navigation != null)
        {
            navigation.GetComponent<NavMeshAgent>().stoppingDistance = attackDistance;
        }
    }
	
	// Update is called once per frame
	protected void BaseUpdate() {
        attackTimer -= Time.deltaTime;
    }

    protected bool CanIAttackTarget()
    {
        bool canAttack = attackTimer <= 0 && objectToAttack != null && (transform.position - objectToAttack.transform.position).magnitude <= attackDistance;
        if (GetComponentInChildren<AnimationManagement>() != null && canAttack)
        {
            GetComponentInChildren<AnimationManagement>().ChangeCurrentAnimation(AnimationManagement.ClipType.FIGHTING);
        }
        return canAttack;
    }
}
