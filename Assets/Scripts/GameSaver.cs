using UnityEngine;
// using System;
// using System.Collections.Generic;
// using System.Linq;
// using PlantSystem;
// using UnityEngine.Tilemaps;
// using System.Reflection; 



// [System.Serializable]
// public class SavedGameInfo
// {
//     public string id;
//     public string playerName;
//     public float money;
//     public int day;
//     public string sceneName;
//     public DateTime saveDate;
// }

// [System.Serializable]
// public class SavedGameData
// {
//     public string id;
//     public string playerName;
//     public Vector3 playerPosition;
//     public float money;
//     public int day;
//     public string sceneName;
//     public float normalizedHour;
//     public bool worldLightsOn;
//     public string difficulty;
//     public Dictionary<string, object> characterStats;
//     public List<InventoryItemData> inventory;
//     public List<InventoryItemData> hotbar;
//     public Dictionary<string, PlantTileData> plantTiles;
//     public TilemapData tilemapData;
// }

// [System.Serializable]
// public class InventoryItemData
// {
//     public string itemInfo; // Format: "ItemName:Amount"
// }

// [System.Serializable]
// public class PlantTileData
// {
//     public Vector3Int position;
//     public string plantName;
//     public int currentStage;
//     public int daysInCurrentStage;
// }

// [System.Serializable]
// public class TilemapData
// {
//     public List<TileData> choppableTiles;
//     public List<TileData> collidableTiles;
//     public List<TileData> groundTiles;
//     public List<TileData> minableTiles;
//     public List<TileData> decorTiles;
//     public List<TileData> interactableTiles;
// }

// [System.Serializable]
// public class TileData
// {
//     public Vector3Int position;
//     public string tileName;
// }

public class GameSaver : MonoBehaviour
{
//     public SceneManager sceneManager;
//     private const string SAVED_GAMES_KEY = "SavedGames";
//     private const int MAX_SAVED_GAMES = 10;

//     private const string CURRENT_GAME_ID_KEY = "CurrentGameId";

//     private Dictionary<string, AdvancedRuleTile> tileNameToTile;
//     private Dictionary<AdvancedRuleTile, string> tileToTileName;


//     public static GameSaver Instance { get; private set; }

//      private void Awake()
//     {
//         if (Instance == null)
//         {
//             Instance = this;
//             DontDestroyOnLoad(gameObject);
//         }
//         else
//         {
//             Destroy(gameObject);
//         }

//         InitializeTileMappings();

//     }

//     private void InitializeTileMappings()
//     {
//         tileNameToTile = new Dictionary<string, AdvancedRuleTile>();
//         tileToTileName = new Dictionary<AdvancedRuleTile, string>();

//         TilePalette tilePalette = TilePalette.Instance;
//         Type tilePaletteType = typeof(TilePalette);

//         foreach (FieldInfo field in tilePaletteType.GetFields(BindingFlags.Public | BindingFlags.Instance))
//         {
//             if (field.FieldType == typeof(AdvancedRuleTile))
//             {
//                 AdvancedRuleTile tile = field.GetValue(tilePalette) as AdvancedRuleTile;
//                 if (tile != null)
//                 {
//                     string tileName = field.Name;
//                     tileNameToTile[tileName] = tile;
//                     tileToTileName[tile] = tileName;
//                 }
//             }
//         }
//     }

//     public void SaveGame()
//     {
//         string gameId = PlayerPrefs.GetString(CURRENT_GAME_ID_KEY, null);
        
//         if (string.IsNullOrEmpty(gameId))
//         {
//             gameId = System.Guid.NewGuid().ToString();
//             PlayerPrefs.SetString(CURRENT_GAME_ID_KEY, gameId);
//             PlayerPrefs.Save();
//         }

//         SavedGameData saveData = new SavedGameData
//         {
//             id = gameId,
//             playerName = Character.Instance.playerName,
//             playerPosition = Character.Instance.transform.position,
//             money = Character.Instance.GetStats().Money,
//             day = sceneManager.day,
//             sceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name,
//             normalizedHour = sceneManager.normalizedHour,
//             worldLightsOn = sceneManager.worldLightsOn,
//             difficulty = sceneManager.difficulty,
//             characterStats = SerializeCharacterStats(),
//             inventory = SerializeInventory(Character.Instance.GetInventory()),
//             hotbar = SerializeInventory(Character.Instance.GetHotbar()),
//             plantTiles = SerializePlantTiles(),
//             tilemapData = SerializeTilemaps()
//         };

//         string json = JsonUtility.ToJson(saveData);
//         SaveToStorage(saveData.id, json);

//         UpdateSavedGamesList(saveData);
//     }

//     public void LoadGame(string saveId)
//     {
//         string json = LoadFromStorage(saveId);
//         if (string.IsNullOrEmpty(json)) return;

//         SavedGameData saveData = JsonUtility.FromJson<SavedGameData>(json);

//         // Set the current game ID
//         PlayerPrefs.SetString(CURRENT_GAME_ID_KEY, saveData.id);
//         PlayerPrefs.Save();
        
//         // Load scene
//         UnityEngine.SceneManagement.SceneManager.LoadScene(saveData.sceneName);

//         // Set character data
//         Character.Instance.playerName = saveData.playerName;
//         Character.Instance.transform.position = saveData.playerPosition;
//         Character.Instance.GetStats().SetMoney(saveData.money);

//         // Set scene manager data
//         sceneManager.day = saveData.day;
//         sceneManager.normalizedHour = saveData.normalizedHour;
//         sceneManager.worldLightsOn = saveData.worldLightsOn;
//         sceneManager.difficulty = saveData.difficulty;

//         // Load character stats
//         DeserializeCharacterStats(saveData.characterStats);

//         // Load inventory and hotbar
//         DeserializeInventory(Character.Instance.GetInventory(), saveData.inventory);
//         DeserializeInventory(Character.Instance.GetHotbar(), saveData.hotbar);

//         // Load plant tiles
//         DeserializePlantTiles(saveData.plantTiles);

//         // Load tilemaps
//         DeserializeTilemaps(saveData.tilemapData);
//     }

//     private Dictionary<string, object> SerializeCharacterStats()
//     {
//         CharacterStats stats = Character.Instance.GetStats();
//         return new Dictionary<string, object>
//         {
//             {"Health", stats.Health},
//             {"MaxHealth", stats.MaxHealth},
//             {"Mana", stats.Mana},
//             {"MaxMana", stats.MaxMana},
//             {"Stamina", stats.Stamina},
//             {"MaxStamina", stats.MaxStamina},
//             {"Speed", stats.Speed},
//             {"SwimSpeed", stats.SwimSpeed}
//         };
//     }

//     private void DeserializeCharacterStats(Dictionary<string, object> statsData)
//     {
//         CharacterStats stats = Character.Instance.GetStats();
//         stats.ModifyMaxHealth((float)statsData["MaxHealth"]);
//         stats.Heal((float)statsData["Health"]);
//         stats.ModifyMaxMana((float)statsData["MaxMana"]);
//         stats.RegainMana((float)statsData["Mana"]);
//         stats.ModifyMaxStamina((float)statsData["MaxStamina"]);
//         stats.RegainStamina((float)statsData["Stamina"]);
//         stats.ModifySpeed((float)statsData["Speed"]);
//         stats.ModifySwimSpeed((float)statsData["SwimSpeed"]);
//     }

//     private List<InventoryItemData> SerializeInventory(CharacterInventory inventory)
//     {
//         return inventory.Items.Select(kvp => new InventoryItemData
//         {
//             itemInfo = $"{kvp.Value.Name}:{kvp.Value.Amount}"
//         }).ToList();
//     }

//     private void DeserializeInventory(CharacterInventory inventory, List<InventoryItemData> itemsData)
//     {
//         inventory.ClearItems();
//         foreach (var itemData in itemsData)
//         {
//             inventory.GiveItem(itemData.itemInfo);
//         }
//     }

//     private Dictionary<string, PlantTileData> SerializePlantTiles()
//     {
//         return PlantTileManager.Instance.plantTiles.ToDictionary(
//             kvp => kvp.Key.ToString(),
//             kvp => new PlantTileData
//             {
//                 position = kvp.Key,
//                 plantName = kvp.Value.plantName,
//                 currentStage = kvp.Value.currentStage,
//                 daysInCurrentStage = kvp.Value.daysInCurrentStage
//             }
//         );
//     }

//     private void DeserializePlantTiles(Dictionary<string, PlantTileData> plantTilesData)
//     {
//         PlantTileManager.Instance.plantTiles.Clear();
//         foreach (var kvp in plantTilesData)
//         {
//             Vector3Int position = StringToVector3Int(kvp.Key);
//             PlantTileManager.Instance.plantTiles[position] = new PlantInfo(kvp.Value.plantName)
//             {
//                 currentStage = kvp.Value.currentStage,
//                 daysInCurrentStage = kvp.Value.daysInCurrentStage
//             };
//         }
//     }

//     private TilemapData SerializeTilemaps()
//     {
//         TilePalette tilePalette = TilePalette.Instance;
//         return new TilemapData
//         {
//             choppableTiles = SerializeTilemap(tilePalette.choppable),
//             collidableTiles = SerializeTilemap(tilePalette.collidable),
//             groundTiles = SerializeTilemap(tilePalette.ground),
//             minableTiles = SerializeTilemap(tilePalette.minable),
//             decorTiles = SerializeTilemap(tilePalette.decor),
//             interactableTiles = SerializeTilemap(tilePalette.interactable)
//         };
//     }

//     private List<TileData> SerializeTilemap(Tilemap tilemap)
//     {
//         List<TileData> tileDataList = new List<TileData>();
//         foreach (var pos in tilemap.cellBounds.allPositionsWithin)
//         {
//             Vector3Int localPlace = new Vector3Int(pos.x, pos.y, pos.z);
//             if (tilemap.HasTile(localPlace))
//             {
//                 TileBase tile = tilemap.GetTile(localPlace);
//                 string tileName;

//                 if (tile is AdvancedRuleTile advancedTile && tileToTileName.TryGetValue(advancedTile, out string name))
//                 {
//                     tileName = name;
//                 }
//                 else
//                 {
//                     tileName = tile.name;
//                 }

//                 tileDataList.Add(new TileData
//                 {
//                     position = localPlace,
//                     tileName = tileName
//                 });
//             }
//         }
//         return tileDataList;
//     }

//     private void DeserializeTilemaps(TilemapData tilemapData)
//     {
//         TilePalette tilePalette = TilePalette.Instance;
//         DeserializeTilemap(tilePalette.choppable, tilemapData.choppableTiles);
//         DeserializeTilemap(tilePalette.collidable, tilemapData.collidableTiles);
//         DeserializeTilemap(tilePalette.ground, tilemapData.groundTiles);
//         DeserializeTilemap(tilePalette.minable, tilemapData.minableTiles);
//         DeserializeTilemap(tilePalette.decor, tilemapData.decorTiles);
//         DeserializeTilemap(tilePalette.interactable, tilemapData.interactableTiles);
//     }

//     private void DeserializeTilemap(Tilemap tilemap, List<TileData> tileDataList)
//     {
//         tilemap.ClearAllTiles();
//         foreach (var tileData in tileDataList)
//         {
//             TileBase tile;
//             if (tileNameToTile.TryGetValue(tileData.tileName, out AdvancedRuleTile advancedTile))
//             {
//                 tile = advancedTile;
//             }
//             else
//             {
//                 tile = Resources.Load<TileBase>($"Tiles/{tileData.tileName}");
//             }

//             if (tile != null)
//             {
//                 tilemap.SetTile(tileData.position, tile);
//             }
//             else
//             {
//                 Debug.LogWarning($"Tile {tileData.tileName} not found.");
//             }
//         }
//     }


//     private void UpdateSavedGamesList(SavedGameData saveData)
//     {
//         List<SavedGameInfo> savedGames = GetSavedGamesList();
        
//         SavedGameInfo gameInfo = new SavedGameInfo
//         {
//             id = saveData.id,
//             playerName = saveData.playerName,
//             money = saveData.money,
//             day = saveData.day,
//             sceneName = saveData.sceneName,
//             saveDate = DateTime.Now
//         };

//         savedGames.Insert(0, gameInfo);
        
//         if (savedGames.Count > MAX_SAVED_GAMES)
//         {
//             savedGames.RemoveAt(savedGames.Count - 1);
//         }

//         string json = JsonUtility.ToJson(new { savedGames });
//         PlayerPrefs.SetString(SAVED_GAMES_KEY, json);
//         PlayerPrefs.Save();
//     }

//     public List<SavedGameInfo> GetSavedGamesList()
//     {
//         string json = PlayerPrefs.GetString(SAVED_GAMES_KEY, "{ \"savedGames\": [] }");
//         return JsonUtility.FromJson<SavedGamesWrapper>(json).savedGames;
//     }

//     private void SaveToStorage(string key, string value)
//     {
//         PlayerPrefs.SetString(key, value);
//         PlayerPrefs.Save();
//     }

//     private string LoadFromStorage(string key)
//     {
//         return PlayerPrefs.GetString(key, null);
//     }

//     private Vector3Int StringToVector3Int(string vectorString)
//     {
//         string[] values = vectorString.Trim('(', ')').Split(',');
//         return new Vector3Int(int.Parse(values[0]), int.Parse(values[1]), int.Parse(values[2]));
//     }
// }

// [System.Serializable]
// public class SavedGamesWrapper
// {
//     public List<SavedGameInfo> savedGames;
// }


// // using System.Collections;
// // using System.Collections.Generic;
// // using System.Runtime.Serialization.Formatters.Binary;
// // using System.Runtime.Serialization;
// // using UnityEngine;
// // using System.IO;


// // public class GameSaver : MonoBehaviour
// // {
// //     Character character;
// //     SceneManager sceneManager;
// //     // Start is called before the first frame update
// //     void Start()
// //     {
// //         character = GameObject.Find("Player1").GetComponent<Character>(); 
// //         sceneManager = GameObject.Find("SceneManager").GetComponent<SceneManager>(); 
// //         // SaveGame();
// //     }

// //     // Update is called once per frame
    

// //     GameData collectGameData(){

// //         string characterName = character.playerName;
// //         // int realGameSecondsPlayed = character.realGameSecondsPlayed;

// //         // int characterLevel = character.level;

// //         // string currentRealm = sceneManager.realmName;

// //         // int numberOfRealmsDiscovered = character.howManyRealmsHasCharacterDiscovered(true);
// //         // int numberOfMiniRealmsDiscovered = character.howManyRealmsHasCharacterDiscovered(false);

// //         GameData gameData = new GameData(characterName, 0, 0, "", 0, 0);

// //         return gameData;
// //     }

// //     void SaveGame(){
// //         string characterName = character.playerName;

// //         GameData gameData =  collectGameData();

// //         if(!Directory.Exists(Application.persistentDataPath + "/SavedGames"))
// //         {    
// //             //if it doesn't, create it
// //             Directory.CreateDirectory(Application.persistentDataPath + "/SavedGames");
        
// //         }

// //         if(!Directory.Exists(Application.persistentDataPath + "/SavedGames/"+characterName))
// //         {    
// //             //if it doesn't, create it
// //             Directory.CreateDirectory(Application.persistentDataPath + "/SavedGames/"+characterName);
        
// //         }

// //         string filePath = Application.persistentDataPath + "/SavedGames"+
// //                           string.Format("/{0}/gameData.save",characterName);

// //         BinaryFormatter bf = new BinaryFormatter();
// //         FileStream file = File.Create(filePath);

// //         Debug.Log("Trying to save "+filePath);

// //         bf.Serialize(file, gameData);
// //         file.Close();
// //     }
}
