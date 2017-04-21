using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DymanicObjectScript : MonoBehaviour
{
    GameObject m_playerObject;
    OrbitGameobjectScript orbiScript;
    Vector3 m_goalPos;
    bool m_needToSetGoalPos = true;

    //Här sätts den när den flyger iväg så man itne drar in den igen. Får sätta den när den träffar något eller vad simon tycker
    public bool m_targetableForPower = true;
    //Är det dags för den att entra orbit.
    bool m_enterOrbit = false;
    //Om den är i orbit och kan skjutas iväg. Om den är tillräcklgit nära spelaren.
    public bool m_inOrbit = false;
    //Sätter denna från ett annat script så den skjuts åt den riktningen hela tiden.
    public bool m_addingForceToIt = false;
    //Gigantisk för att den alltid ska vara större än något
    Vector3 m_prevVecBetween = new Vector3(10000, 10000, 10000);
    float m_gravForce = 1.0f;
    public float m_radius = 15.0f;
    float m_veloMaxSpeed = 25.0f;
    float m_veloMultiplier = 2.0f;
    float m_radiusForOutPusher = 3.0f;
    float m_radiusForDetermineInOrbit = 0.0f;
    public float m_pullRange = 20.0f;
    // Use this for initialization
    void Start()
    {
        m_radiusForDetermineInOrbit = m_radiusForOutPusher + 5.0f;
        m_playerObject = GameObject.Find("PlaceHolderPlayerController");
    }


    // Update is called once per frame
    void Update()
    {
        ////Kollar alla spelarna om Co-op skulle vara en grej
        //for (int i = 0; i < length; i++)
        //{
        // Om spelaren använder kraften att dra åt sig saker i orbit.
        orbiScript = (OrbitGameobjectScript)GameObject.Find("PlaceHolderPlayerController").GetComponent("OrbitGameobjectScript");
        Transform t_targetTransform = GameObject.Find("PlaceHolderPlayerController").GetComponent<Transform>();
        Rigidbody t_targetRigid = GameObject.Find("PlaceHolderPlayerController").GetComponent<Rigidbody>();
        Vector3 t_totalForce = new Vector3();
        Vector3 t_vectorBetween = -1 * (gameObject.transform.position - t_targetTransform.position);
        Vector3 t_crossResultNorm = Vector3.Cross(t_targetTransform.up, t_vectorBetween).normalized;
        m_goalPos = t_crossResultNorm * m_radius + t_targetTransform.position;
        ShooterPuller shootScript = m_playerObject.GetComponent<ShooterPuller>();

        if (m_targetableForPower)
        {


            if (shootScript.m_pullingIntoOrbit && t_vectorBetween.magnitude < m_pullRange)
            {
                GetComponent<Rigidbody>().useGravity = false;
                if (t_vectorBetween.magnitude > m_prevVecBetween.magnitude)//|| t_vectorBetween.magnitude < m_radius)
                {
                    m_enterOrbit = true;
                    
                    m_gravForce = Mathf.Pow(gameObject.GetComponent<Rigidbody>().velocity.magnitude, 2f) / t_vectorBetween.magnitude;
                }

                if (m_enterOrbit)
                {
                    transform.gameObject.layer = 12;
                    if (t_vectorBetween.magnitude < m_radius)
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
                GetComponent<Rigidbody>().useGravity = true;
                m_enterOrbit = false;
                transform.gameObject.layer = 1;
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
            if (gameObject.GetComponent<Rigidbody>().velocity.magnitude > m_veloMaxSpeed)
            {
                gameObject.GetComponent<Rigidbody>().velocity = gameObject.GetComponent<Rigidbody>().velocity.normalized * m_veloMaxSpeed;
            }
            //print(-1 * (gameObject.GetComponent<Rigidbody>().velocity).magnitude);
            gameObject.GetComponent<Rigidbody>().AddForce(t_totalForce);
            if (t_distBetween <= m_radiusForDetermineInOrbit * 1.5)
            {
                m_inOrbit = true;
            }
            else
            {
                m_inOrbit = false;
            }
        }


    }
}
