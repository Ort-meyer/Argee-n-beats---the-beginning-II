using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleTrailManager : MonoBehaviour {

    Rigidbody rig;
    float reduceSpeed = 0.9f;

    // Use this for initialization
    void Start () {
        rig = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
        int numchild = transform.childCount;
            
        for (int i = 0; i < numchild; i++)
        {
            TrailRenderer trailRend = transform.GetChild(i).GetComponent<TrailRenderer>();
            if (trailRend && rig.velocity.magnitude > 0.0f)
            {
                Vector3 norm = trailRend.transform.position - transform.position;

                float dotVal = Vector3.Dot(norm, rig.velocity.normalized);

                if (Mathf.Abs(dotVal) < 0.25f )
                {
                    trailRend.time = (0.25f - Mathf.Abs(dotVal))*6.0f;
                }
                else
                {
                    trailRend.time *= reduceSpeed;
                }
            }
            else if(trailRend)
            {
                trailRend.time *= reduceSpeed;
            }
        }
	}
}
