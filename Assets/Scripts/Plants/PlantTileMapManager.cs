using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

namespace PlantSystem
{
    public class PlantTileMapManager : MonoBehaviour
    {
        [SerializeField] private Tilemap groundTilemap;
        [SerializeField] private Tilemap choppableTilemap;

        public TileBase dirt;


        public void UpdateTile(Vector3Int location, TileBase tile)
        {
            groundTilemap.SetTile(location, tile);
        }

        public void ClearTile(Vector3Int location)
        {
            groundTilemap.SetTile(location, dirt);
        }
    }

    public class PlantInfo
    {
        public string plantName;
        public int currentStage;
        public int daysInCurrentStage;

        public PlantInfo(string name)
        {
            this.plantName = name;
            this.currentStage = 0;
            this.daysInCurrentStage = 0;
        }
    }

}