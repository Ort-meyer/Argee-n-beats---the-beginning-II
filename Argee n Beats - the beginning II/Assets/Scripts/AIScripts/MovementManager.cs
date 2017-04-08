using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementManager : MonoBehaviour {
    public float noOutsideForceVelcoityThreshold;
    public float dragWhenOutsideForce;
    public float agroRange;
    public float allertRadius;
    public GameObject patroleManager;
    Vector3 impulseToAdd;
    bool gotOutsideForce;
    bool shouldAddOutsideForce;

    GameObject[] players;
    FollowNavigationAgent followNavigation;
    AttackBase attackScript;
    bool inAgro = false;
	// Use this for initialization
	void Start () {
        followNavigation = GetComponent<FollowNavigationAgent>();
        attackScript = GetComponent<AttackBase>();
        players = GameObject.FindGameObjectsWithTag("Player");
        if (attackScript != null)
        {
            attackScript.enabled = false;
        }
        if (patroleManager != null)
        {
            Vector3 newPos = patroleManager.GetComponent<NextPatrolPoint>().GetNextPatrolePoint();
            print(newPos);
            followNavigation.navigation.GetComponent<NavigationAgentManager>().SetTargetPosition(newPos);
        }
    }
	
	// Update is called once per frame
	void Update () {
        GameObject attackPlayer = null;
        if (!inAgro)
        {
            players = GameObject.FindGameObjectsWithTag("Player");
            float distClosestEnemy = float.MaxValue;
            foreach (var item in players)
            {
                float distToItem = (item.transform.position - this.transform.position).magnitude;
                if (distToItem <= distClosestEnemy)
                {
                    distClosestEnemy = distToItem;
                    attackPlayer = item;
                }
            }
            if (distClosestEnemy > agroRange)
            {
                attackPlayer = null;
            }
        }
        if (!inAgro && attackPlayer != null)
        {
            Collider[] allInRadius = Physics.OverlapSphere(transform.position, allertRadius);
            foreach (var item in allInRadius)
            {
                MovementManager manager = item.GetComponent<MovementManager>();
                if (manager != null)
                {
                    manager.AllertOfTarget(attackPlayer);
                }
            }
        }
        if (!inAgro)
        {
            bool reachedPatrolePoint = followNavigation.navigation.GetComponent<NavigationAgentManager>().ReachedTarget();
            
            if (reachedPatrolePoint && patroleManager != null)
            {
                Vector3 newPos = patroleManager.GetComponent<NextPatrolPoint>().GetNextPatrolePoint();
                followNavigation.navigation.GetComponent<NavigationAgentManager>().SetTargetPosition(newPos);
            }
        }
        if (gotOutsideForce == true && GetComponent<Rigidbody>().velocity.magnitude< noOutsideForceVelcoityThreshold)
        {
            gotOutsideForce = false;
            if (followNavigation != null)
            {
                followNavigation.enabled = true;
            }
        }
	}

    void FixedUpdate()
    {
        if (shouldAddOutsideForce)
        {
            GetComponent<Rigidbody>().AddForce(impulseToAdd, ForceMode.Impulse);
            GetComponent<Rigidbody>().angularDrag = dragWhenOutsideForce;
            impulseToAdd = Vector3.zero;
            gotOutsideForce = true;
            shouldAddOutsideForce = false;
            if (followNavigation != null)
            {
                followNavigation.enabled = false;
            }
        }
    }

    public void AddImpulse(Vector3 impulse)
    {
        shouldAddOutsideForce = true;
        impulseToAdd += impulse;
    }

    public void AllertOfTarget(GameObject target)
    {
        attackScript.enabled = true;
        attackScript.objectToAttack = target;
        followNavigation.navigation.GetComponent<NavigationAgentManager>().SetTargetObject(target);
        inAgro = true;
    }
}
