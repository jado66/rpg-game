using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CharacterInventory))]
public class CharacterInventoryEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        CharacterInventory inventory = (CharacterInventory)target;

        // if (GUILayout.Button("Add Random Tool Item"))
        // {
        //     var tool = InventoryItem.Uses.Tool;

        //     List<ToolItem> toolItems = new List<ToolItem>
        //     {
        //         new ToolItem("33", "Hammer", "Used to construct whatever is in the build menu?", 1, tool, new Dictionary<string, int>(), 125, new List<string>(), new List<string>()),
        //         new ToolItem("34", "Garden Shovel", "For digging gardens", 1, tool, new Dictionary<string, int>(), 150, new List<string>(), new List<string>()),
        //         new ToolItem("35", "Trench Shovel", "For digging trenches", 1, tool, new Dictionary<string, int>(), 350, new List<string>(), new List<string>()),
        //         new ToolItem("36", "Axe", "For chopping all things choppable", 1, tool, new Dictionary<string, int>(), 550, new List<string>(), new List<string>()),
        //         new ToolItem("37", "Pickaxe", "It's off to work", 1, tool, new Dictionary<string, int>(), 650, new List<string>(), new List<string>())
        //     };

        //     // Pick a random item from the list
        //     System.Random random = new System.Random();
        //     int randomIndex = random.Next(toolItems.Count);
        //     ToolItem randomToolItem = toolItems[randomIndex];

        //     inventory.TryAddItem(randomToolItem);
        //     EditorUtility.SetDirty(inventory);
        // }
    }
}
