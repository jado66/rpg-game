using UnityEngine;
using UnityEngine.Tilemaps;
using System;
using System.Collections.Generic;

namespace PlantSystem
{
    [Serializable]
    public class PlantStage
    {
        public string stageName;
        public int daysToNextStage;
        public TileBase tile;

        // Constructor for convenience
        public PlantStage(string stageName, int daysToNextStage, TileBase tile)
        {
            this.stageName = stageName;
            this.daysToNextStage = daysToNextStage;
            this.tile = tile;
        }
    }

    [Serializable]
    public class PlantData
    {
        public string plantName;
        public List<PlantStage> growthStages;
        public bool isTree;

        // Helper property to get the final stage
        public PlantStage FinalStage => growthStages[growthStages.Count - 1];
    }

    [CreateAssetMenu(fileName = "NewPlantType", menuName = "Plant System/Plant Type")]
    public class PlantType : ScriptableObject
    {
        public PlantData data;
    }

    [CreateAssetMenu(fileName = "PlantDatabase", menuName = "Plant System/Plant Database")]
    public class PlantDatabase : ScriptableObject
    {
        [SerializeField] private List<PlantType> plantTypes;

        private Dictionary<string, PlantData> plantDict;
        private Dictionary<TileBase, string> tileToNameDict;

        private void OnEnable()
        {
            InitializeDictionaries();
        }

        private void InitializeDictionaries()
        {
            plantDict = new Dictionary<string, PlantData>();
            tileToNameDict = new Dictionary<TileBase, string>();

            foreach (var plantType in plantTypes)
            {
                AddPlantToDict(plantType.data);
            }
        }

        private void AddPlantToDict(PlantData plantData)
        {
            plantDict[plantData.plantName] = plantData;

            for (int i = 0; i < plantData.growthStages.Count; i++)
            {
                var stage = plantData.growthStages[i];
                if (stage.tile != null)
                {
                    tileToNameDict[stage.tile] = $"{plantData.plantName}_Stage_{i}";
                }
            }
        }

        public PlantData GetPlantData(string plantName)
        {
            if (plantDict.TryGetValue(plantName, out PlantData plantData))
            {
                return plantData;
            }
            Debug.LogWarning($"Plant data for {plantName} not found.");
            return null;
        }

        public string GetTileName(TileBase tile)
        {
            if (tileToNameDict.TryGetValue(tile, out string name))
            {
                return name;
            }
            Debug.LogWarning($"Name for tile {tile} not found.");
            return null;
        }

        public void AddPlantType(PlantType plantType)
        {
            if (!plantTypes.Contains(plantType))
            {
                plantTypes.Add(plantType);
                AddPlantToDict(plantType.data);
            }
        }

        public List<string> GetAllPlantNames()
        {
            return new List<string>(plantDict.Keys);
        }
    }
}