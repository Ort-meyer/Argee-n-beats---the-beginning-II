using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitGameobjectScript : MonoBehaviour {

    // Use this for initialization
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
                //Set target gameobject pull force towards this gameobject.
                if(t_gameObject.GetComponent<Rigidbody>().velocity.magnitude<10)
                {
                    t_gameObject.GetComponent<Rigidbody>().AddForce((gameObject.transform.position-t_gameObject.GetComponent<Transform>().position)*10);
                }
            }
            else if(t_gameObject.name == "OrbitControl")
            {
                //t_gameObject.GetComponent<Transform>.
            }
            //Transform t_tempTrans = GetComponentInChildren<Transform>();

        }
        print(gameObject.transform.GetChild(0).name);
        Transform t_tempTrans = gameObject.transform.GetChild(0).transform.RotateAround(gameObject.transform, 10f);
        //gameObject.transform.position += new Vector3(0,0, 0.1f);
    }

}
