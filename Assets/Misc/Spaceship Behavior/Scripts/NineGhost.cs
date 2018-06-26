using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NineGhost : MonoBehaviour {

    private Rect worldBounds;
    private Sprite ghostSprite;

    GameObject anchor;
    Transform[] ghostTransforms;

    // Use this for initialization
    void Start () {
        worldBounds = GetComponent<WorldWrap>().worldBounds;
        ghostSprite = GetComponent<SpriteRenderer>().sprite;
        ghostTransforms = new Transform[8];
        anchor = new GameObject(name + "_Ghost_Anchor");

        Vector3[] neighborhood = GetNeighborhood(transform.position, worldBounds);

        for (int i = 0; i < neighborhood.Length; i++)
        {
            GameObject ghost = new GameObject(name + "_Ghost");
            ghost.AddComponent<SpriteRenderer>().sprite = ghostSprite;
            ghost.transform.SetParent(anchor.transform);
            ghost.transform.localScale = transform.localScale;
            ghost.transform.position = neighborhood[i];
            
            ghostTransforms[i] = ghost.transform;
        }

        //Vector3 topPosition = transform.position;
        //Vector3 bottomPosition = topPosition;




        //topPosition.y += worldBounds.height;
        //bottomPosition.y -= worldBounds.height;
        //anchor = new GameObject(name + "_Ghost_Anchor");
        //GameObject topGhost = new GameObject(name + "_Ghost");
        //topGhost.AddComponent<SpriteRenderer>().sprite = ghostSprite;
        //topGhost.transform.SetParent(anchor.transform);
        //topGhost.transform.position = topPosition;
        //topGhost.transform.localScale = transform.localScale;

        //GameObject ghost = new GameObject(name + "_Ghost");
        //ghost.AddComponent<SpriteRenderer>().sprite = ghostSprite;
        //ghost.transform.SetParent(anchor.transform);
        //ghost.transform.position = bottomPosition;
        //ghost.transform.localScale = transform.localScale;

    }

    // Update is called once per frame
    void Update () {
        anchor.transform.position = transform.position;


        for (int i = 0; i < ghostTransforms.Length; i++)
        {
            ghostTransforms[i].localRotation = transform.rotation;
        }
	}

    Vector3[] GetNeighborhood(Vector3 position, Rect size)
    {
        Vector3[] hood = new Vector3[8];

        // Top Left
        hood[0] = new Vector3(position.x - size.width, position.y + size.height, 0);
        // Top
        hood[1] = new Vector3(position.x             , position.y + size.height, 0);
        // Top Right
        hood[2] = new Vector3(position.x + size.width, position.y + size.height, 0);
        // Left
        hood[3] = new Vector3(position.x - size.width, position.y            , 0);
        // Right
        hood[4] = new Vector3(position.x + size.width, position.y            , 0);
        // Bottom Left
        hood[5] = new Vector3(position.x - size.width, position.y - size.height, 0);
        // Bottom 
        hood[6] = new Vector3(position.x             , position.y - size.height, 0);
        // Bottom Right
        hood[7] = new Vector3(position.x + size.width, position.y - size.height, 0);

        return hood;
    }
}
