using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {
    ObjectPooler objectPooler;
	// Use this for initialization
	void Start () {
        objectPooler = ObjectPooler.Instance;

    }
	
	// Update is called once per frame
	void FixedUpdate () {

        objectPooler.SpawnFromPool("box", transform.position, Quaternion.identity);
        objectPooler.SpawnFromPool("ball", transform.position, Quaternion.identity);
    }
}
