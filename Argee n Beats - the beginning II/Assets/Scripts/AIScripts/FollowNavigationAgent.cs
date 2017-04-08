using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FollowNavigationAgent : MonoBehaviour {
    public GameObject navigation;
    public float acceleration; // Units/second
    public float maxSpeed; // units/second
    private Vector3 totalForce;
	// Use this for initialization
	void Start () {
        navigation.GetComponent<NavMeshAgent>().speed = maxSpeed+2.0f;
        navigation.GetComponent<NavMeshAgent>().acceleration = acceleration;
        AttackBase attacker = GetComponent<AttackBase>();
        if (attacker != null)
        {
            navigation.GetComponent<NavMeshAgent>().stoppingDistance = attacker.attackDistance;
        }
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
        if (direction != Vector3.zero)
        {
            totalForce += direction * acceleration * Time.deltaTime;
        }
        if (distance < 0.3)
        {
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            totalForce = Vector3.zero;
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            GetComponent<MovementManager>().AddImpulse(-direction * 20 + new Vector3(0,20,0));
        }
	}

    void FixedUpdate()
    {
        Rigidbody myBody = GetComponent<Rigidbody>();
        
        myBody.AddForce(totalForce, ForceMode.VelocityChange);
        
        totalForce = Vector3.zero;
        if (myBody.velocity.magnitude > maxSpeed)
        {
            myBody.velocity = myBody.velocity.normalized * maxSpeed;
        }
    }
}
