using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MovementManager : MonoBehaviour {
    public float noOutsideForceVelcoityThreshold;
    public float dragWhenOutsideForce;
    public float agroRange;
    public float allertRadius;
    public GameObject patroleManager;
    public float artificialDrag = .5f;
    Vector3 impulseToAdd;
    bool gotOutsideForce;
    bool shouldAddOutsideForce;
    bool isOnGround = false;

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
            followNavigation.navigation.GetComponent<NavigationAgentManager>().SetTargetPosition(newPos);
        }

        AllEnemyTracker.manager.NewEnemy(gameObject);
    }
	
	// Update is called once per frame
	void Update () {
        GameObject attackPlayer = null;
        isOnGround = IsOnGround();
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
                followNavigation.navigation.GetComponent<NavMeshAgent>().stoppingDistance = 1;
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
        if (!isOnGround)
        {
            followNavigation.enabled = false;
        }
        else if (!gotOutsideForce)
        {
            followNavigation.enabled = true;
        }
	}

    void OnDestroy()
    {
        AllEnemyTracker.manager.KilledEnemy(gameObject);
    }

    void FixedUpdate()
    {
        if (gotOutsideForce && isOnGround)
        {
            Rigidbody rb = GetComponent<Rigidbody>();
            rb.AddForce(-rb.velocity.normalized * artificialDrag, ForceMode.VelocityChange);
        }
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

    public void PlayerDead(GameObject player)
    {
        if (inAgro && attackScript != null)
        {
            if (player == attackScript.objectToAttack)
            {
                // The enemy we were chasing is DEAD!!!! Go back to patrolling
                Vector3 newPos = patroleManager.GetComponent<NextPatrolPoint>().GetNextPatrolePoint();
                followNavigation.navigation.GetComponent<NavigationAgentManager>().SetTargetPosition(newPos);
                followNavigation.navigation.GetComponent<NavMeshAgent>().stoppingDistance = 1;
                attackScript.enabled = false;
                inAgro = false;
            }
        }
    }

    private bool IsOnGround()
    {
        RaycastHit t_info;
        LayerMask skipme = LayerMask.NameToLayer("IgnoreCameraOcclusion"); // kanske ta med fiender etc

        int layer = int.MaxValue;
        int test = 1 << skipme;
        //layer &=~test;
        layer -= test;
        bool r_hit = Physics.Raycast(transform.position, Vector3.down, out t_info, GetComponent<Collider>().bounds.extents.y + 0.2f, layer);
        return r_hit;
    }
}
