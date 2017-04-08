using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitGameobjectScript : MonoBehaviour {

    
    // Use this for initialization
    //float orbitRadiusCorrectionSpeed = 40.5f;
    //float orbitRoationSpeed = 100.0f;
    //float orbitRadius = 0.5f;
    //float orbitAlignToDirectionSpeed = 10.5f;
    //Vector3 previousPosition = new Vector3(10, 0, 0);
    public bool m_usingDragAbility = true;

    void Start () {

    }
	
	// Update is called once per frame
	void Update ()
    {
        object[] obj = GameObject.FindObjectsOfType(typeof(GameObject));
        foreach (object o in obj)
        {
            GameObject t_gameObject = (GameObject)o;
            if(t_gameObject.tag == "DynamicObject")
            {
                /////Vector3 t_orbitAxis = new Vector3(0, 1, 0);
                ///////Movement
                /////t_gameObject.transform.RotateAround(gameObject.transform.position, t_orbitAxis, orbitRoationSpeed * Time.deltaTime);
                /////Vector3 orbitDesiredPosition =(t_gameObject.transform.position - t_gameObject.transform.position).normalized * orbitRadius + t_gameObject.transform.position;
                /////t_gameObject.transform.position = Vector3.Slerp(t_gameObject.transform.position, orbitDesiredPosition, Time.deltaTime * orbitRadiusCorrectionSpeed);
                /////
                ///////Rotation
                /////Vector3 relativePos = t_gameObject.transform.position - previousPosition;
                /////Quaternion rotation = Quaternion.LookRotation(relativePos);
                /////t_gameObject.transform.rotation = Quaternion.Slerp(t_gameObject.transform.rotation, rotation, orbitAlignToDirectionSpeed * Time.deltaTime);
                /////previousPosition = t_gameObject.transform.position;


                //Set target gameobject pull force towards this gameobject.
                //if (t_gameObject.GetComponent<Rigidbody>().velocity.magnitude<10)
               // {
                    ////////////t_gameObject.GetComponent<Rigidbody>().AddForce((gameObject.transform.position-t_gameObject.GetComponent<Transform>().position).normalized*400);
                //}
                //else
                //{
                //    t_gameObject.GetComponent<Rigidbody>().AddForce((gameObject.transform.position - t_gameObject.GetComponent<Transform>().position));
                //}
            }
        
        
        }


        //print("GubbPOS" + gameObject.transform.position);
        //print("BOLLPOS" + gameObject.transform.GetChild(0).position);
        //Vector3 tempRotateTrans = gameObject.transform.GetChild(0).position - transform.position; 
        //tempRotateTrans = Quaternion.Euler(0,10 * Time.deltaTime,0) * tempRotateTrans;
        //gameObject.transform.GetChild(0).transform.position = tempRotateTrans;
    }

}
