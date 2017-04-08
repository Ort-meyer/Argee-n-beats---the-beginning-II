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
    public float m_radius = 5.0f;

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
        orbiScript = (OrbitGameobjectScript)GameObject.Find("PlayerCaps").GetComponent("OrbitGameobjectScript");
        Transform t_targetTransform = GameObject.Find("PlayerCaps").GetComponent<Transform>();
        if (orbiScript.m_usingDragAbility)
        {
                Vector3 t_vectorBetween = -1*(gameObject.transform.position - t_targetTransform.position);
                Vector3 t_crossResultNorm = Vector3.Cross(t_targetTransform.up, t_vectorBetween).normalized;
            m_goalPos = t_crossResultNorm * m_radius + t_targetTransform.position;
            if(t_vectorBetween.magnitude > m_prevVecBetween.magnitude)//|| t_vectorBetween.magnitude < m_radius)
            {
                m_enterOrbit = true;
                m_gravForce = Mathf.Pow(gameObject.GetComponent<Rigidbody>().velocity.magnitude, 2f) / t_vectorBetween.magnitude;

            }
            if(m_enterOrbit)
            {
                //Kontrollera orbit. försöka
                // typ reducera kraften på spheren.
                // sätt ny target vecotr.
                //float t_gravForce = Mathf.Pow(gameObject.GetComponent<Rigidbody>().velocity.magnitude,2)/ t_vectorBetween.magnitude;
                //GameObject.Find("GoalTarget").GetComponent<Transform>().position

                //gameObject.GetComponent<Rigidbody>().AddForce((GameObject.Find("GoalTarget").GetComponent<Transform>().position - gameObject.transform.position) * gameObject.GetComponent<Rigidbody>().velocity.magnitude*2);
                if(t_vectorBetween.magnitude<m_radius)
                {
                    print(t_vectorBetween.magnitude);
                    //print(t_vectorBetween.magnitude / m_radius);
                    //print((Mathf.Log10(t_vectorBetween.magnitude / m_radius)));
                    //print((1 * (t_targetTransform.position - gameObject.transform.position) * (Mathf.Log10(t_vectorBetween.magnitude / m_radius)) * 1000).magnitude);
                    //gameObject.GetComponent<Rigidbody>().AddForce((gameObject.transform.position - t_targetTransform.position) * (-1*Mathf.Log10(t_vectorBetween.magnitude/m_radius)));
                    //den bragameObject.GetComponent<Rigidbody>().AddForce(1*(t_targetTransform.position - gameObject.transform.position) * (Mathf.Log10(t_vectorBetween.magnitude/m_radius))*150);
                    gameObject.GetComponent<Rigidbody>().AddForce((t_targetTransform.position - gameObject.transform.position) * gameObject.GetComponent<Rigidbody>().velocity.magnitude*2);
                    //gameObject.GetComponent<Rigidbody>().AddForce((t_targetTransform.position - gameObject.transform.position) * gameObject.GetComponent<Rigidbody>().velocity.magnitude*2);
                }
                else
                {
                    gameObject.GetComponent<Rigidbody>().AddForce((t_targetTransform.position - gameObject.transform.position) * gameObject.GetComponent<Rigidbody>().velocity.magnitude*2);
                }

            }
            else
            {
                gameObject.GetComponent<Rigidbody>().AddForce((m_goalPos - gameObject.transform.position).normalized * 10);

            }

            m_prevVecBetween = t_vectorBetween;
           // Debug.DrawLine(gameObject.transform.position, m_goalPos);

        }
        else
        {
            m_enterOrbit = false;
        }
        //}
    }
}
