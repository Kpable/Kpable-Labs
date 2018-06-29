using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class FloatInSpace : MonoBehaviour {

    public float speed = 0.5f;
    public float roationalSpeed = 5f;
    Rigidbody2D body;
	// Use this for initialization
	void Start () {
        body = GetComponent<Rigidbody2D>();
        body.velocity = new Vector2(Random.Range(-speed, speed), Random.Range(-speed, speed));
        body.angularVelocity = Random.Range(-roationalSpeed, roationalSpeed);
        
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
