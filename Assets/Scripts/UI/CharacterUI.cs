using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Text;

public class CharacterUI : MonoBehaviour
{
    public CharacterStats stats;
    public Character character;

    public CharacterInventory inventory;
    public CharacterInventory hotbar;


    private static CharacterUI _instance;

    public static CharacterUI Instance { get { return _instance; } }

    public List<GameObject> buildMenuTabs;

    public SceneManager sceneManager;

    public BuildMenu buildMenu1;
    public BuildMenu buildMenu2;
    public BuildMenu buildMenu3;
    public BuildMenu buildMenu4;
    public BuildMenu buildMenu5;
    

    private GameObject dialogBox;

    private Text dialogBoxText; // I need to connect this to the dialog box
    private Text storeText;
    public GameObject customSignBox;
    public Text customSignText; 
    public Text debugInventoryText;
    public Text moneyText;
    public GameObject buildGui;
    public GameObject inventoryGui;
    public GameObject externalInventoryGui;
    public GameObject externalInventoryPanels;
    public GameObject buyNSellMenu;
    public GameObject buyNSellMenuPanel;

    public GameObject mainMenuGui;

    // public StoreUiInventory storeUi;

    public GameObject sunDialSun;
    public GameObject sunDialMoon;

    public Image characterHealth; 
    public Image staminaBar;   
    public Image manaBar;   


    // Start is called before the first frame update
    private float[] keyCount = new float[10];

    public GameObject loadingScreen;

    public GameObject gameSaver;



  

    // Start is called before the first frame update

    void Awake(){
        if (_instance != null && _instance != this)
        {
            Destroy(this);
        } else {
            _instance = this;
            DontDestroyOnLoad(this);

        }


    }

    private void Start()
    {
        InitializeComponents();
    }

     
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        InitializeComponents();
    }

    private void InitializeComponents()
    {

        CharacterInventory[] inventories = character.GetComponents<CharacterInventory>();
        // Grab inventories
        foreach (CharacterInventory inv in inventories)
        {
            if (inv.inventoryIdentifier == "Primary")
            {
                inventory = inv;
            }
            else if (inv.inventoryIdentifier == "Hotbar")
            {
                hotbar = inv;
            }
        }

       

        dialogBox = GameObject.Find("DialogBox");
        dialogBoxText = GameObject.Find("DialogText").GetComponent<Text>();
        storeText = GameObject.Find("StoreTitle").GetComponent<Text>();
        customSignBox = GameObject.Find("CustomSign");
        customSignText = GameObject.Find("CustomSignText").GetComponent<Text>();
        debugInventoryText = GameObject.Find("DebugInventory").GetComponent<Text>();
        buildGui.SetActive(false);
        externalInventoryGui.SetActive(false);
        mainMenuGui.SetActive(false);
        externalInventoryPanels.SetActive(false);
        dialogBox.SetActive(false);
        customSignBox.SetActive(false);
        buyNSellMenu.SetActive(false);
        buyNSellMenuPanel.SetActive(false);

    }

    // public void AddStartingBlueprints(){

    //     Debug.Log("adding blueprints"); 
    //     // Item crafting
    //     buildMenu1.bluePrintSlots[0].LoadBluePrint("Torch x 3");

    //     // External Objects
    //     buildMenu2.bluePrintSlots[0].LoadBluePrint("Fence");
    //     buildMenu2.bluePrintSlots[1].LoadBluePrint("Gate");
    //     buildMenu2.bluePrintSlots[2].LoadBluePrint("Sign");
    //     buildMenu2.bluePrintSlots[3].LoadBluePrint("Sack");
    //     buildMenu2.bluePrintSlots[4].LoadBluePrint("Cobblestone Path");
    //     buildMenu2.bluePrintSlots[5].LoadBluePrint("Chest");
    //     buildMenu2.bluePrintSlots[6].LoadBluePrint("Combat Dummy");
    //     buildMenu3.bluePrintSlots[0].LoadBluePrint("Single Bed");
    //     buildMenu3.bluePrintSlots[1].LoadBluePrint("Double Bed");
    //     buildMenu3.bluePrintSlots[2].LoadBluePrint("Table");
    //     buildMenu3.bluePrintSlots[3].LoadBluePrint("Decorated Table");  
    //     buildMenu3.bluePrintSlots[4].LoadBluePrint("Rug");
    //     buildMenu3.bluePrintSlots[5].LoadBluePrint("Chair");
    //     buildMenu3.bluePrintSlots[6].LoadBluePrint("Chest");
    //     buildMenu3.bluePrintSlots[7].LoadBluePrint("Combat Dummy");

    //     // buildMenu.bluePrintSlots[6].LoadBluePrint("Combat Dummy");
    // }
    

    public void ToggleMainMenu(){
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
        
        // Append inventory items
        foreach (var kvp in inventory.Items)
        {
            if (!firstItem)
            {
                sb.Append(", ");
            }
            var item = kvp.Value; // Get InventoryItem from the KeyValuePair
            sb.Append($"{item.Name} x {item.Amount}");
            firstItem = false;
        }

        // Append hotbar items
        foreach (var kvp in hotbar.Items)
        {
            if (!firstItem)
            {
                sb.Append(", ");
            }
            var item = kvp.Value; // Get InventoryItem from the KeyValuePair
            sb.Append($"{item.Name} x {item.Amount}");
            firstItem = false;
        }

        debugInventoryText.text = sb.ToString();
    }



    public void ToggleBuildMenu(){
        sceneManager.ToggleBuildMenu();
    }

    public void ToggleBuildMenuTab(int i){
        foreach(var tab in buildMenuTabs){
            tab.SetActive(false);
        }
        buildMenuTabs[i].SetActive(true);

        character.activeBlueprint = null;

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
        // if (buyNSellMenu.activeSelf){
        //     if (storeUi == null)
        //         storeUi = buyNSellMenuPanel.GetComponent<StoreUiInventory>();
        //     storeText.text = storeUi.merchant.characterName+"'s "+storeUi.merchant.storeType+": "+storeUi.merchant.money.ToString()+"$";
        // }
        moneyText.text = stats.Money.ToString()+ "$";
        characterHealth.fillAmount = (float)(stats.Health)/(float)(stats.MaxHealth);
        staminaBar.fillAmount = (float)(stats.Stamina)/(float)(stats.MaxStamina);
        manaBar.fillAmount = (float)(stats.Mana)/(float)(stats.MaxMana);

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
        // if (character.currentState== character.tate.standby){
        //     // Fix me
        //     // character.StartCoroutine(character.interact("interact"));
        // }
        inventoryGui.SetActive(!inventoryGui.activeSelf);
    }
}
