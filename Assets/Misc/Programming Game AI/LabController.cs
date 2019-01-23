using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kpable.AI.Steering;

public class LabController : MonoBehaviour {

    public Vehicle[] agents;
    public Transform[] targets;

	// Use this for initialization
	void Start () {
        agents[0].MaxSpeed = 10f;
        agents[0].MaxForce = 10f;
        agents[0].MaxTurnRate = 2f;
        agents[0].SetTarget(targets[0].position);

        agents[1].MaxSpeed = 10f;
        agents[1].MaxForce = 10f;
        agents[1].MaxTurnRate = 2f;
        agents[1].SetTarget(targets[1].position);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
