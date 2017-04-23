using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToolTipManager : MonoBehaviour {

    List<GameObject> toolTips = new List<GameObject>();
    GameObject toggle;
    int curToolTip = 0;
    bool finished = false;

	// Use this for initialization
	void Start () {
        GameObject canvas = GameObject.Find("Canvas");
        for (int i = 0; i < canvas.transform.childCount; i++)
        {
            GameObject child = canvas.transform.GetChild(i).gameObject;
            if (child.name.Contains("TOOLTIP"))
            {
                toolTips.Add(child);
            }
            else if(child.name.Contains("TOGGLE"))
            {
                toggle = child;
            }
        }

        // sort
        

        toolTips[0].GetComponent<Text>().enabled = true;
	}
	
	// Update is called once per frame
	void Update () {

        if (finished)
        {
            if (Input.GetKeyDown(KeyCode.F1))
            {
                if (curToolTip == toolTips.Count)
                {
                    curToolTip = 0;
                }
                UpdateToolTip();
            }
        }
	}

    public void UpdateToolTip()
    {
        if (curToolTip < toolTips.Count && curToolTip > -1)
        {
            toolTips[curToolTip].GetComponent<Text>().enabled = false;
            curToolTip++;

            if (curToolTip < toolTips.Count)
            {
                toolTips[curToolTip].GetComponent<Text>().enabled = true;
            }
            else
            {
                finished = true;
                toggle.GetComponent<Text>().enabled = true;

                // Show crystals
                GameObject.FindGameObjectWithTag("Player").GetComponent<CollectPickUpsAndCheckGoal>().StartHUD();
            }
        }
    }
}
