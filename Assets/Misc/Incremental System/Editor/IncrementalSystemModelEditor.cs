using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class IncrementalSystemModelEditor : EditorWindow {

    public IncrementalSystemModel model;

    private string systemDataFilePath = "/StreamingAssets/IncrementalSystemEditorDemo.json";

    [MenuItem ("Window/Incremental System Editor")]
    static void Initialize()
    {
        IncrementalSystemModelEditor window = (IncrementalSystemModelEditor)EditorWindow.GetWindow(typeof(IncrementalSystemModelEditor));
        window.Show();
    }

    private void OnGUI()
    {
        if(model != null)
        {
            SerializedObject serializedObject = new SerializedObject(this);
            SerializedProperty serializedProperty = serializedObject.FindProperty("model");

            EditorGUILayout.PropertyField(serializedProperty, true);

            serializedObject.ApplyModifiedProperties();

            if(GUILayout.Button("Save data"))
            {
                SaveGameData();
            }
        }

        if (GUILayout.Button("Load data"))
        {
            LoadGameData();
        }
    }

    private void LoadGameData()
    {
        string filePath = Application.dataPath + systemDataFilePath;

        if (File.Exists(filePath))
        {
            string dataAsJson = File.ReadAllText(filePath);
            model = JsonUtility.FromJson<IncrementalSystemModel>(dataAsJson);
        }
        else
        {
            model = new IncrementalSystemModel();
        }
    }

    private void SaveGameData()
    {
        string dataAsJson = JsonUtility.ToJson(model, true);
        string filePath = Application.dataPath + systemDataFilePath;

        File.WriteAllText(filePath, dataAsJson);
    }
}
