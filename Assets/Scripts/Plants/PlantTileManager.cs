﻿using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

namespace PlantSystem
{
    public class PlantTileManager : MonoBehaviour
    {
        public static PlantTileManager Instance { get; private set; }

        private Dictionary<Vector3Int, PlantInfo> plantTiles = new Dictionary<Vector3Int, PlantInfo>();

        [SerializeField] private PlantTileMapManager tileMapManager;
        [SerializeField] private PlantDataManager plantDataManager;


        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public bool TryPlantSeed(Vector3Int location, string plantName)
        {
            Debug.Log($"Attempting to plant seed '{plantName}' at location {location}.");
            
            if (!plantTiles.ContainsKey(location) && IsLocationPlantable(location))
            {
                plantTiles[location] = new PlantInfo(plantName);
                UpdatePlantTile(location);

                Debug.Log($"Successfully planted '{plantName}' at location {location}.");
                return true;
            }

            if (plantTiles.ContainsKey(location))
            {
                Debug.LogWarning($"Failed to plant '{plantName}'. Location {location} is already occupied.");
            }
            else if (!IsLocationPlantable(location))
            {
                Debug.LogWarning($"Failed to plant '{plantName}'. Location {location} is not plantable.");
            }

            return false;
        }

        private void UpdatePlantTile(Vector3Int location)
        {
            if (plantTiles.TryGetValue(location, out PlantInfo plant))
            {
                TileBase newTile = plantDataManager.GetStageTile(plant.plantName, plant.currentStage);
                tileMapManager.UpdateTile(location, newTile);
            }
        }

        private bool IsLocationPlantable(Vector3Int location)
        {
            // You can add more complex checks here, such as:
            // - Is the tile suitable for planting?
            // - Is there no obstacle at this location?
            // - Any other game-specific conditions
            
            // For now, we'll just check if the location is not occupied by another plant
            return !plantTiles.ContainsKey(location);
        }

        public void GrowPlant(Vector3Int location)
        {
            if (plantTiles.TryGetValue(location, out PlantInfo plant))
            {
                plant.daysInCurrentStage++;
                var plantData = plantDataManager.GetPlantData(plant.plantName);
                
                if (plant.currentStage < plantData.growthStages.Count - 1 && 
                    plant.daysInCurrentStage >= plantData.growthStages[plant.currentStage].daysToNextStage)
                {
                    plant.currentStage++;
                    plant.daysInCurrentStage = 0;
                    UpdatePlantTile(location);
                }

                if (plant.currentStage == plantData.growthStages.Count - 1)
                {
                    // Plant is fully grown, you might want to handle this case (e.g., make it harvestable)
                }
            }
        }

        public void GrowAllPlants()
        {
            foreach (var location in new List<Vector3Int>(plantTiles.Keys))
            {
                GrowPlant(location);
            }
        }

        public void AdvanceAllPlantsToNextStage()
        {
            List<Vector3Int> plantsToRemove = new List<Vector3Int>();

            foreach (var kvp in plantTiles)
            {
                Vector3Int location = kvp.Key;
                PlantInfo plant = kvp.Value;
                var plantData = plantDataManager.GetPlantData(plant.plantName);

                if (plant.currentStage < plantData.growthStages.Count - 1)
                {
                    // Advance to the next stage
                    plant.currentStage++;
                    plant.daysInCurrentStage = 0;
                    UpdatePlantTile(location);
                    Debug.Log($"Advanced {plant.plantName} at {location} to stage {plant.currentStage}");
                }
                else
                {
                    Debug.Log($"{plant.plantName} at {location} is already fully grown");
                }
            }
        }

        
        public void HarvestPlant(Vector3Int location)
        {
            if (plantTiles.TryGetValue(location, out PlantInfo plant))
            {
                var plantData = plantDataManager.GetPlantData(plant.plantName);
                if (plant.currentStage == plantData.growthStages.Count - 1)
                {
                    // The plant is fully grown, so we can harvest it
                    tileMapManager.ClearTile(location);
                    plantTiles.Remove(location);
                    
                    // Here you might want to add logic for giving the player the harvested item
                }
            }
        }

        public bool CanHarvest(Vector3Int location)
        {
            if (plantTiles.TryGetValue(location, out PlantInfo plant))
            {
                var plantData = plantDataManager.GetPlantData(plant.plantName);
                return plant.currentStage == plantData.growthStages.Count - 1;
            }
            return false;
        }
    }

    
}