using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour {

    public float m_burnTime = 1;
    public float m_velocity = 100;
    public Vector3 m_target;
    public bool m_homing = false;
    public GameObject m_targetObject;

	// Use this for initialization
	void Start () {
        GetComponent<Rigidbody>().useGravity = false;
    }
	
	// Update is called once per frame
	void Update ()
    {
        m_burnTime -= Time.deltaTime;
        Rigidbody t_thisBody = GetComponent<Rigidbody>();
        if (m_burnTime <=0)
        {
            t_thisBody.useGravity = true;
            Debug.Log("done");
            // No longer a rocket. Remove script
            Destroy(this);
        }
        
        Vector3 t_betweenVectorDir;
        if (m_homing)
        {
            t_betweenVectorDir = (m_targetObject.transform.position - transform.position).normalized;
        }
        else
        {
            t_betweenVectorDir = (m_target - transform.position).normalized;
        }

        //t_thisBody.AddForce(t_betweenVectorDir * m_forcePerSec * Time.deltaTime);
        t_thisBody.velocity = t_betweenVectorDir * m_velocity * Time.deltaTime;
    }
}
