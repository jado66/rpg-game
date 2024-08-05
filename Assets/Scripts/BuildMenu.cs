using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildMenu : MonoBehaviour
{
    public List<BluePrintSlot> bluePrintSlots;
    
    public GameObject bluePrintSlotPrefab;
    public Transform bluePrintPanel;

    [SerializeField]
    private BluePrint activeBlueprint;

    [SerializeField]
    private List<string> initialBlueprints = new List<string>(10);

    // Start is called before the first frame update
    void Awake()
    {
        activeBlueprint = null;
        
        for(int i = 0; i < 10; i++)
        {
            GameObject instance = Instantiate(bluePrintSlotPrefab,bluePrintPanel);
            // instance.GetComponent<BluePrintSlot>().buildMenu = this;

            bluePrintSlots.Add(instance.transform.GetChild(1).GetComponent<BluePrintSlot>());
        }      

    }

    void Start(){
        AddStartingBlueprints();

    }

    public bool IsInitialized()
    {
        return bluePrintSlots.Count > 0;
    }

    public void AddStartingBlueprints()
    {
        Debug.Log("Adding blueprints");

        for (int i = 0; i < bluePrintSlots.Count && i < initialBlueprints.Count; i++)
        {
            bluePrintSlots[i].LoadBluePrint(initialBlueprints[i]);
        }
    }

   
}

public class Assignment
{
    public BluePrintSlot slot;
}