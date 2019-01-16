using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LabController : MonoBehaviour {

    public Steering[] agents;
    public Transform target;

	// Use this for initialization
	void Start () {
        agents[0].MaxSpeed = 10f;
        agents[0].MaxForce = 10f;
        agents[0].Arrive(target.position);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
