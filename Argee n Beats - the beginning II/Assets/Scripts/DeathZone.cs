using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathZone : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }


    void OnTriggerEnter(Collider other)
    {
        Health healtComp = other.GetComponent<Health>();
        
        if (healtComp)
        {
            healtComp.TakeDamage(healtComp.startingHealth);
        }

        if (other.transform.parent)
        {
            RespawnNoHealth respNoHealth = other.transform.parent.GetComponent<RespawnNoHealth>();
            if (respNoHealth)
            {
                respNoHealth.Respawn();
            }
        }
    }
}

