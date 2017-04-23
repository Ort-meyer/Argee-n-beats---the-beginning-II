using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitShootScript : MonoBehaviour
{

    // Use this for initialization
    public float m_shootForce = 5000.0f;
    public float m_shootCooldown = 1.0f;
    public float m_shootDirTiltFromCamera = 15.0f;
    float m_timer = 0.0f;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        m_timer += Time.deltaTime;

        if (Input.GetKey(KeyCode.Mouse0) && m_timer > m_shootCooldown)
        {

            /// Get all objects that are in orbit
            /*
            What we do now is find all objects, then check if they're in orbit. This probably scales VERY badly with many
            objects. An alternative solution would be for objects to put a reference to themselves in a list here when they
            enter orbit, and remove themselves from that list if they leave the object.*/

            // Object that we will fire
            DymanicObjectScript objComponent = null;
            float bestDot = -1; // Change this so stuff behind never fires
            GameObject[] allDynamicObjects = GameObject.FindGameObjectsWithTag("DynamicObject");
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
                        Vector3 vecTargetNorm = transform.forward.normalized;
                        float thisDot = Vector3.Dot(vecBetweenNorm, vecTargetNorm);
                        if (thisDot > bestDot)
                        {
                            bestDot = thisDot;
                            // USELESS COMMENT. REMOVE
                            id = i;
                        }
                    }
                }
            }

            // We have the object to be fired (if any). Now fire it
            if (id != -1)
            {

                m_timer = 0.0f;
                objComponent = allDynamicObjects[id].GetComponent<DymanicObjectScript>();
                // Move it up so it doesn't hit the ground. Not sure if needed
                allDynamicObjects[id].transform.position += new Vector3(0, 0.5f, 0);
                allDynamicObjects[id].transform.gameObject.layer = 11;
                // This comp will not be null if we get here. Danger zone!
                objComponent.m_targetableForPower = false;
                objComponent.m_inOrbit = false;

                // Reset velocity, remove gravity and fire away! (this is where we'll maybe want it to be a rocket)
                Rigidbody fireObjBody = allDynamicObjects[id].GetComponent<Rigidbody>();
                fireObjBody.useGravity = false;
                fireObjBody.velocity = Vector3.zero;


                //Vector3 fireDirection = transform.forward; // This will be changed if we want auto aim



                //Vector3 fireDirection = Camera.main.transform.forward;
                // Rotate fire vector so we get a more natural/sensible fire direction
                //fireDirection = Quaternion.AngleAxis(-m_shootDirTiltFromCamera, Vector3.Cross(transform.up, transform.forward)) * fireDirection;

                Vector3 fireDirection = GetFireDirection(allDynamicObjects[id].transform.position);

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
            }

        }
    }
    
    Vector3 GetFireDirection(Vector3 p_posOfObjectBeingFired)
    {
        LayerMask mask;
        // Ignore stuff in orbit and fired projectiles (could be made public to be scalable and all that good stuff)
        mask = (1 << 11);
        mask |= (1 << 12);
        mask |= (1 << 8);
        mask = ~mask;
        Vector3 fireDirection = Camera.main.transform.forward;
        // Rotate fire vector so we get a more natural/sensible fire direction
        fireDirection = Quaternion.AngleAxis(-m_shootDirTiltFromCamera, Vector3.Cross(transform.up, transform.forward)) * fireDirection;
        RaycastHit rayHit;
        if (Physics.Raycast(transform.position, fireDirection, out rayHit, 1000, mask)) // Hard coded range of ray cast
        {
            fireDirection = (rayHit.point - p_posOfObjectBeingFired).normalized;
        }
        return fireDirection;
    }
}
