using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseBehavior : MonoBehaviour {

    Vector3 targetPosition;
	// Use this for initialization
	void Start () {
        targetPosition = transform.position;

    }

    // Update is called once per frame
    void Update () {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            targetPosition.x = hit.point.x;
            targetPosition.z = hit.point.z;
        }

        transform.position = targetPosition;
    }
}
