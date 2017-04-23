using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CollectPickUpsAndCheckGoal : MonoBehaviour {
    int m_maxCollectibles = 0;
    int m_howManyCollectibles = 0;

    bool hudstarted = false;

    GameObject hudObject;

    void Start()
    {
        GameObject[] collectebles = GameObject.FindGameObjectsWithTag("Pickups");
        m_maxCollectibles = collectebles.Length;
        GameObject canvas = GameObject.Find("Canvas");
        for (int i = 0; i < canvas.transform.childCount; i++)
        {
            if (canvas.transform.GetChild(i).name.Equals("CRYSTALS"))
            {
                hudObject = canvas.transform.GetChild(i).gameObject;
                break;
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Pickups")
        {
            other.gameObject.SetActive(false);
            m_howManyCollectibles++;
            UpdateText();

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

    public void StartHUD()
    {
        if (hudstarted == false)
        {
            hudstarted = true;

            hudObject.GetComponent<Text>().enabled = true;
            UpdateText();

        }
    }

    void UpdateText()
    {
        hudObject.GetComponent<Text>().text = "Crystals: " + m_howManyCollectibles.ToString() + "/" + m_maxCollectibles.ToString();
    }
}
