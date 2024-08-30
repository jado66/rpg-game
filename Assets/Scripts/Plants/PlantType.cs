using UnityEngine;
using UnityEngine.Tilemaps;
using System;
using System.Collections.Generic;

namespace PlantSystem
{
    [CreateAssetMenu(fileName = "NewPlantType", menuName = "Plant System/Plant Type")]
    public class PlantType : ScriptableObject
    {
        public PlantData data;
    }
}