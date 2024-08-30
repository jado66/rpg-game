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

    

    
}