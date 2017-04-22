using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectPickUps : MonoBehaviour {
    public int m_howManyCollectibles = 0;
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Pickup")
        {
            other.gameObject.SetActive(false);
            m_howManyCollectibles++;

        }
    }
}
