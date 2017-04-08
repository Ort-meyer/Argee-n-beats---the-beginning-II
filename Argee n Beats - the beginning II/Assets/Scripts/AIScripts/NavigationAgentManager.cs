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
        target = GameObject.FindGameObjectWithTag("Player");
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
    }

    public void SetTargetPosition(Vector3 position)
    {
        target = null;
        targetPosition = position;
    }
}
