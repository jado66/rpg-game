using UnityEngine;
using System.Collections.Generic;

public class EntitySpawner : MonoBehaviour
{
    public List<GameObject> spawnedEntities = new List<GameObject>();
    public GameObject entityPrefab;
    public int maxEntities = 5;
    public float spawnRadius = 5f;
    public float chanceToSpawn = 0.5f;
    public float activationDistance = 20f;

    private TilePalette tilePalette;
    private Transform player;

    void Start()
    {
        tilePalette = FindObjectOfType<TilePalette>();
        if (tilePalette == null)
        {
            Debug.LogError("TilePalette not found in the scene!");
        }

        player = GameObject.FindGameObjectWithTag("Character").transform;
        if (player == null)
        {
            Debug.LogError("Player not found in the scene!");
        }
    }

    void Update()
    {
        if (player != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);
            bool isPlayerInRange = distanceToPlayer <= activationDistance;

            // Enable or disable entities based on player proximity
            foreach (GameObject entity in spawnedEntities)
            {
                if (entity != null)
                {
                    entity.SetActive(isPlayerInRange);
                }
            }
        }
    }

    public void SpawnEntities()
    {
        int entitiesToSpawn = maxEntities - spawnedEntities.Count;

        for (int i = 0; i < entitiesToSpawn; i++)
        {
            if (Random.value <= chanceToSpawn)
            {
                Vector3 spawnPosition = GetValidSpawnPosition();
                if (spawnPosition != Vector3.zero)
                {
                    GameObject newEntity = Instantiate(entityPrefab, spawnPosition, Quaternion.identity);
                    newEntity.GetComponent<LivingEntity>().spawner = this;
                    spawnedEntities.Add(newEntity);

                    // Set initial active state based on player proximity
                    float distanceToPlayer = Vector3.Distance(spawnPosition, player.position);
                    newEntity.SetActive(distanceToPlayer <= activationDistance);
                }
            }
        }
    }

    private Vector3 GetValidSpawnPosition()
    {
        for (int attempts = 0; attempts < 30; attempts++)
        {
            Vector3 randomPos = Random.insideUnitSphere * spawnRadius;
            randomPos.y = 0; // Ensure the entity spawns on the ground
            Vector3 spawnPos = transform.position + randomPos;

            // Convert the spawn position to a Vector3Int
            Vector3Int tilePos = new Vector3Int(Mathf.FloorToInt(spawnPos.x), Mathf.FloorToInt(spawnPos.y), Mathf.FloorToInt(spawnPos.z));

            // Check if the tile at this position is collidable
            if (!tilePalette.IsCollidableAtTile(tilePos))
            {
                return spawnPos; // Return the valid spawn position
            }
        }

        Debug.LogWarning("Could not find a valid spawn position after 30 attempts.");
        return Vector3.zero; // Fallback if no valid position found
    }

    public void RemoveDeadEntity(GameObject deadEntity)
    {
        if (spawnedEntities.Contains(deadEntity))
        {
            spawnedEntities.Remove(deadEntity);
            Destroy(deadEntity);
        }
    }
}