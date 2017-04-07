using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TP_RigidBodyController : MonoBehaviour {

    public static Rigidbody m_rigidBodyController;
    public static TP_RigidBodyController m_instance;

    private Vector3 m_forceToAdd;
    private void Awake()
    {
        m_rigidBodyController = GetComponent<Rigidbody>();
        m_instance = this;
    }
    
	// Update is called once per frame
	void Update () {
        // New update reset forcevector
        m_forceToAdd = Vector3.zero;
	}



    void GetLocomotionInput()
    {
        float t_deadZone = 0.1f;


        float t_vert = Input.GetAxis("Vertical");
        float t_hori = Input.GetAxis("Horizontal");

        if (t_vert > t_deadZone || t_vert < -t_deadZone)
        {
            m_forceToAdd += new Vector3(t_hori, 0, t_vert);
        }
    }

}
