using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

public class BaseAssetCreator : MonoBehaviour
{
    static string ASSETS = "Assets";

    static string MATERIALS = "Materials";
    static string SPRITES = "Sprites";

    static string BASIC_MATERIALS = "Basic Materials";
    static string BASIC_SPRITES = "Basic Sprites";

    static string BASIC_MATERIALS_FOLDER = ASSETS + "/" + MATERIALS + "/" + BASIC_MATERIALS + "/";
    static string BASIC_SPRITES_FOLDER = ASSETS + "/" + SPRITES + "/" + BASIC_SPRITES + "/";


    [MenuItem("Kpable/Create Basic Assets")]
    static void CreateBasicAssets()
    {
        ValidateFolderStructure();

        CreateBasicMaterial(Color.black, "Black");
        CreateBasicMaterial(Color.blue, "Blue");
        CreateBasicMaterial(Color.cyan, "Cyan");
        CreateBasicMaterial(Color.grey, "Grey");
        CreateBasicMaterial(Color.green, "Green");
        CreateBasicMaterial(Color.magenta, "Magenta");
        CreateBasicMaterial(Color.red, "Red");
        CreateBasicMaterial(Color.white, "White");
        CreateBasicMaterial(Color.yellow, "Yellow");

        //CreateBasicSprite();

    }

    static void ValidateFolderStructure()
    {
        if (!AssetDatabase.IsValidFolder(ASSETS + "/" + MATERIALS))
        {
            Debug.Log("Creating folder: " + "/" + ASSETS + "/" + MATERIALS);
            AssetDatabase.CreateFolder(ASSETS, MATERIALS);
        }

        if (!AssetDatabase.IsValidFolder(ASSETS + "/" + SPRITES))
        {
            Debug.Log("Creating folder: " + "/" + ASSETS + "/" + SPRITES);
            AssetDatabase.CreateFolder(ASSETS, SPRITES);
        }

        if (!AssetDatabase.IsValidFolder(ASSETS + "/" + MATERIALS + "/" + BASIC_MATERIALS))
        {
            Debug.Log("Creating folder: " + ASSETS + "/" + MATERIALS + "/" + BASIC_MATERIALS);
            AssetDatabase.CreateFolder(ASSETS + "/" + MATERIALS, BASIC_MATERIALS);
        }

        if (!AssetDatabase.IsValidFolder(ASSETS + "/" + SPRITES + "/" + BASIC_SPRITES))
        {
            Debug.Log("Creating folder: " + ASSETS + "/" + SPRITES + "/" + BASIC_SPRITES);
            AssetDatabase.CreateFolder(ASSETS + "/" + SPRITES, BASIC_SPRITES);
        }
    }

    static void CreateBasicMaterial(Color color, string label, string shaderName = "Standard")
    {
        string path = BASIC_MATERIALS_FOLDER + "Mat_" + shaderName + "_" + label + ".mat";

        Material material = new Material(Shader.Find(shaderName));
        material.color = color;
        AssetDatabase.CreateAsset(material, path);
        Debug.Log(AssetDatabase.GetAssetPath(material));
    }

    static void CreateBasicSprite()
    {
        string path = BASIC_SPRITES_FOLDER + "Sprite_.png";

        //Sprite sprite = Sprite.Create(Texture2D.whiteTexture, new Rect(Vector2.zero, Vector2.one * 4), Vector2.one * 2) ;
        
        //AssetDatabase.CreateAsset(tex, path);
        //Debug.Log(AssetDatabase.GetAssetPath(tex));
    }

}
