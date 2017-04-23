using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pusher : MonoBehaviour {
    public bool activated = false;
    public bool pushAway = false;
    ForceMode forceMode = ForceMode.VelocityChange;
    GameObject m_playerGO;
    public float pushForce = 100;
    bool m_playerEligable = true;
    AudioSource audioSource;
    // Use this for initialization
    void Start ()
    {
        m_playerGO = GameObject.FindGameObjectWithTag("Player").gameObject;
        audioSource = GetComponent<AudioSource>();
    }
	void OnTriggerExit(Collider c)
    {
        if(c.gameObject==m_playerGO)
        {
            m_playerEligable = true;
        }
    }
    void OnTriggerStay(Collider c)
    {
        if (!activated)
            return;

        Rigidbody o_r = c.GetComponent<Rigidbody>();
        MovementManager o_m = c.GetComponent<MovementManager>();

        if (o_m != null) //fiende, använd speciell impuls!
        {
            float forceFactor = 0.005f;
            if (pushAway)
            {
                Vector3 tSameY = new Vector3(transform.position.x, c.transform.position.y, transform.position.z);
                Vector3 dir = (c.transform.position - tSameY).normalized;
                o_m.AddImpulse(dir * pushForce * forceFactor);
                //print(o_r.velocity);
            }
            else
            {
                o_m.AddImpulse(transform.up * pushForce * forceFactor);
                //print(o_r.velocity);
            }
        }
        else if (o_r != null)
        {
            if(pushAway)
            {
                Vector3 tSameY = new Vector3(transform.position.x, c.transform.position.y, transform.position.z);
                Vector3 dir = transform.parent.transform.up;//(c.transform.position - tSameY).normalized;
                //o_r.velocity = Vector3.zero;
                //print(o_r.velocity);
                o_r.AddForce(dir * pushForce, forceMode);


            }
            else
            {
                if(c.gameObject==m_playerGO && m_playerEligable)
                {
                    o_r.velocity = Vector3.zero;
                    //print(o_r.velocity);
                    o_r.AddForce(transform.up * pushForce, forceMode);
                    m_playerEligable = false;
                    audioSource.Play();
                }
            }
            
        }
    }
    
    public void Toggle(bool b)
    {
        activated = b;
    }
}
