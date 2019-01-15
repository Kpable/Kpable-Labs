using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopToPositions : MonoBehaviour {

    public Transform transforms;
    Vector3[] points;
    int index = 0;

	// Use this for initialization
	void Start () {
        points = new Vector3[transforms.childCount];
        for (int i = 0; i < transforms.childCount; i++)
        {
            points[i] = transforms.GetChild(i).position;
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider collision)
    {
        transform.position = points[index];
        index++;
        if (index == points.Length)
            index = 0;
    }
}
