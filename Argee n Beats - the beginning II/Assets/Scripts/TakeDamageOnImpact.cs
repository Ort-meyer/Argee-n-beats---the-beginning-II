using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeDamageOnImpact : MonoBehaviour {
    public float impactSpeedThreshold;
    [Tooltip("Per speed unit over impact speed")]
    public float damage;
    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    void OnCollisionEnter(Collision collision)
    {
        float magnitude = collision.relativeVelocity.magnitude;
        if (magnitude > this.impactSpeedThreshold)
        {
            float amountOver = magnitude - this.impactSpeedThreshold;
            Health health = GetComponent<Health>();
            if (health != null)
            {
                health.TakeDamage(damage * amountOver);
            }
        }
    }
}
