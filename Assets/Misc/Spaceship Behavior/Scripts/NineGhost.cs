using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NineGhost : MonoBehaviour {

    private Rect worldBounds;
    private Sprite ghostSprite;

	// Use this for initialization
	void Start () {
        worldBounds = GetComponent<WorldWrap>().worldBounds;
        ghostSprite = GetComponent<SpriteRenderer>().sprite;

        Vector3 topPosition = transform.position;
        Vector3 bottomPosition = topPosition;

        topPosition.y += worldBounds.height;
        bottomPosition.y -= worldBounds.height;

        GameObject ghost = new GameObject(name + "_Ghost");
        ghost.AddComponent<SpriteRenderer>().sprite = ghostSprite;
        ghost.transform.SetParent(transform);
        ghost.transform.position = topPosition;

        ghost = new GameObject(name + "_Ghost");
        ghost.AddComponent<SpriteRenderer>().sprite = ghostSprite;
        ghost.transform.SetParent(transform);
        ghost.transform.position = bottomPosition;



    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
