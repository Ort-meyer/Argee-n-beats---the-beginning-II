using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitShootScript : MonoBehaviour {

    // Use this for initialization
    Ray m_ray;
    RaycastHit[] m_rayCastHits;
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        // Ta spelarens SiktarVector
        Transform t_targetTransform = GameObject.Find("PlaceHolderPlayerController").GetComponent<Transform>();
        //Kolla vilka som är framför gubben med en ray eller en fyrkant. 
        m_ray = new Ray(t_targetTransform.position, t_targetTransform.forward);
        m_rayCastHits = Physics.RaycastAll(m_ray.origin, m_ray.direction, 20.0f);
        int rayCastHitslength = m_rayCastHits.Length;
        for (int i = 0; i < rayCastHitslength; i++)
        {
            if (m_rayCastHits[i].transform.gameObject.tag == "DynamicObject")
            {
                //print("Okejdå");
            }
        }
        //Kolla om objektet är in orbit så vi kan använda det som projektil.
        //Skjuta iväg den.

    }
}
