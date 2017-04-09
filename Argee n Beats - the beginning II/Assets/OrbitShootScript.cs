using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitShootScript : MonoBehaviour
{

    // Use this for initialization
    DymanicObjectScript m_dynamicObjectScript;
    Ray m_ray;
    RaycastHit[] m_rayCastHits;
    public float m_shootForce = 5000.0f;
    public float m_shootCooldown = 1.0f;
    float m_timer = 0.0f;
    bool m_shotThisUpdate = false;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //Sätter den som false så vi kan skjuta denna update.
        m_shotThisUpdate = false;
        //uppdatera cooldowntimer
        m_timer += Time.deltaTime;
        // Ta spelarens SiktarVector
        Transform t_targetTransform = GameObject.Find("PlaceHolderPlayerController").GetComponent<Transform>();
        //Kolla vilka som är framför gubben med en ray eller en fyrkant. 
        m_ray = new Ray(t_targetTransform.position, t_targetTransform.forward);
        m_rayCastHits = Physics.RaycastAll(m_ray.origin, m_ray.direction, 100.0f);
        int rayCastHitslength = m_rayCastHits.Length;
        if (m_shootCooldown < m_timer)
        {
            //Debug.DrawLine(m_ray.origin, m_ray.origin + m_ray.direction * 100.0f);

            for (int i = 0; i < rayCastHitslength; i++)
            {
                // Vill bräjka forloopen om vi har skjutit den här updaten
                if (m_rayCastHits[i].transform.gameObject.tag == "DynamicObject")// && !m_shotThisUpdate)
                {
                    GameObject thisObj = m_rayCastHits[i].transform.gameObject;
                    m_dynamicObjectScript = (DymanicObjectScript)m_rayCastHits[i].transform.gameObject.GetComponent("DymanicObjectScript");
                    //Kolla om objektet är in orbit så vi kan använda det som projektil.
                    if (m_dynamicObjectScript.m_inOrbit && GetComponent<ShooterPuller>().m_shootingFromOrbit)// && m_dynamicObjectScript.m_targetableForPower)
                    {
                        thisObj.transform.position += new Vector3(0, 0.5f, 0);
                        m_rayCastHits[i].transform.gameObject.layer = 11;
                        m_timer = 0.0f;
                        m_shotThisUpdate = true;
                        m_dynamicObjectScript.m_targetableForPower = false;
                        m_dynamicObjectScript.m_inOrbit = false;
                        m_rayCastHits[i].rigidbody.useGravity = false;
                        m_rayCastHits[i].transform.GetComponent<Rigidbody>().velocity = Vector3.zero;
                        //Skjuta iväg den.
                        Vector3 t_launchVector = Quaternion.AngleAxis(0, Vector3.Cross(transform.up, transform.forward)) * m_ray.direction;
                        //m_rayCastHits[i].transform.GetComponent<Rigidbody>().AddForce(m_ray.direction * m_shootForce);
                        //m_rayCastHits[i].transform.GetComponent<Rigidbody>().AddForce(new Vector3(0,1,0)* m_shootForce);
                        m_rayCastHits[i].transform.GetComponent<Rigidbody>().AddForce(t_launchVector * m_shootForce);
                        m_rayCastHits[i].transform.GetComponent<Collider>().isTrigger = true;
                        m_rayCastHits[i].transform.gameObject.AddComponent<Projectile>();
                        Projectile proj = m_rayCastHits[i].transform.GetComponent<Projectile>();
                        proj.radius = 40;
                        proj.damage = 10;
                        proj.damageRadius = 4;
                        proj.impulse = 60;
                        //print(m_ray.direction * m_shootForce);
                    }
                }
            }
        }

    }
}
