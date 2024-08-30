using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class TilemapChunkManager : MonoBehaviour
{
    public int chunkSize = 32;
    public Transform player;
    public Camera mainCamera;
    
    [SerializeField]
    private List<Tilemap> originalTilemaps = new List<Tilemap>();
    
    private Dictionary<Vector2Int, TilemapChunk> chunks = new Dictionary<Vector2Int, TilemapChunk>();

    private void Start()
    {
        foreach (var tilemap in originalTilemaps)
        {
            var renderer = tilemap.GetComponent<TilemapRenderer>();
            if (renderer != null) renderer.enabled = false;
        }

        // Create a single chunk at the player's position
        Vector2Int playerChunkPos = WorldToChunkPosition(player.position);
        CreateAndPopulateSingleChunk(playerChunkPos);
    }

    private Vector2Int WorldToChunkPosition(Vector3 worldPosition)
    {
        return new Vector2Int(
            Mathf.FloorToInt(worldPosition.x / chunkSize),
            Mathf.FloorToInt(worldPosition.y / chunkSize)
        );
    }

    private void CreateAndPopulateSingleChunk(Vector2Int chunkPos)
    {
        Debug.Log($"Creating chunk at position: {chunkPos}");

        GameObject chunkObject = new GameObject($"Chunk {chunkPos.x},{chunkPos.y}");
        Vector3 chunkWorldPos = new Vector3(chunkPos.x * chunkSize, chunkPos.y * chunkSize, 0);
        chunkObject.transform.position = chunkWorldPos;
        chunkObject.transform.SetParent(transform);

        Debug.Log($"Chunk object created at world position: {chunkWorldPos}");

        TilemapChunk newChunk = new TilemapChunk(chunkPos, chunkSize, originalTilemaps, chunkObject);
        chunks.Add(chunkPos, newChunk);

        PopulateChunk(newChunk);
    }

    private void PopulateChunk(TilemapChunk chunk)
    {
        Vector3Int chunkStart = new Vector3Int(chunk.ChunkPosition.x * chunkSize, chunk.ChunkPosition.y * chunkSize, 0);
        int tilesPopulated = 0;

        Debug.Log($"Populating chunk {chunk.ChunkPosition}. Chunk start: {chunkStart}");

        for (int x = 0; x < chunkSize; x++)
        {
            for (int y = 0; y < chunkSize; y++)
            {
                Vector3Int worldTilePos = chunkStart + new Vector3Int(x, y, 0);
                Vector3Int localTilePos = new Vector3Int(x, y, 0);

                for (int l = 0; l < originalTilemaps.Count; l++)
                {
                    TileBase tile = originalTilemaps[l].GetTile(worldTilePos);
                    if (tile != null)
                    {
                        chunk.SetTile(localTilePos, tile, l);
                        tilesPopulated++;
                        Debug.Log($"Set tile at world position: {worldTilePos}, local position: {localTilePos}, layer: {l}");
                    }
                }
            }
        }

        Debug.Log($"Populated chunk {chunk.ChunkPosition} with {tilesPopulated} tiles.");
    }
}

public class TilemapChunk
{
    public Vector2Int ChunkPosition { get; private set; }
    public bool IsActive { get; private set; }
    
    private GameObject chunkObject;
    private List<Tilemap> tilemapLayers = new List<Tilemap>();
    private int chunkSize;
    
    public TilemapChunk(Vector2Int position, int size, List<Tilemap> originalLayers, GameObject parentObject)
    {
        ChunkPosition = position;
        chunkObject = parentObject;
        chunkSize = size;
        CreateTilemapLayers(originalLayers);
    }
    
    private void CreateTilemapLayers(List<Tilemap> originalLayers)
    {
        Debug.Log($"Creating tilemap layers for chunk {ChunkPosition}");

        foreach (var originalLayer in originalLayers)
        {
            GameObject layerObject = new GameObject(originalLayer.name);
            layerObject.transform.SetParent(chunkObject.transform);
            
            Tilemap tilemap = layerObject.AddComponent<Tilemap>();
            TilemapRenderer renderer = layerObject.AddComponent<TilemapRenderer>();
            
            renderer.sortingLayerID = originalLayer.GetComponent<TilemapRenderer>().sortingLayerID;
            renderer.sortingOrder = originalLayer.GetComponent<TilemapRenderer>().sortingOrder;
            tilemap.tileAnchor = originalLayer.tileAnchor;
            
            tilemap.ResizeBounds();
            
            tilemapLayers.Add(tilemap);

            Debug.Log($"Created tilemap layer: {originalLayer.name} for chunk {ChunkPosition}");
        }
    }
    
    public void SetTile(Vector3Int localPosition, TileBase tile, int layerIndex)
    {
        if (layerIndex >= 0 && layerIndex < tilemapLayers.Count)
        {
            tilemapLayers[layerIndex].SetTile(localPosition, tile);
            Debug.Log($"Set tile in chunk {ChunkPosition} at local position {localPosition} in layer {layerIndex}");
        }
        else
        {
            Debug.LogError($"Invalid layer index {layerIndex} for chunk {ChunkPosition}");
        }
    }
}