using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour, IPooledObject
{

    public float upforce = 1f;
    public float sideForce = .1f;

    // Use this for initialization
    public void OnObjectSpawned()
    {
        float xForce = Random.Range(-sideForce, sideForce);
        float yForce = Random.Range(upforce/2f, upforce);

        Vector2 force = new Vector2(xForce, yForce);

        GetComponent<Rigidbody2D>().velocity = force;
    }
}
