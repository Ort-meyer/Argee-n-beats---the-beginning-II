using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextPatrolPoint : MonoBehaviour {
    GameObject[] patrolePoints;
    int currentPatrolePoint = 0;
	// Use this for initialization
	void Awake () {
        Waypoint[] points = GetComponentsInChildren<Waypoint>();
        patrolePoints = new GameObject[points.Length];
        foreach (var item in points)
        {
            int index = item.order;
            patrolePoints[index] = item.gameObject;
        }
	}

    public Vector3 GetNextPatrolePoint()
    {
        Vector3 returnPosition = patrolePoints[currentPatrolePoint].transform.position;
        currentPatrolePoint++;
        if (currentPatrolePoint > patrolePoints.Length - 1)
        {
            currentPatrolePoint = 0;
        }
        return returnPosition;
    }
}
