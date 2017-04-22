using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolTipCheckpoint : MonoBehaviour {

    bool entered = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter(Collider other)
    {
        if (entered == false)
        {
            entered = true;
            transform.parent.GetComponent<ToolTipManager>().UpdateToolTip();
        }

    }
}
