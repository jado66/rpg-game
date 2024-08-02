using UnityEngine;
using System.Runtime.InteropServices;
using System;

public class LocalStorageManager : MonoBehaviour
{
    [DllImport("__Internal")]
    private static extern void SaveToLocalStorage(string key, string value);

    [DllImport("__Internal")]
    private static extern string LoadFromLocalStorage(string key);

    [DllImport("__Internal")]
    private static extern void RemoveFromLocalStorage(string key);

    public void SaveData(string key, string value)
    {
        #if UNITY_WEBGL && !UNITY_EDITOR
            SaveToLocalStorage(key, value);
        #else
            PlayerPrefs.SetString(key, value);
            PlayerPrefs.Save();
        #endif
    }

    public string LoadData(string key)
    {
        #if UNITY_WEBGL && !UNITY_EDITOR
            return LoadFromLocalStorage(key);
        #else
            return PlayerPrefs.GetString(key);
        #endif
    }

    public void RemoveData(string key)
    {
        #if UNITY_WEBGL && !UNITY_EDITOR
            RemoveFromLocalStorage(key);
        #else
            PlayerPrefs.DeleteKey(key);
            PlayerPrefs.Save();
        #endif
    }

    // New method: JSON Serialize
    public string JsonSerialize<T>(T data)
    {
        return JsonUtility.ToJson(data);
    }

    // New method: JSON Deserialize
    public T JsonDeserialize<T>(string json)
    {
        return JsonUtility.FromJson<T>(json);
    }

    // New method: Save object as JSON
    public void SaveObject<T>(string key, T data)
    {
        string json = JsonSerialize(data);
        SaveData(key, json);
    }

    // New method: Load object from JSON
    public T LoadObject<T>(string key)
    {
        string json = LoadData(key);
        return JsonDeserialize<T>(json);
    }
}