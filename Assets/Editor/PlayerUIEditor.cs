using UnityEngine;
using UnityEditor;
using System.Collections.Generic;


[CustomEditor(typeof(PlayerUI))]
public class PlayerUIEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        // Draw default inspector
        DrawDefaultInspector();

        // Explicitly draw the inventoryMap property
        EditorGUILayout.PropertyField(serializedObject.FindProperty("inventoryMap"), true);

        serializedObject.ApplyModifiedProperties();
    }
}