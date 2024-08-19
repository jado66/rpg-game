using UnityEngine;
using System.Collections.Generic;

public class NighttimeMonsterManager : MonoBehaviour
{
    private List<GameObject> nighttimeMonsters = new List<GameObject>();

    void Start()
    {
        // Find all NighttimeMonster components, add them to the list, and disable them
        NighttimeMonster[] monsters = FindObjectsOfType<NighttimeMonster>();
        foreach (NighttimeMonster monster in monsters)
        {
            nighttimeMonsters.Add(monster.gameObject);
            monster.gameObject.SetActive(false);
        }
    }

    public void EnableNighttimeMonsters()
    {
        foreach (GameObject monster in nighttimeMonsters)
        {
            monster.SetActive(true);
        }
    }

    public void DisableNighttimeMonsters()
    {
        foreach (GameObject monster in nighttimeMonsters)
        {
            monster.SetActive(false);
        }
    }
}