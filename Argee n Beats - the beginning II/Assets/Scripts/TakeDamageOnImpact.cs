using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeDamageOnImpact : MonoBehaviour {
    public float imapctSpeedThreshold;
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
        
    }
}
