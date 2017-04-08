using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavigationAgentManager : MonoBehaviour {
    public GameObject target;

    NavMeshAgent agent;

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
	}

    public void SetTargetObject(GameObject newTarget)
    {
        target = newTarget;
    }
}
