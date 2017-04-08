using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbiterTEST : MonoBehaviour {

    public GameObject m_target;

    //public float distance = 2;
    //public float speed = 1;
    //public float orbitalAngleX = 0;
    //public float orbitalAngleY = 0;
    //
    //private float progress = 0;
    //
    //private Vector3 orbitalVectorStart = new Vector3(0,0,1);
    //private Vector3 orbitalVectorUp = new Vector3(0, 1, 0);

    public float m_anglePerFrame = 10;

    float m_angle = 0;
	// Use this for initialization
	void Start ()
    {

        //orbitalVectorStart = Quaternion.AngleAxis(orbitalAngleX, new Vector3(1, 0, 0)) * orbitalVectorStart;
        //orbitalVectorUp = Quaternion.AngleAxis(orbitalAngleX, new Vector3(1, 0, 0)) * orbitalVectorUp;
    }
	
	// Update is called once per frame
	void Update ()
    {
       // m_angle += m_anglePerFrame;
        transform.RotateAround(m_target.transform.position, new Vector3(0, 1, 0), m_anglePerFrame);
        //transform.RotateAround(m_target.transform.position, new Vector3(0, 1, 0), m_anglePerFrame);
        //transform.Rotate(new Vector3(0, 1, 0), m_angle);
        //orbitalVectorStart = Quaternion.AngleAxis(orbitalAngleX, new Vector3(1, 0, 0)) * new Vector3(0,0,1);
        //orbitalVectorUp = Quaternion.AngleAxis(orbitalAngleX, new Vector3(1, 0, 0)) * new Vector3(0,1,0);
        //progress += speed;
        //if (progress > 360)
        //{
        //    progress = 360 - progress;
        //}
        //Vector3 orbitalVector = Quaternion.AngleAxis(progress, orbitalVectorUp) * orbitalVectorStart;
        //transform.position = m_target.transform.position + orbitalVector;
    }
}
