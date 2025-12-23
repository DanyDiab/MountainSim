using UnityEngine;
using System.IO;

public static class SaveManger{
    public static void Save<T>(T data, string filename){
        string path = Path.Combine(Application.persistentDataPath, filename);
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(path, json);
    }

    public static void Load<T>(T data, string filename) where T : class{
        string path = Path.Combine(Application.persistentDataPath, filename);
        if (File.Exists(path)){
            string json = File.ReadAllText(path);
            JsonUtility.FromJsonOverwrite(json, data);
        }
    }
}