using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Text;

public class PlayerUI : MonoBehaviour
{
    public Player player;

    private static PlayerUI _instance;

    public static PlayerUI Instance { get { return _instance; } }

    public SceneManager sceneManager;
    public BuildMenu buildMenu1;
    public BuildMenu buildMenu2;
    public BuildMenu buildMenu3;
    public BuildMenu buildMenu4;
    public BuildMenu buildMenu5;
    

    public List<GameObject> buildMenuTabs;
    public GameObject dialogBox;

    public Text dialogBoxText; // I need to connect this to the dialog box
    public Text storeText;
    public GameObject customSignBox;
    public InputField customSignText; 
    public Text debugInventoryText;
    public Text moneyText;
    public GameObject buildGui;
    public GameObject inventoryGui;
    public GameObject externalInventoryGui;
    public GameObject externalInventoryPanels;
    public GameObject buyNSellMenu;
    public GameObject buyNSellMenuPanel;

    public GameObject mainMenuGui;

    public StoreUiInventory storeUi;

    public GameObject sunDialSun;
    public GameObject sunDialMoon;

    public Image playerHealth; 
    public Image staminaBar;   

    // Start is called before the first frame update
    private float[] keyCount = new float[10];

    public GameObject loadingScreen;

    public GameObject gameSaver;



  

    // Start is called before the first frame update

    void Awake(){
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        } else {
            _instance = this;
        }
    }

    void Start()
    {
        DontDestroyOnLoad(this);

        player = GameObject.Find("Character").GetComponent<Player>();
        sceneManager = GameObject.Find("SceneManager").GetComponent<SceneManager>();
        AddStartingBlueprints();

        buildGui.SetActive(false);
        inventoryGui.SetActive(PlayerPrefs.GetInt("inventoryUp?")==1);
        externalInventoryGui.SetActive(false);
        mainMenuGui.SetActive(false);
        externalInventoryPanels.SetActive(false);
        dialogBox.SetActive(false);
        // Debug.Log("Dialog box is "+(dialogBox.activeSelf?"still on":"turned off"));
        customSignBox.SetActive(false);
        // Debug.Log("Custom sign box is "+(customSignBox.activeSelf?"still on":"turned off"));
        buyNSellMenu.SetActive(false);
        buyNSellMenuPanel.SetActive(false);

    }

    public void toggleMainMenu(){
        mainMenuGui.SetActive(!mainMenuGui.activeSelf);
        if (mainMenuGui.activeSelf){
            inventoryGui.SetActive(false);
            externalInventoryGui.SetActive(false); // I should change these to functions that handle the shut downs
            buildGui.SetActive(false);
        }
    }

     void UpdateDebugInventoryText()
    {
        StringBuilder sb = new StringBuilder("Inventory: ");
        bool firstItem = true;
        foreach (var item in player.inventory.items)
        {
            if (!firstItem)
            {
                sb.Append(", ");
            }
            sb.Append($"{item.title} x {item.amount}");
            firstItem = false;
        }
        debugInventoryText.text = sb.ToString();
    }

    public void ToggleBuildMenu(){
        sceneManager.ToggleBuildMenu();
    }

    public void toggleBuildMenuTab(int i){
        foreach(var tab in buildMenuTabs){
            tab.SetActive(false);
        }
        buildMenuTabs[i].SetActive(true);

        player.activeBlueprint = null;

        foreach( var buildSlot in buildMenuTabs[i].GetComponent<BuildMenu>().bluePrintSlots){
            buildSlot.transform.parent.GetComponent<Image>().color = new Color(.2627f,.2627f,.2627f); 
        }
    }

    public void LoadStartScreen(){
        LoadingData.sceneToLoad = "StartingScreen";
            
        Instantiate(loadingScreen,Vector3.zero,Quaternion.identity);
    }

    public void Quit(){
        // UnityEditor.EditorApplication.isPlaying = false;
        Application.Quit();
    }

    public void SaveGame(){
        Debug.Log("Saving Game");
        Instantiate(gameSaver);
    }

    void Update()
    {
        UpdateGui();
        UpdateDebugInventoryText(); // Add this line to continually update inventory text
    }

    void UpdateGui(){
        if (buyNSellMenu.activeSelf){
            if (storeUi == null)
                storeUi = buyNSellMenuPanel.GetComponent<StoreUiInventory>();
            storeText.text = storeUi.merchant.characterName+"'s "+storeUi.merchant.storeType+": "+storeUi.merchant.money.ToString()+"$";
        }
        moneyText.text = player.money.ToString()+ "$";
        playerHealth.fillAmount = (float)(player.health)/(float)(player.healthMax);
        staminaBar.fillAmount = (float)(player.stamina)/(float)(player.staminaMax);

    }

    public void AddStartingBlueprints(){
        // Item crafting
        buildMenu1.bluePrintSlots[0].LoadBluePrint("Torch x 3");

        // External Objects
        buildMenu2.bluePrintSlots[0].LoadBluePrint("Fence");
        buildMenu2.bluePrintSlots[1].LoadBluePrint("Gate");
        buildMenu2.bluePrintSlots[2].LoadBluePrint("Sign");
        buildMenu2.bluePrintSlots[3].LoadBluePrint("Sack");
        buildMenu2.bluePrintSlots[4].LoadBluePrint("Cobblestone Path");
        buildMenu2.bluePrintSlots[5].LoadBluePrint("Chest");
        buildMenu2.bluePrintSlots[6].LoadBluePrint("Combat Dummy");
        buildMenu3.bluePrintSlots[0].LoadBluePrint("Single Bed");
        buildMenu3.bluePrintSlots[1].LoadBluePrint("Double Bed");
        buildMenu3.bluePrintSlots[2].LoadBluePrint("Table");
        buildMenu3.bluePrintSlots[3].LoadBluePrint("Decorated Table");  
        buildMenu3.bluePrintSlots[4].LoadBluePrint("Rug");
        buildMenu3.bluePrintSlots[5].LoadBluePrint("Chair");
        buildMenu3.bluePrintSlots[6].LoadBluePrint("Chest");
        buildMenu3.bluePrintSlots[7].LoadBluePrint("Combat Dummy");

        // buildMenu.bluePrintSlots[6].LoadBluePrint("Combat Dummy");
    }
    public void ToggleInventory(){
        if (customSignBox.activeSelf)
            return;

        if (buildGui.activeSelf){
            buildGui.SetActive(false);
        }
        if (mainMenuGui.activeSelf){
            mainMenuGui.SetActive(false);
        }
        if (player.currentState== PlayerState.standby){
            // Fix me
            // player.StartCoroutine(player.interact("interact"));
        }
        inventoryGui.SetActive(!inventoryGui.activeSelf);
    }
}
