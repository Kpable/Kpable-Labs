using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PicrossGenerator : MonoBehaviour {

    public Texture2D puzzleImage;
    public PicrossPuzzle puzzleData;
	// Use this for initialization
	void Start () {
        puzzleData = new PicrossPuzzle(puzzleImage.width, puzzleImage.height);

        GeneratePuzzle();
    }
	
    void GeneratePuzzle()
    {

        for (int x = 0; x < puzzleImage.width; x++ )
        {
            for (int y = 0; y < puzzleImage.height; y++)
            {
                GenerateTile(x, y);
            }
        }
    }

    void GenerateTile(int x, int y)
    {
        Color pixelColor = puzzleImage.GetPixel(x, y);

        puzzleData.Data[x, y] = new PicrossTile(pixelColor.a == 0, pixelColor);


        //if (pixelColor.a == 0) return;
        //Debug.Log(pixelColor);
    }
}

[System.Serializable]
public class PicrossPuzzle
{
    public string Name;
    public string Category;
    public int Width;
    public int Height;
    public PicrossTile[,] Data;

    public Vector2 Size { get { return new Vector2(Width, Height); } }


    public PicrossPuzzle(int width, int height)
    {
        Width = width;
        Height = height;
        Data = new PicrossTile[Width, Height];
    }
}

[System.Serializable]
public class PicrossTile
{
    public bool Set;
    public Color Color;

    public PicrossTile(bool set, Color pixelColor)
    {
        Set = set;
        Color = pixelColor;
    }
}
