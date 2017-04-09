using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitSpawner : MonoBehaviour {
    public Transform[] growers;
    SoundChange sChange;
	// Use this for initialization
	void Start () {
        sChange = GetComponent<SoundChange>();

        StartCoroutine(Grow());
	}
	
    IEnumerator Grow()
    {
        while(sChange.GetPercentageFilled() < 0.95f)
        {
            for(int i = 0; i < growers.Length; i++)
            {
                float value = sChange.GetPercentageFilled() + 1.0f + Random.Range(0.001f, 0.01f);
                growers[i].localScale = new Vector3(value, value, value);
            }
            yield return new WaitForEndOfFrame();
        }
    }
}
