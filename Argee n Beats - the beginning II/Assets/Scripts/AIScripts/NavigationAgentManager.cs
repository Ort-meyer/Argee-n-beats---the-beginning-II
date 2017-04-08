using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavigationAgentManager : MonoBehaviour {
    public GameObject target;

    NavMeshAgent agent;
    Vector3 targetPosition = Vector3.zero;
	// Use this for initialization
	void Start () {
        agent = GetComponent<NavMeshAgent>();
    }
	
	// Update is called once per frame
	void Update () {
        if (target != null && (target.transform.position - this.transform.position).magnitude > agent.stoppingDistance)
        {
            agent.SetDestination(target.transform.position);
        }
        else if (targetPosition != Vector3.zero)
        {
            agent.SetDestination(targetPosition);
        }
	}

    public void SetTargetObject(GameObject newTarget)
    {
        target = newTarget;
        targetPosition = Vector3.zero;
    }

    public void SetTargetPosition(Vector3 position)
    {
        target = null;
        targetPosition = position;
    }

    public bool ReachedTarget()
    {
        bool reachedTarget = true;
        if (target != null)
        {
            reachedTarget = (target.transform.position - this.transform.position).magnitude <= agent.stoppingDistance;
        }
        else if (target == null && targetPosition != Vector3.zero)
        {
            float distance = (targetPosition - this.transform.position).magnitude;
            reachedTarget = (targetPosition - this.transform.position).magnitude <= agent.stoppingDistance;
        }
        return reachedTarget;
    }
}
