using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FollowNavigationAgent : MonoBehaviour {
    public GameObject navigation;
    public float acceleration; // Units/second
    public float maxSpeed; // units/second

    private float standardSpeed;
    private float standardAcceleration;

    private Vector3 totalForce = Vector3.zero;
	// Use this for initialization
	void Start () {
        navigation.GetComponent<NavMeshAgent>().speed = maxSpeed+2.0f;
        navigation.GetComponent<NavMeshAgent>().acceleration = acceleration;
        standardSpeed = maxSpeed;
        standardAcceleration = acceleration;
    }

    void OnEnable()
    {
        totalForce = Vector3.zero;
        navigation.transform.position = this.transform.position;
        navigation.GetComponent<NavMeshAgent>().Warp(this.transform.position);
        GetComponent<Rigidbody>().angularDrag = 0.05f;
    }
	
	// Update is called once per frame
	void Update () {
        // Find the direction vector
        Vector3 direction = navigation.transform.position - this.transform.position;
        float distance = direction.magnitude;
        direction.Normalize();
        if (distance > 2.0f)
        {
            navigation.transform.position = this.transform.position + direction;
        }
        if (distance < 0.1)
        {
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            totalForce = Vector3.zero;
            if (GetComponentInChildren<AnimationManagement>() != null)
            {
                GetComponentInChildren<AnimationManagement>().ChangeCurrentAnimation(AnimationManagement.ClipType.IDLE);
            }
        }
        else if (direction != Vector3.zero)
        {
            totalForce += direction * acceleration * Time.deltaTime;
            if (GetComponentInChildren<AnimationManagement>() != null)
            {
                GetComponentInChildren<AnimationManagement>().ChangeCurrentAnimation(AnimationManagement.ClipType.RUNNING);
            }
        }
    }

    void FixedUpdate()
    {
        Rigidbody myBody = GetComponent<Rigidbody>();
        if (GetComponentInChildren<AnimationManagement>() != null)
        {
            if (totalForce != Vector3.zero)
            {
                Vector3 newDirection = totalForce.normalized;
                newDirection = Vector3.ProjectOnPlane(newDirection, new Vector3(0, 1, 0));
                if (newDirection != Vector3.zero)
                {
                    transform.forward = newDirection;

                }
            }
        }
        myBody.AddForce(totalForce, ForceMode.VelocityChange);
        
        totalForce = Vector3.zero;
        if (myBody.velocity.magnitude > maxSpeed)
        {
            myBody.velocity = myBody.velocity.normalized * maxSpeed;
        }
    }

    public void ChangeSpeedAndAcceleration(float newSpeed, float newAcceleration)
    {
        acceleration = newAcceleration;
        maxSpeed = newSpeed;
        navigation.GetComponent<NavMeshAgent>().speed = maxSpeed + 2.0f;
        navigation.GetComponent<NavMeshAgent>().acceleration = acceleration;
    }

    public void ResetSpeedAndAcceleration()
    {
        maxSpeed = standardSpeed;
        acceleration = standardAcceleration;
        navigation.GetComponent<NavMeshAgent>().speed = maxSpeed + 2.0f;
        navigation.GetComponent<NavMeshAgent>().acceleration = acceleration;
    }
}
