using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectPickUpsAndCheckGoal : MonoBehaviour {
    public int m_howManyCollectibles = 0;
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Pickup")
        {
            other.gameObject.SetActive(false);
            m_howManyCollectibles++;

        }
        if (other.gameObject.name == "Goal" && m_howManyCollectibles >= 2)
        {
            print("Brajobbat");
        }
    }
}
