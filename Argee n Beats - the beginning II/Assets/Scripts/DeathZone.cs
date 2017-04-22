using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeathZone : MonoBehaviour
{
    GameObject youDiedObj;
    // Use this for initialization
    void Start()
    {
        youDiedObj = GameObject.Find("YOUDIED");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void DisableHealthGUI()
    {
        youDiedObj.gameObject.GetComponent<Text>().enabled = false;
    }

    void OnTriggerEnter(Collider other)
    {
        Health healtComp = other.GetComponent<Health>();
        
        if (healtComp)
        {
            healtComp.TakeDamage(healtComp.startingHealth);

            if (other.tag.Equals("Player"))
            {
                youDiedObj.gameObject.GetComponent<Text>().enabled = true;
                Invoke("DisableHealthGUI", 3.0f);
            }
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

