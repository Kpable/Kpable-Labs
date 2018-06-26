using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NineGhost : MonoBehaviour {

    private Rect worldBounds;
    private Sprite ghostSprite;

    GameObject anchor;

    // Use this for initialization
    void Start () {
        worldBounds = GetComponent<WorldWrap>().worldBounds;
        ghostSprite = GetComponent<SpriteRenderer>().sprite;

        Vector3 topPosition = transform.position;
        Vector3 bottomPosition = topPosition;

        topPosition.y += worldBounds.height;
        bottomPosition.y -= worldBounds.height;
        anchor = new GameObject(name + "_Ghost_Anchor");
        GameObject topGhost = new GameObject(name + "_Ghost");
        topGhost.AddComponent<SpriteRenderer>().sprite = ghostSprite;
        topGhost.transform.SetParent(anchor.transform);
        topGhost.transform.position = topPosition;
        topGhost.transform.localScale = transform.localScale;

        GameObject ghost = new GameObject(name + "_Ghost");
        ghost.AddComponent<SpriteRenderer>().sprite = ghostSprite;
        ghost.transform.SetParent(anchor.transform);
        ghost.transform.position = bottomPosition;
        ghost.transform.localScale = transform.localScale;

    }

    // Update is called once per frame
    void Update () {
        anchor.transform.position = transform.position;

        foreach (var ghost in anchor.GetComponentsInChildren<Transform>())
        {
            if (ghost == anchor.transform) continue;
            ghost.localRotation = transform.rotation;

        }
	}

    Vector3[] GetNeighborhood(Vector3 position, Rect size)
    {
        Vector3[] hood = new Vector3[7];

        // Top Left
        hood[0] = new Vector3(position.x + size.xMin, position.y + size.yMax, 0);
        // Top
        hood[1] = new Vector3(position.x            , position.y + size.yMax, 0);
        // Top Right
        hood[2] = new Vector3(position.x + size.xMax, position.y + size.yMax, 0);
        // Left
        hood[3] = new Vector3(position.x + size.xMin, position.y            , 0);
        // Right
        hood[4] = new Vector3(position.x + size.xMax, position.y            , 0);
        // Bottom Left
        hood[5] = new Vector3(position.x + size.xMin, position.y + size.yMin, 0);
        // Bottom Right
        hood[6] = new Vector3(position.x + size.xMax, position.y + size.yMin, 0);

        return hood;
    }
}
