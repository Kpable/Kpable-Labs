using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kpable.AI.Steering;

public class LabController : MonoBehaviour {

    public Vehicle[] agents;
    public Transform[] targets;

    public enum TestBehavior { Seek, Arrive, Flee, Pursuit }

	// Use this for initialization
	void Start () {
        agents[(int)TestBehavior.Seek].MaxSpeed = 10f;
        agents[(int)TestBehavior.Seek].MaxForce = 10f;
        agents[(int)TestBehavior.Seek].MaxTurnRate = 2f;
        agents[(int)TestBehavior.Seek].SetTarget(targets[(int)TestBehavior.Seek].position);

        agents[(int)TestBehavior.Arrive].MaxSpeed = 10f;
        agents[(int)TestBehavior.Arrive].MaxForce = 10f;
        agents[(int)TestBehavior.Arrive].MaxTurnRate = 2f;
        agents[(int)TestBehavior.Arrive].SetTarget(targets[(int)TestBehavior.Arrive].position);

        agents[(int)TestBehavior.Flee].MaxSpeed = 10f;
        agents[(int)TestBehavior.Flee].MaxForce = 10f;
        agents[(int)TestBehavior.Flee].MaxTurnRate = 2f;
        agents[(int)TestBehavior.Flee].SetTarget(targets[(int)TestBehavior.Flee].position);

        agents[(int)TestBehavior.Pursuit].MaxSpeed = 10f;
        agents[(int)TestBehavior.Pursuit].MaxForce = 10f;
        agents[(int)TestBehavior.Pursuit].MaxTurnRate = 2f;
        agents[(int)TestBehavior.Pursuit].SetTargetVehicle(targets[(int)TestBehavior.Pursuit].GetComponent<Vehicle>());
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        agents[(int)TestBehavior.Seek].SetTarget(targets[(int)TestBehavior.Seek].position);
        agents[(int)TestBehavior.Arrive].SetTarget(targets[(int)TestBehavior.Arrive].position);
        agents[(int)TestBehavior.Flee].SetTarget(targets[(int)TestBehavior.Flee].position);
        //agents[(int)TestBehavior.Pursuit].SetTarget(targets[(int)TestBehavior.Pursuit].position);

    }
}
