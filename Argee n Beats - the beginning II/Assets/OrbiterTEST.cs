using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbiterTEST : MonoBehaviour
{

    public GameObject m_target;
    public float m_orbitRadius;
    public float m_dragForce;



    private bool m_inOrbit;
    private Vector3 m_prevVecBetween = new Vector3(10000,10000,100000);
    
    
    // Use this for initialization
    void Start()
    {
        m_inOrbit = false;
        m_prevVecBetween = new Vector3(10000, 10000, 100000);
        //orbitalVectorStart = Quaternion.AngleAxis(orbitalAngleX, new Vector3(1, 0, 0)) * orbitalVectorStart;
        //orbitalVectorUp = Quaternion.AngleAxis(orbitalAngleX, new Vector3(1, 0, 0)) * orbitalVectorUp;
    }

    // Update is called once per frame
    void Update()
    {
        Transform t_targetTransform = m_target.transform;
        Rigidbody t_targetRigid =m_target.GetComponent<Rigidbody>();

        Rigidbody t_myBody = GetComponent<Rigidbody>();

        Vector3 t_vectorBetween = -1 * (gameObject.transform.position - t_targetTransform.position);
        Vector3 t_crossResultNorm = Vector3.Cross(t_targetTransform.up, t_vectorBetween).normalized;
        Vector3 t_goalPos = t_crossResultNorm * m_orbitRadius + t_targetTransform.position;

        Vector3 t_totalForce = new Vector3(0, 0, 0);

        if (t_vectorBetween.magnitude < m_orbitRadius * 1.1f)
        {
            m_inOrbit = true;
            //m_gravForce = Mathf.Pow(gameObject.GetComponent<Rigidbody>().velocity.magnitude, 2f) / t_vectorBetween.magnitude;
            Debug.Log("der");
        }

        if (m_inOrbit)
        {

            //if (t_vectorBetween.magnitude < m_radius)
            //{
            //    m_enterOrbit = false;
            //    //gameObject.GetComponent<Rigidbody>().AddForce((gameObject.transform.position - t_targetTransform.position) * (-1*Mathf.Log10(t_vectorBetween.magnitude/m_radius)));
            //    //den bragameObject.GetComponent<Rigidbody>().AddForce(1*(t_targetTransform.position - gameObject.transform.position) * (Mathf.Log10(t_vectorBetween.magnitude/m_radius))*150);
            //    gameObject.GetComponent<Rigidbody>().AddForce((t_targetTransform.position - gameObject.transform.position) * gameObject.GetComponent<Rigidbody>().velocity.magnitude * m_veloMultiplier);
            //    //gameObject.GetComponent<Rigidbody>().AddForce((t_targetTransform.position - gameObject.transform.position) * gameObject.GetComponent<Rigidbody>().velocity.magnitude*2);
            //}
            //else
            //{
            //    gameObject.GetComponent<Rigidbody>().AddForce((t_targetTransform.position - gameObject.transform.position) * gameObject.GetComponent<Rigidbody>().velocity.magnitude * m_veloMultiplier);
            //}

        }
        else
        {
            t_totalForce += (t_goalPos - gameObject.transform.position).normalized * m_dragForce;
        }
        t_myBody.AddForce(t_totalForce);
        m_prevVecBetween = t_vectorBetween;


        //if (gameObject.GetComponent<Rigidbody>().velocity.magnitude - t_targetTransform > m_veloMaxSpeed)
        //if (gameObject.GetComponent<Rigidbody>().velocity.magnitude > m_veloMaxSpeed)
        //{
        //    print("nu går det undan");
        //    float t_veloFactor = gameObject.GetComponent<Rigidbody>().velocity.magnitude / m_veloMaxSpeed;
        //    gameObject.GetComponent<Rigidbody>().AddForce(-1 * (gameObject.GetComponent<Rigidbody>().velocity / t_veloFactor));
        //}
        //}
    }
}
