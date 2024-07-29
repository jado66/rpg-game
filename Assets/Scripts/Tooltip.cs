﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tooltip : MonoBehaviour {
    private Text tooltipText;

	void Start () {
        tooltipText = GetComponentInChildren<Text>();
        gameObject.SetActive(false);
	}

    public void GenerateTooltip(GameData savedGame){
        
        int realSecondsPlayed = savedGame.realGameSecondsPlayed;
        int hoursPlayed = realSecondsPlayed/3600;
        int minutesPlayed = (realSecondsPlayed-hoursPlayed*3600)/60;
        string timePlayed = (hoursPlayed>=1?string.Format("{0} Hr{1} ",hoursPlayed.ToString(),(hoursPlayed>1?"s":"")):"")+
                            (hoursPlayed>=1&&minutesPlayed>=1?"and ":"") +
                            (minutesPlayed>=1?string.Format("{0} Min{1} ",minutesPlayed.ToString(),(minutesPlayed>1?"s":"")):"");
        string tooltip = string.Format("<b>{0}\nTime Played:</b> {1}\n<b>Character Level:</b> {2}\n<b>Current Realm:</b> {3}", 
                                       savedGame.name, timePlayed, savedGame.characterLevel,savedGame.currentRealm)+
                         string.Format("\n<b>Realms Visited:</b> {0}\n<b>Mini-realms Visited</b>: {1}",savedGame.numberOfRealmsDiscovered, savedGame.numberOfMiniRealmsDiscovered);
        
        
        tooltipText.text = tooltip;
        gameObject.SetActive(true);
    }
    public void GenerateTooltip(Item item)
    {
        string statText = "";
        if (item.stats.Count > 0)
        {
            foreach(var stat in item.stats)
            {
                statText += stat.Key.ToString() + ": " + stat.Value + "\n";
            }
        }

        string tooltip = string.Format("<b>{0}</b>\n{1}\n\n<b>{2}</b>", item.titleToDisplay, item.description, statText);
        tooltipText.text = tooltip;
        gameObject.SetActive(true);
    }

    public void GenerateBuyTooltip(Item item)
    {
        string statText = "";
        if (item.stats.Count > 0)
        {
            foreach(var stat in item.stats)
            {
                statText += stat.Key.ToString() + ": " + stat.Value + "\n";
            }
        }

        string tooltip = string.Format("<b>{0}</b> - Buy:{3}\n{1}\n\n<b>{2}</b>", item.titleToDisplay, item.description, statText,item.value);
        tooltipText.text = tooltip;
        gameObject.SetActive(true);
    }

    public void GenerateSellTooltip(Item item)
    {
        string statText = "";
        if (item.stats.Count > 0)
        {
            foreach(var stat in item.stats)
            {
                statText += stat.Key.ToString() + ": " + stat.Value + "\n";
            }
        }

        string tooltip = string.Format("<b>{0}</b> - Sell:{3}\n{1}\n\n<b>{2}</b>", item.titleToDisplay, item.description, statText,item.value);
        tooltipText.text = tooltip;
        gameObject.SetActive(true);
    }

    public void GenerateTooltip(BluePrint bluePrint)
    {
        string statText = "Build Cost\n";
        if (bluePrint.price.Count > 0)
        {
            foreach(var value in bluePrint.price)
            {
                statText += value.Key.ToString() + ": " + value.Value + "\n";
            }
        }

        string tooltip = string.Format("<b>{0}</b>\n{1}\n\n<b>{2}</b>", bluePrint.title, bluePrint.description, statText);
        if (bluePrint.alternates != null){
            foreach(var value in bluePrint.alternates)
            {
                string alternate = string.Format("<b>{0}</b>", value);
                tooltip += "or " +alternate+"\n";
            } 
        }
        tooltipText.text = tooltip;
        gameObject.SetActive(true);
    }
}