using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DymanicObjectScript : MonoBehaviour {

    OrbitGameobjectScript orbiScript;
    Vector3 m_goalPos;
    bool m_needToSetGoalPos = true;
    bool m_enterOrbit = false;
    //Gigantisk för att den alltid ska vara större än något
    Vector3 m_prevVecBetween = new Vector3(10000, 10000, 10000);
    float m_gravForce = 1.0f;
    public float m_radius = 15.0f;
    float m_veloMaxSpeed = 25.0f;
    float m_veloMultiplier = 2.0f;
    float m_radiusForOutPusher = 3.0f;
    // Use this for initialization
    void Start ()
    {
        
    }

	
	// Update is called once per frame
	void Update ()
    {
        ////Kollar alla spelarna om Co-op skulle vara en grej
        //for (int i = 0; i < length; i++)
        //{
        // Om spelaren använder kraften att dra åt sig saker i orbit.
        orbiScript = (OrbitGameobjectScript)GameObject.Find("PlaceHolderPlayerController").GetComponent("OrbitGameobjectScript");
        Transform t_targetTransform = GameObject.Find("PlaceHolderPlayerController").GetComponent<Transform>();
        Rigidbody t_targetRigid = GameObject.Find("PlaceHolderPlayerController").GetComponent<Rigidbody>();
        Vector3 t_totalForce = new Vector3();
        Vector3 t_vectorBetween = -1*(gameObject.transform.position - t_targetTransform.position);
        Vector3 t_crossResultNorm = Vector3.Cross(t_targetTransform.up, t_vectorBetween).normalized;
        m_goalPos = t_crossResultNorm * m_radius + t_targetTransform.position;

        if (orbiScript.m_usingDragAbility)
        {
            if(t_vectorBetween.magnitude > m_prevVecBetween.magnitude)//|| t_vectorBetween.magnitude < m_radius)
            {
                m_enterOrbit = true;
                m_gravForce = Mathf.Pow(gameObject.GetComponent<Rigidbody>().velocity.magnitude, 2f) / t_vectorBetween.magnitude;
            }

            if (m_enterOrbit)
            {
                if (t_vectorBetween.magnitude<m_radius)
                {
                    m_enterOrbit = false;
             
                    t_totalForce += (m_goalPos - gameObject.transform.position) * gameObject.GetComponent<Rigidbody>().velocity.magnitude * m_veloMultiplier;
                    // den nya bra 3,0t_totalForce += (t_targetTransform.position - gameObject.transform.position) * gameObject.GetComponent<Rigidbody>().velocity.magnitude * m_veloMultiplier;
                    //DEN nyabra 2.0gameObject.GetComponent<Rigidbody>().AddForce((t_targetTransform.position - gameObject.transform.position) * gameObject.GetComponent<Rigidbody>().velocity.magnitude* m_veloMultiplier);
              
                }
                else
                {
                    t_totalForce += (m_goalPos - gameObject.transform.position) * gameObject.GetComponent<Rigidbody>().velocity.magnitude * m_veloMultiplier;
                   m_enterOrbit = true;
                }

            }
            else
            {
                t_totalForce += (m_goalPos - gameObject.transform.position) * m_veloMultiplier;

            }

            m_prevVecBetween = t_vectorBetween;
        }
        else
        {
            m_enterOrbit = false;
        }
        if ((gameObject.GetComponent<Rigidbody>().velocity - t_targetRigid.velocity).magnitude > m_veloMaxSpeed)
            {
            float t_veloFactor = (gameObject.GetComponent<Rigidbody>().velocity - t_targetRigid.velocity).magnitude / m_veloMaxSpeed;
            t_totalForce += -1 * (gameObject.GetComponent<Rigidbody>().velocity);
        }
        float t_distBetween = Vector3.Distance(gameObject.transform.position, t_targetTransform.position);
        if (t_distBetween <= m_radiusForOutPusher)
        {
            float t_asd = 1 - (t_distBetween / m_radiusForOutPusher);
            ////gameObject.GetComponent<Rigidbody>().velocity.normalized
            ////Vector3 t_splitVelo = gameObject.GetComponent<Rigidbody>().velocity;
            ////gameObject.GetComponent<Rigidbody>().velocity = t_splitVelo - Vector3.Dot(t_vectorBetween.normalized, t_splitVelo) * t_vectorBetween.normalized;
            t_totalForce -= (t_targetTransform.position - gameObject.transform.position).normalized * 5000 * t_asd;
        }
        if(gameObject.GetComponent<Rigidbody>().velocity.magnitude > m_veloMaxSpeed)
        {
            gameObject.GetComponent<Rigidbody>().velocity = gameObject.GetComponent<Rigidbody>().velocity.normalized * m_veloMaxSpeed;
        }
            print(-1 * (gameObject.GetComponent<Rigidbody>().velocity).magnitude);
            gameObject.GetComponent<Rigidbody>().AddForce(t_totalForce);
    }
}
