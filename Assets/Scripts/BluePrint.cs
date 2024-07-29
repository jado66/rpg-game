using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BluePrint{

    public string title;
    public string description;

    public string[] alternates;
    
    public Dictionary<string,int> price;

    public Sprite icon;
    
    public BluePrint(string title,string description, Dictionary<string,int> price,string[] alternates = null) 
    {
        this.icon = Resources.Load<Sprite>("Sprites/Items/" + title);
        
        this.title = title;
        if (this.icon == null){
            Debug.Log("Problem loading icon"+title);
        }
        this.description = description;
        // icon.sprite = Resources.Load<Sprite>("Sprites/Items/" + title);
        this.price = price;
        this.alternates = alternates;
    }

}
