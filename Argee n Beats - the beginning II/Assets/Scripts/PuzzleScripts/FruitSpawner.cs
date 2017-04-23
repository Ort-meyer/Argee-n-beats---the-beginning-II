using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitSpawner : MonoBehaviour {
    public Transform[] growers;
    SoundChange sChange;

    bool isGrown = false;

    List<GameObject> fruits = new List<GameObject>();
    public int maxNrFruits = 10;
    public GameObject fruit;
    public Transform fruitSpawnPos;
    public float spawnTime = 5.0f;
    public float area = 4;
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
                float changeValue = 3;
                float value = (sChange.GetPercentageFilled() * changeValue) + 1.0f + Random.Range(0.001f, 0.01f);
                growers[i].localScale = new Vector3(value, value, value);
            }
            yield return new WaitForEndOfFrame();
        }
        isGrown = true;

        while(this != null)
        {
            Vector3 randomV = new Vector3(Random.Range(-area, area), Random.Range(-area * 0.3f, area * 0.3f), Random.Range(-area, area));
            Vector3 rPos = fruitSpawnPos.position + randomV;

            GameObject tempO = null;
            if (fruits.Count < maxNrFruits)
            {
                tempO = Instantiate(fruit.gameObject);
                tempO.transform.position = rPos;
                fruits.Add(tempO);
            }

            for(int i = 0; i < fruits.Count; i++)
            {
                if(fruits[i] == null)
                {
                    fruits.RemoveAt(i);
                }
            }

            //if (tempO != null)
            //{
            //    Destroy(tempO, 10); //explodera dem istället för fan!
            //}
            yield return new WaitForSeconds(spawnTime);
        }
    }
}
