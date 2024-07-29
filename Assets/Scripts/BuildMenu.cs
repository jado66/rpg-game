using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildMenu : MonoBehaviour
{
    public List<BluePrintSlot> bluePrintSlots;
    
    public GameObject bluePrintSlot;
    public Transform bluePrintPanel;

    public BluePrint activeBlueprint;
    // Start is called before the first frame update
    void Awake()
    {
        activeBlueprint = null;
        
        for(int i = 0; i < 10; i++)
        {
            GameObject instance = Instantiate(bluePrintSlot,bluePrintPanel);
            // instance.GetComponent<BluePrintSlot>().buildMenu = this;
            bluePrintSlots.Add(instance.transform.GetChild(1).GetComponent<BluePrintSlot>());
        }      


    }
}
