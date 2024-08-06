using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TooltipUI : MonoBehaviour {
    private Text tooltipText;

	private Image tooltipImage;
	void Start () {
        tooltipText = GetComponentInChildren<Text>();
        tooltipImage = GetComponentInChildren<Image>();
        HideTooltip();
	}


    public void HideTooltip(){
        tooltipText.enabled = false;
        tooltipImage.enabled = false;
    }

    public void ShowTooltip(){
        tooltipText.enabled = true;
        tooltipImage.enabled = true;
    }

    public void GenerateSaveGameTooltip(GameData savedGame){
        
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

     public void GenerateTooltip(InventoryItem item)
    {
        string statText = "";
        if (item.Stats.Count > 0)
        {
            foreach(var stat in item.Stats)
            {
                statText += stat.Key.ToString() + ": " + stat.Value + "\n";
            }
        }

        string tooltip = string.Format("<b>{0}</b>\n{1}\n\n<b>{2}</b>", item.Name, item.Description, statText);
        tooltipText.text = tooltip;
        gameObject.SetActive(true);
    }


    public void GenerateBuyTooltip(InventoryItem item)
    {
        string statText = "";
        if (item.Stats.Count > 0)
        {
            foreach(var stat in item.Stats)
            {
                statText += stat.Key.ToString() + ": " + stat.Value + "\n";
            }
        }

        string tooltip = string.Format("<b>{0}</b> - Buy:{3}\n{1}\n\n<b>{2}</b>", item.Name, item.Description, statText,item.Value);
        tooltipText.text = tooltip;
        gameObject.SetActive(true);
    }

    public void GenerateSellTooltip(InventoryItem item)
    {
        string statText = "";
        if (item.Stats.Count > 0)
        {
            foreach(var stat in item.Stats)
            {
                statText += stat.Key.ToString() + ": " + stat.Value + "\n";
            }
        }

        string tooltip = string.Format("<b>{0}</b> - Sell:{3}\n{1}\n\n<b>{2}</b>", item.Name, item.Description, statText,item.Value);
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
        ShowTooltip();
    }
}
