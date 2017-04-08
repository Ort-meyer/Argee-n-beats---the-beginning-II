using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementManager : MonoBehaviour {
    public float noOutsideForceVelcoityThreshold;
    public float dragWhenOutsideForce;
    Vector3 impulseToAdd;
    bool gotOutsideForce;
    bool shouldAddOutsideForce;

    FollowNavigationAgent followNavigation;
	// Use this for initialization
	void Start () {
        followNavigation = GetComponent<FollowNavigationAgent>();
	}
	
	// Update is called once per frame
	void Update () {
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
}
