using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CollectPickUpsAndCheckGoal : MonoBehaviour {
    int m_maxCollectibles = 0;
    int m_howManyCollectibles = 0;

    void Start()
    {
        GameObject[] collectebles = GameObject.FindGameObjectsWithTag("Pickups");
        m_maxCollectibles = collectebles.Length;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Pickups")
        {
            other.gameObject.SetActive(false);
            m_howManyCollectibles++;

        }
        if (other.gameObject.tag == "Goal" && m_howManyCollectibles >= m_maxCollectibles)
        {
            // output canvas stuff
            GameObject canvas = GameObject.Find("Canvas");
            for (int i = 0; i < canvas.transform.childCount; i++)
            {
                if (canvas.transform.GetChild(i).name.Equals("YOUWON"))
                {
                    canvas.transform.GetChild(i).GetComponent<Text>().enabled = true;
                }
            }
        }
    }
}
