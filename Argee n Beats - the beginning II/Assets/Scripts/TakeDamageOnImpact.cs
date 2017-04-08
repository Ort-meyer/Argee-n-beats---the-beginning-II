using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeDamageOnImpact : MonoBehaviour {
    public float impactSpeedThreshold;
    [Tooltip("Per speed unit over impact speed")]
    public float damage;
    [Tooltip("Which impact tags to ignore")]
    public List<string> ignoreTags = new List<string>();
    [Tooltip("If the ignore tags should be used, mainly used by scripts")]
    public bool useIgnoreTags = false;

    private Vector3 lastVelocity;
    // Use this for initialization
    void Start() {
        print(Vector3.Dot(new Vector3(1, 0, 0), new Vector3(-1, 0, 0)));
    }

    // Update is called once per frame
    void Update() {
        
    }

    void FixedUpdate()
    {
        lastVelocity = GetComponent<Rigidbody>().velocity;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (useIgnoreTags)
        {
            string tag = collision.gameObject.tag;
            if (ignoreTags.Contains(tag))
            {
                return;
            }
        }
        float magnitude = collision.relativeVelocity.magnitude;

        if (magnitude > this.impactSpeedThreshold)
        {
            // Making sure we dont hit the ground, or something that isnt sloping enough
            bool hitWall = true;
            if (lastVelocity.magnitude >= this.impactSpeedThreshold)
            {
                hitWall = false;
                foreach (var item in collision.contacts)
                {
                    if (Mathf.Abs(Vector3.Dot(item.normal, lastVelocity.normalized)) > 0.4f)
                    {
                        hitWall = true;
                    }
                }

            }
            if (hitWall)
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
}
