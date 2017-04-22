using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalPickup : MonoBehaviour {

    public LayerMask collisionMask;
    public float collisionExtent = 5;

    bool hasPickup = false;
    GameObject pickUpItem = null;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown(KeyCode.E))
        {
            // If we have something
            if (hasPickup)
            {
                // Let it gooo, let it gooo! Can't hold it back anymore
                Collider[] cols = pickUpItem.GetComponentsInChildren<Collider>();
                foreach (var item in cols)
                {
                    item.enabled = true;
                }

                Rigidbody rig = pickUpItem.GetComponent<Rigidbody>();
                rig.isKinematic = false;

                pickUpItem.transform.transform.position = transform.position + transform.forward * 1.5f;

                // set parent
                pickUpItem.transform.parent = null;

                hasPickup = false;
                pickUpItem = null;

            }
            else
            {
                // Check something close
                GameObject gameObject = FindClosestCrystal();
                if (gameObject != null)
                {
                    hasPickup = true;
                    pickUpItem = gameObject.transform.parent.gameObject;

                    Collider[] cols = pickUpItem.GetComponentsInChildren<Collider>();
                    foreach (var item in cols)
                    {
                        item.enabled = false;
                    }

                    Rigidbody rig = pickUpItem.GetComponent<Rigidbody>();
                    rig.isKinematic = true;
                    
                    // set parent
                    pickUpItem.transform.parent = transform;
                    pickUpItem.transform.transform.position = transform.position - transform.forward * 1.0f;
                }
            }
        }
	}

    GameObject FindClosestCrystal()
    {
        GameObject closest = null;
        Collider[] col = Physics.OverlapBox(transform.position, new Vector3(collisionExtent, collisionExtent, collisionExtent), Quaternion.identity, collisionMask);
        if (col.Length > 0)
        {
            closest = col[0].gameObject;
            float dist = (transform.position - closest.transform.position).magnitude;

            for (int i = 1; i < col.Length; i++)
            {
                float newDist = (transform.position - col[i].transform.position).magnitude;
                if (newDist < dist)
                {
                    dist = newDist;
                    closest = col[i].gameObject;
                }
            }
        }

        return closest;
    }
}
