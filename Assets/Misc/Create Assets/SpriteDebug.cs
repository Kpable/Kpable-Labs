using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteDebug : MonoBehaviour {

	// Use this for initialization
	void Awake () {
        var s = GetComponent<SpriteRenderer>().sprite;

        Debug.Log("Sprite: " + s.ToString() + ", Texture2D: " + s.texture.ToString() + 
            "Rect: " + s.rect.ToString() + ", Pivot: " + s.pivot.ToString() + 
            ", PPU " + s.pixelsPerUnit + ""
        
            );
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
