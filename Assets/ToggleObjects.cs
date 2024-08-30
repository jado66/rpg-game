using UnityEngine;
using System.Collections.Generic;

public class ToggleObjects : MonoBehaviour
{
    [System.Serializable]
    public class ToggleEntry
    {
        public string key;
        public GameObject gameObject;
    }

    public List<ToggleEntry> toggleEntries = new List<ToggleEntry>();
    private Dictionary<string, GameObject> toggleDictionary = new Dictionary<string, GameObject>();

    private void Awake()
    {
        // Populate the dictionary from the list
        foreach (var entry in toggleEntries)
        {
            if (!string.IsNullOrEmpty(entry.key) && entry.gameObject != null)
            {
                toggleDictionary[entry.key] = entry.gameObject;
            }
        }
    }

    public void ToggleObjectByKey(string key)
    {
        if (toggleDictionary.TryGetValue(key, out GameObject targetObject))
        {
            targetObject.SetActive(!targetObject.activeSelf);
        }
        else
        {
            Debug.LogWarning($"No object found with key: {key}");
        }
    }

    public void ToggleAllObjects()
    {
        foreach (var obj in toggleDictionary.Values)
        {
            obj.SetActive(!obj.activeSelf);
        }
    }

    public void ToggleAllOffAndTargetOn(string key)
    {
        foreach (var obj in toggleDictionary.Values)
        {
            obj.SetActive(false);
        }

        if (toggleDictionary.TryGetValue(key, out GameObject targetObject))
        {
            targetObject.SetActive(true);
        }
        else
        {
            Debug.LogWarning($"No object found with key: {key}");
        }
    }
}