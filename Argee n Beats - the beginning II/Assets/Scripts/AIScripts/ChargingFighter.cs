using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChargingFighter : AttackBase {
    public GameObject navigation;
    public float chargeSteepness;
    public float chargeSpeed;
    public float chargeAcceleration;
    public float stunOnHitDuration;
    public float collisionStunThreshold;

    private bool charging = false;
    private bool stunned = false;
    private float stunTimer;
    private NavMeshAgent agent;
    private Vector3 lastVelocity;
    // Use this for initialization
    void Start()
    {
        base.BaseStart();
        agent = navigation.GetComponent<NavMeshAgent>(); 
    }

    // Update is called once per frame
    void Update()
    {
        // UGLY!!
        if (agent.stoppingDistance != 0.1f)
        {
            agent.stoppingDistance = 0.1f;
        }

        
        if (stunned == true)
        {
            stunTimer -= Time.deltaTime;
            GetComponent<FollowNavigationAgent>().enabled = false;
            GetComponent<TakeDamageOnImpact>().useIgnoreTags = false;
        }
        else
        {
            base.BaseUpdate();
            if (!charging && base.CanIAttackTarget())
            {
                // make sure we are acctually walking towards the enemy..
                if (navigation.GetComponent<NavigationAgentManager>().target == objectToAttack)
                {
                    float leftToEnemy = agent.remainingDistance;
                    if (leftToEnemy < attackDistance)
                    {
                        if (leftToEnemy > (objectToAttack.transform.position - this.transform.position).magnitude - chargeSteepness)
                        {
                            attackTimer = 1 / attackSpeed;
                            MakeAttack();
                        }
                    }
                }
            }
        }
        if (stunned && stunTimer <= 0)
        {
            GetComponent<FollowNavigationAgent>().ResetSpeedAndAcceleration();
            GetComponent<FollowNavigationAgent>().enabled = true;
            stunned = false;
        }
    }

    void FixedUpdate()
    {
        lastVelocity = GetComponent<Rigidbody>().velocity;
    }
    
    void OnCollisionEnter(Collision collision)
    {
        if (charging && collision.relativeVelocity.magnitude > collisionStunThreshold)
        {
            bool hitWall = false;
            foreach (var item in collision.contacts)
            {
                if (Mathf.Abs(Vector3.Dot(item.normal, lastVelocity.normalized)) > 0.4f)
                {
                    hitWall = true;
                }
            }
            if (hitWall)
            {
                charging = false;
                stunned = true;
                stunTimer = stunOnHitDuration;
                GetComponent<Rigidbody>().velocity = Vector3.zero;
                GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
                GetComponent<Rigidbody>().angularDrag = 3;
                GetComponent<FollowNavigationAgent>().enabled = false;
            }
        }
        if (collision.gameObject.tag == "Player")
        {
            Health playerHP =  collision.gameObject.GetComponent<Health>();
            playerHP.TakeDamage(damage);
        }
    }

    private void MakeAttack()
    {
        GetComponent<FollowNavigationAgent>().ChangeSpeedAndAcceleration(chargeSpeed, chargeAcceleration);
        charging = true;
        GetComponent<TakeDamageOnImpact>().useIgnoreTags = true;
    }
}
