using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SavedGameButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

    GameData gameData;

    public Text textBox;

    public TooltipUI tooltip;
    // Start is called before the first frame update
    void Awake()
    {
        // tooltip = GameObject.Find("TooltipUI").GetComponent<TooltipUI>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void loadGameData(GameData gameData){
        this.gameData = gameData;
        ChangeText(gameData.name);
    }
    public void ChangeText(string text){
        textBox.text = text.ToUpper()+"'S GAME";
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("Enter");
        // tooltip.GenerateTooltip(gameData);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        tooltip.HideTooltip();
    }
}
