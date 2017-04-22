using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToolTipManager : MonoBehaviour {

    List<GameObject> toolTips = new List<GameObject>();
    int curToolTip = 0;
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
        }

        toolTips[0].GetComponent<Text>().enabled = true;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void UpdateToolTip()
    {
        if (curToolTip < toolTips.Count)
        {
            toolTips[curToolTip].GetComponent<Text>().enabled = false;
            curToolTip++;

            if (curToolTip < toolTips.Count)
            {
                toolTips[curToolTip].GetComponent<Text>().enabled = true;
            }
        }
    }
}
