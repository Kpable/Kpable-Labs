using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Tilemover : MonoBehaviour {
    public Tilemap tiles;

    Tile messwithme;
	// Use this for initialization
	void Start () {
        messwithme = (Tile) tiles.GetTile(new Vector3Int(-3, -6, 0));
        Debug.Log(messwithme.gameObject.transform.position);
        
    }

    // Update is called once per frame
    void Update () {
	}
}
