using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PairedPortals : MonoBehaviour
{
    public int teleporterID;
    public GameObject exitPoint; 
    
    public bool isExit = false;
    public bool isIndoors = false;
    public bool isUnderground = false;
    public string targetSceneName;

    private static Dictionary<int, List<PairedPortals>> teleporterPairs = new Dictionary<int, List<PairedPortals>>();

    public MusicChanger musicChanger;

    private SceneManager sceneManager;
    void Start()
    {
        musicChanger = FindObjectOfType<MusicChanger>();
        if (musicChanger == null)
        {
            Debug.LogWarning("MusicChanger component not found.");
        }

        sceneManager = FindObjectOfType<SceneManager>();
        if (musicChanger == null)
        {
            Debug.LogWarning("sceneManager component not found.");
        }

        if (exitPoint == null)
        {
            Debug.LogError($"Exit point not set for teleporter {gameObject.name}. Please assign an exit point in the inspector.");
            return;
        }

        // Register this teleporter
        if (!teleporterPairs.ContainsKey(teleporterID))
        {
            teleporterPairs[teleporterID] = new List<PairedPortals>();
        }
        teleporterPairs[teleporterID].Add(this);
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (!collider.CompareTag("Character")) return;

        StartCoroutine(TeleportWithDelay(collider.gameObject));
    }

    private IEnumerator TeleportWithDelay(GameObject player)
    {
        CharacterMovement playerMovement = player.GetComponent<CharacterMovement>();
        if (!playerMovement.CanTeleport()) yield break;

        playerMovement.ResetTeleportTimer();

        sceneManager.StartFakeLoading();
        yield return new WaitForSeconds(0.2f);

        UpdateLightingSettings(sceneManager);

        if (ShouldChangeScene())
        {
            PrepareTeleportData(player.transform.position);
            // SceneManager.LoadScene(targetSceneName);
        }
        else
        {
            TeleportPlayer(player);
        }
    }

    private void UpdateLightingSettings(SceneManager sceneManager)
    {
        PairedPortals targetTeleporter = GetPairedPortals();

        if (targetTeleporter.isIndoors)
        {
            Debug.Log($"min {20}; max {80}");
            sceneManager.minIntensity = 20;
            sceneManager.maxIntensity = 80;
        }
        else if (targetTeleporter.isUnderground)
        {
            Debug.Log($"min {0}; max {0}");
            sceneManager.minIntensity = 0;
            sceneManager.maxIntensity = 0;
            sceneManager.characterUI.TriggerNight(0f, true);
        }
        else
        {
            Debug.Log($"min {0}; max {80}");
            sceneManager.maxIntensity = 80;
            sceneManager.minIntensity = 0;
        }
    }

    private bool ShouldChangeScene()
    {
        return !string.IsNullOrEmpty(targetSceneName) && targetSceneName != sceneManager.GetSceneName();
    }

    private void PrepareTeleportData(Vector3 playerPosition)
    {
        PlayerPrefs.SetInt("lastTeleporterID", teleporterID);
        PlayerPrefs.SetFloat("lastPlayerX", playerPosition.x);
        PlayerPrefs.SetFloat("lastPlayerY", playerPosition.y);
        PlayerPrefs.Save();
    }

    private void TeleportPlayer(GameObject player)
    {
        PairedPortals targetTeleporter = GetPairedPortals();
        if (targetTeleporter == null || targetTeleporter.exitPoint == null) return;

        Vector3 targetPosition = targetTeleporter.exitPoint.transform.position;
        player.transform.position = targetPosition;

        GameObject mainCamera = GameObject.FindWithTag("MainCamera");
        if (mainCamera != null)
        {
            mainCamera.transform.position = new Vector3(targetPosition.x, targetPosition.y, mainCamera.transform.position.z);
        }

        Character characterComponent = player.GetComponent<Character>();
        if (characterComponent != null)
        {
            characterComponent.isIndoors = targetTeleporter.isIndoors;
        }

        if (musicChanger != null)
        {
            musicChanger.OnPlayerLocationChange(targetTeleporter.isIndoors, targetTeleporter.isUnderground);
        }
    }

    private PairedPortals GetPairedPortals()
    {
        if (!teleporterPairs.ContainsKey(teleporterID)) return null;
        
        foreach (var teleporter in teleporterPairs[teleporterID])
        {
            if (teleporter != this) return teleporter;
        }

        Debug.LogWarning($"No paired teleporter found for ID: {teleporterID}");
        return null;
    }

    public static void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (!PlayerPrefs.HasKey("lastTeleporterID")) return;

        int lastTeleporterID = PlayerPrefs.GetInt("lastTeleporterID");
        float lastPlayerX = PlayerPrefs.GetFloat("lastPlayerX");
        float lastPlayerY = PlayerPrefs.GetFloat("lastPlayerY");

        PlayerPrefs.DeleteKey("lastTeleporterID");
        PlayerPrefs.DeleteKey("lastPlayerX");
        PlayerPrefs.DeleteKey("lastPlayerY");

        // StartCoroutine(TeleportPlayerAfterSceneLoad(lastTeleporterID, new Vector2(lastPlayerX, lastPlayerY)));
    }

    private static IEnumerator TeleportPlayerAfterSceneLoad(int teleporterID, Vector2 lastPlayerPosition)
    {
        yield return new WaitForSeconds(0.1f); // Wait for scene to initialize

        if (!teleporterPairs.ContainsKey(teleporterID)) yield break;

        GameObject player = GameObject.FindWithTag("Character");
        if (player == null) yield break;

        PairedPortals targetTeleporter = teleporterPairs[teleporterID][0];
        if (targetTeleporter.exitPoint == null) yield break;

        player.transform.position = targetTeleporter.exitPoint.transform.position;

        GameObject mainCamera = GameObject.FindWithTag("MainCamera");
        if (mainCamera != null)
        {
            mainCamera.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, mainCamera.transform.position.z);
        }
    }

    void OnDestroy()
    {
        if (teleporterPairs.ContainsKey(teleporterID))
        {
            teleporterPairs[teleporterID].Remove(this);
            if (teleporterPairs[teleporterID].Count == 0)
            {
                teleporterPairs.Remove(teleporterID);
            }
        }
    }
}

// TODO needed for cross scene teleports
// using UnityEngine;
// using UnityEngine.SceneManagement;

// public class SceneLoadHandler : MonoBehaviour
// {
//     void Awake()
//     {
//         SceneManager.sceneLoaded += PairedPortals.OnSceneLoaded;
//     }

//     void OnDestroy()
//     {
//         SceneManager.sceneLoaded -= PairedPortals.OnSceneLoaded;
//     }
// }