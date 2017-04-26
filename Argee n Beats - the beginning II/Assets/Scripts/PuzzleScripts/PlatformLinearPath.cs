using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformLinearPath : MonoBehaviour {
    int initTimes = 0;

    public Rigidbody o_rigidbody;
    public bool useForce = false;
    public float speedF = 100;
    Vector3 momentum = Vector3.zero;

    public Transform[] paths_t;
    Transform currPath = null;
    int currIndex = -1;
    // Use this for initialization
    void Start() {
        if (o_rigidbody == null)
        {
            o_rigidbody = GetComponent<Rigidbody>();
        }

        if (useForce)
        {
            o_rigidbody.isKinematic = false;
        }

        initTimes++;
    }

    // Update is called once per frame
    void FixedUpdate() {

        if (initTimes == 0 || currPath == null)
            return;

        if (Vector3.Distance(o_rigidbody.position, currPath.position) < 0.4f)
        {
            o_rigidbody.velocity = o_rigidbody.velocity * 0.4f; //slöa ned den
            return;
        }

        Vector3 dir = (currPath.position - o_rigidbody.position).normalized;

        if (!useForce)
        {
            momentum = Vector3.Lerp(momentum, dir, Time.fixedDeltaTime * 3);

            o_rigidbody.MovePosition(o_rigidbody.position + momentum * Time.fixedDeltaTime * speedF); //flytta den från den gamla positionen
        }
        else
        {
            o_rigidbody.isKinematic = false;
            o_rigidbody.AddForce(dir * speedF);
        }
    }

    public void MoveTo(int index)
    {
        if (currIndex == index)
        {
            return;
        }
        currIndex = index;
        currPath = paths_t[currIndex];
        momentum = Vector3.zero;
    }

    public bool ReachedCurrentPath()
    {
        if (currPath == null)
        {
            return true;
        }
        float distance = (transform.position - currPath.position).magnitude;
        return distance < 0.5; // A epsilon
    }

    public void StopMove()
    {
        currIndex = -1;
        currPath = null;
        momentum = Vector3.zero;
        return;    
    }
}
