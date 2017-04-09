using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllEnemyTracker : MonoBehaviour {
    public static AllEnemyTracker manager = null;
    List<GameObject> enemies = new List<GameObject>();
	// Use this for initialization
	void Awake () {
        if (manager != null)
        {
            Debug.LogError("Two instances of AllEnemyTracker in scene");
        }
        manager = this;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void NewEnemy(GameObject enemy)
    {
        enemies.Add(enemy);
    }

    public void KilledEnemy(GameObject enemy)
    {
        enemies.Remove(enemy);  
    }

    public void KilledPlayer(GameObject player)
    {
        foreach (var item in enemies)
        {
            item.GetComponent<MovementManager>().PlayerDead(player);
        }
    }
}
