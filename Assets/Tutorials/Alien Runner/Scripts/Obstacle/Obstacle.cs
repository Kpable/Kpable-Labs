using UnityEngine;
using System.Collections;

public class Obstacle : MonoBehaviour {

    private float speed = -9f;

    private Rigidbody2D myBody;

	// Use this for initialization
	void Awake () {
        myBody = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
        myBody.velocity = new Vector2(speed, 0);
	
	}

    void OnTriggerEnter2D(Collider2D target)
    {
        if (target.name == "Collector")
        {
            Destroy(gameObject);
        }
    }
}
