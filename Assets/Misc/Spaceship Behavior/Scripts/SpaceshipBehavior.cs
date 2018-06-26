using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceshipBehavior : MonoBehaviour {

    public float acceleration = 15f;
    public float maxSpeed = 250f;
    public float rotationSpeed = 180f;
    public float drag = 0.8f;

    private float hAxis, vAxis;

    Rigidbody2D body;

	// Use this for initialization
	void Start () {
        body = GetComponent<Rigidbody2D>();
        if(body)
            body.drag = drag;
	}
	
	// Update is called once per frame
	void Update () {
        hAxis = Input.GetAxis("Horizontal");
        vAxis = Input.GetAxis("Vertical");
    }

    private void FixedUpdate()
    {

        Rotation();
        Thrust();
    }

    private void Rotation()
    {
        Quaternion currentRotation = transform.rotation;

        float z = currentRotation.eulerAngles.z;
        z += -hAxis * rotationSpeed * Time.deltaTime;

        currentRotation = Quaternion.Euler(0, 0, z);

        transform.rotation = currentRotation;
    }

    private void Thrust()
    {
        Vector3 velocity = new Vector3(0, vAxis * acceleration * Time.deltaTime, 0);

        if (body)
        {
            body.AddForce(transform.rotation * velocity);

        }
        else
        {
            Vector3 pos = transform.position;
            pos += transform.rotation * velocity;
            transform.position = pos;
        }
    }

}
