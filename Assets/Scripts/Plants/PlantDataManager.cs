using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Tilemaps;

namespace PlantSystem
{
    public class PlantDataManager : MonoBehaviour
    {
        public static PlantDataManager Instance { get; private set; }

        [SerializeField] private PlantDatabase plantDatabase;

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

        public PlantData GetPlantData(string plantName)
        {
            return plantDatabase.GetPlantData(plantName);
        }

        public TileBase GetStageTile(string plantName, int stageIndex)
        {
            var plantData = GetPlantData(plantName);
            if (plantData == null || stageIndex < 0 || stageIndex >= plantData.growthStages.Count)
            {
                return null;
            }
            return plantData.growthStages[stageIndex].tile;
        }

        public int GetTotalGrowthDays(string plantName)
        {
            var plantData = GetPlantData(plantName);
            if (plantData == null) return 0;

            int totalDays = 0;
            for (int i = 0; i < plantData.growthStages.Count - 1; i++)  // Exclude the last stage
            {
                totalDays += plantData.growthStages[i].daysToNextStage;
            }
            return totalDays;
        }

        public bool IsTree(string plantName)
        {
            var plantData = GetPlantData(plantName);
            return plantData?.isTree ?? false;
        }

        public List<string> GetAllPlantNames()
        {
            return plantDatabase.GetAllPlantNames();
        }

        public int GetTotalStages(string plantName)
        {
            var plantData = GetPlantData(plantName);
            return plantData?.growthStages.Count ?? 0;
        }
    }
}