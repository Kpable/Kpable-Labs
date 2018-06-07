using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;


public static class DataManager {

    #region IO - Save Load Delete
    public static void SaveData(string filename, object data)
    {
        Debug.Log("DataManager: Saving Data to: " + filename);

        BinaryFormatter formatter = new BinaryFormatter();

        FileStream saveFile = File.Create(filename);

        formatter.Serialize(saveFile, data);

        saveFile.Close();
    }

    public static object LoadData(string filename)
    {
        object data = null;

        if (File.Exists(filename))
        {
            Debug.Log("DataManager: Loading Data from: " + filename);

            BinaryFormatter formatter = new BinaryFormatter();

            FileStream saveFile = File.Open(filename, FileMode.Open);

            data = formatter.Deserialize(saveFile);

            saveFile.Close();
        }
        else
        {
            Debug.LogWarning("DataManager: File: " + filename + " does not exist");
        }

        return data;
    }

    public static bool DeleteData(string filename)
    {
        if (File.Exists(filename))
        {
            File.Delete(filename);
        }
        else
        {
            Debug.LogError("DataManager: File " + filename + " does not exists");

            return false;
        }

        if (File.Exists(filename))
        {
            Debug.LogError("DataManager: Deletion of " + filename + " Failed");
            return false;
        }

        return true;
    }



    public static void SaveJson(string filename, string jsonData)
    {
        Debug.Log("DataManager: Saving Json Data to: " + filename);

        File.WriteAllText(filename, jsonData);
    }

    public static void SaveJson(string filename, object serializableObject, bool prettify=true)
    {
        if (serializableObject.GetType().IsSerializable)
        {
            Debug.Log("DataManager: Saving Json Data to: " + filename);

            File.WriteAllText(filename, JsonUtility.ToJson(serializableObject, prettify));
        }
        else
        {
            Debug.LogError("DataManager: Object passed is not serializable: " + serializableObject);
        }
    }

    public static string LoadJson(string filename)
    {

        string jsonData = "";
        if (File.Exists(filename))
        {
            Debug.Log("DataManager: Loading Json Data to: " + filename);
            jsonData = File.ReadAllText(filename);
        }
        else
            Debug.Log("DataManager: File does not Exists: " + filename);

        return jsonData;
    }

    #endregion
}
