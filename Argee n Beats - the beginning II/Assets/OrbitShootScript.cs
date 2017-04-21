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
        //Transform t_targetTransform = GameObject.Find("PlaceHolderPlayerController").GetComponent<Transform>();
        //Kolla vilka som är framför gubben med en ray eller en fyrkant. 
        //m_ray = new Ray(t_targetTransform.position, t_targetTransform.forward);
        //m_rayCastHits = Physics.RaycastAll(m_ray.origin, m_ray.direction, 100.0f);
        //int rayCastHitslength = m_rayCastHits.Length;
        //if (m_shootCooldown < m_timer)
        //{
        //Debug.DrawLine(m_ray.origin, m_ray.origin + m_ray.direction * 100.0f);

        //for (int i = 0; i < rayCastHitslength; i++)
        //{
        // Vill bräjka forloopen om vi har skjutit den här updaten
        // if (m_rayCastHits[i].transform.gameObject.tag == "DynamicObject")// && !m_shotThisUpdate)
        //{
        //GameObject thisObj = m_rayCastHits[i].transform.gameObject;
        //m_dynamicObjectScript = (DymanicObjectScript)m_rayCastHits[i].transform.gameObject.GetComponent("DymanicObjectScript");
        //Kolla om objektet är in orbit så vi kan använda det som projektil.
        //if (m_dynamicObjectScript.m_inOrbit && GetComponent<ShooterPuller>().m_shootingFromOrbit)// && m_dynamicObjectScript.m_targetableForPower)
        //{

        if (Input.GetKey(KeyCode.Q) && m_timer > m_shootCooldown)
        {

            /// Get all objects that are in orbit
            /*
            What we do now is find all objects, then check if they're in orbit. This probably scales VERY badly with many
            objects. An alternative solution would be for objects to put a reference to themselves in a list here when they
            enter orbit, and remove themselves from that list if they leave the object.*/

            // Object that we will fire
            DymanicObjectScript objComponent = null;
            //GameObject fireObject = null; // Really? This is explicit?
            float bestDot = -1; // Change this so stuff behind never fires
            GameObject[] allDynamicObjects = GameObject.FindGameObjectsWithTag("DynamicObject");
            //foreach (GameObject obj in allDynamicObjects)
            int id = -1;
            for(int i = 0; i < allDynamicObjects.Length; i++)
            {
                objComponent = allDynamicObjects[i].GetComponent<DymanicObjectScript>();
                if (objComponent != null)
                {

                    // Get the one with best dot product (most in front of player). If that is within fire arc, fire 
                    if (objComponent.m_inOrbit)
                    {
                        Vector3 vecBetweenNorm = (allDynamicObjects[i].transform.position - transform.position).normalized;
                        //Vector3 vecTargetNorm = Camera.current.transform.forward.normalized;
                        Vector3 vecTargetNorm = transform.forward.normalized;
                        float thisDot = Vector3.Dot(vecBetweenNorm, vecTargetNorm);
                        if (thisDot > bestDot)
                        {
                            id = i;
                            //fireObject = obj; // This is reference, right?
                        }
                    }
                }
            }

            // We have the object to be fired (if any). Now fire it
            if (id != -1)// fireObject != null)
            {

                m_timer = 0.0f;
                objComponent = allDynamicObjects[id].GetComponent<DymanicObjectScript>();
                //DestroyObject(fireObject);
                // Move it up so it doesn't hit the ground. Not sure if needed
                allDynamicObjects[id].transform.position += new Vector3(0, 0.5f, 0);
                allDynamicObjects[id].transform.gameObject.layer = 11;
                m_shotThisUpdate = true;
                // This comp will not be null if we get here. Danger zone!
                objComponent.m_targetableForPower = false;
                objComponent.m_inOrbit = false;

                // Reset velocity, remove gravity and fire away! (this is where we'll maybe want it to be a rocket)
                Rigidbody fireObjBody = allDynamicObjects[id].GetComponent<Rigidbody>();
                fireObjBody.useGravity = true;
                fireObjBody.velocity = Vector3.zero;
                Vector3 fireDirection = transform.forward; // This will be changed if we want auto aim
                fireDirection = Quaternion.AngleAxis(-20, Vector3.Cross(transform.up, transform.forward)) * fireDirection;
                fireObjBody.AddForce(fireDirection * m_shootForce);
                fireObjBody.GetComponent<Collider>().isTrigger = true; // why?
                allDynamicObjects[id].AddComponent<Projectile>();
                ProjectileInfo t_projInfo = allDynamicObjects[id].GetComponent<ProjectileInfo>();
                if (t_projInfo == null)
                {
                    print("Picked up something that doesn't have ProjectileInfo");
                }
                else
                {
                    Projectile proj = allDynamicObjects[id].GetComponent<Projectile>();
                    proj.radius = t_projInfo.radius;
                    proj.damage = t_projInfo.damage;
                    proj.damageRadius = t_projInfo.damageRadius;
                    proj.impulse = t_projInfo.impulse;
                    proj.explosionPrefab = t_projInfo.explosionPrefab;
                }

                //Destroy(allDynamicObjects[id].GetComponent<DymanicObjectScript>());

                //m_timer = 0.0f;
                ////DestroyObject(fireObject);
                //// Move it up so it doesn't hit the ground. Not sure if needed
                //fireObject.transform.position += new Vector3(0, 0.5f, 0);
                //fireObject.transform.gameObject.layer = 11;
                //m_shotThisUpdate = true;
                //// This comp will not be null if we get here. Danger zone!
                //objComponent.m_targetableForPower = false;
                //objComponent.m_inOrbit = false;

                //// Reset velocity, remove gravity and fire away! (this is where we'll maybe want it to be a rocket)
                //Rigidbody fireObjBody = fireObject.GetComponent<Rigidbody>();
                //fireObjBody.useGravity = true;
                //fireObjBody.velocity = Vector3.zero;
                //Vector3 fireDirection = transform.forward; // This will be changed if we want auto aim
                //fireDirection = Quaternion.AngleAxis(-20, Vector3.Cross(transform.up, transform.forward)) * fireDirection;
                //fireObjBody.AddForce(fireDirection * m_shootForce);
                //fireObjBody.GetComponent<Collider>().isTrigger = true; // why?
                //fireObject.AddComponent<Projectile>();
                //ProjectileInfo t_projInfo = fireObject.GetComponent<ProjectileInfo>();
                //if (t_projInfo == null)
                //{
                //    print("Picked up something that doesn't have ProjectileInfo");
                //}
                //else
                //{
                //    Projectile proj = fireObject.GetComponent<Projectile>();
                //    proj.radius = t_projInfo.radius;
                //    proj.damage = t_projInfo.damage;
                //    proj.damageRadius = t_projInfo.damageRadius;
                //    proj.impulse = t_projInfo.impulse;
                //    proj.explosionPrefab = t_projInfo.explosionPrefab;
                //}











                //m_rayCastHits[i].transform.GetComponent<Rigidbody>().AddForce(t_launchVector * m_shootForce);
                //m_rayCastHits[i].transform.GetComponent<Collider>().isTrigger = true;
                //m_rayCastHits[i].transform.gameObject.AddComponent<Projectile>();
                ////ProjectileInfo t_projInfo = m_rayCastHits[i].transform.GetComponent<ProjectileInfo>();
                //if (t_projInfo == null)
                //{
                //    print("Picked up something that doesn't have ProjectileInfo");
                //}
                //else
                //{
                //    Projectile proj = m_rayCastHits[i].transform.GetComponent<Projectile>();
                //    proj.radius = t_projInfo.radius;
                //    proj.damage = t_projInfo.damage;
                //    proj.damageRadius = t_projInfo.damageRadius;
                //    proj.impulse = t_projInfo.impulse;
                //    proj.explosionPrefab = t_projInfo.explosionPrefab;
                //}
            }

        }

        //thisObj.transform.position += new Vector3(0, 0.5f, 0);
        //m_rayCastHits[i].transform.gameObject.layer = 11;
        //m_timer = 0.0f;
        //m_shotThisUpdate = true;
        //m_dynamicObjectScript.m_targetableForPower = false;
        //m_dynamicObjectScript.m_inOrbit = false;
        //m_rayCastHits[i].rigidbody.useGravity = false;
        //m_rayCastHits[i].transform.GetComponent<Rigidbody>().velocity = Vector3.zero;
        //Skjuta iväg den.
        //Vector3 t_launchVector = Quaternion.AngleAxis(0, Vector3.Cross(transform.up, transform.forward)) * m_ray.direction;
        ////m_rayCastHits[i].transform.GetComponent<Rigidbody>().AddForce(m_ray.direction * m_shootForce);
        ////m_rayCastHits[i].transform.GetComponent<Rigidbody>().AddForce(new Vector3(0,1,0)* m_shootForce);
        //m_rayCastHits[i].transform.GetComponent<Rigidbody>().AddForce(t_launchVector * m_shootForce);
        //m_rayCastHits[i].transform.GetComponent<Collider>().isTrigger = true;
        //m_rayCastHits[i].transform.gameObject.AddComponent<Projectile>();
        ////ProjectileInfo t_projInfo = m_rayCastHits[i].transform.GetComponent<ProjectileInfo>();
        //if (t_projInfo == null)
        //{
        //    print("Picked up something that doesn't have ProjectileInfo");
        //}
        //else
        //{
        //    Projectile proj = m_rayCastHits[i].transform.GetComponent<Projectile>();
        //    proj.radius = t_projInfo.radius;
        //    proj.damage = t_projInfo.damage;
        //    proj.damageRadius = t_projInfo.damageRadius;
        //    proj.impulse = t_projInfo.impulse;
        //    proj.explosionPrefab = t_projInfo.explosionPrefab;
        //}

        //print(m_ray.direction * m_shootForce);
        //}
        //}
        // }
        //}

    }
}
