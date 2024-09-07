using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Collections.Generic;
using UnityEngine.Events;
#if UNITY_EDITOR
using UnityEditor;
#endif


[Serializable]
public class DialogData
{
    public List<DialogNode> allNodes = new List<DialogNode>();
    public UnityEvent onDialogStart;
    public UnityEvent onDialogEnd;

    public DialogNode RootNode => allNodes.Count > 0 ? allNodes[0] : null;
}

[Serializable]
public class DialogNode
{
    [TextArea(3, 10)]
    public string text;
    
    public BranchingType branchingType;

    [SerializeField]
    private List<DialogOption> options = new List<DialogOption>();

    [SerializeField]
    private UnityEvent onNodeEnter;

    [SerializeField]
    private BooleanEvent booleanEvent;

    [SerializeField]
    private List<DialogNode> branch1 = new List<DialogNode>();

    [SerializeField]
    private List<DialogNode> branch2 = new List<DialogNode>();

    // Public properties to access the private fields
    public List<DialogOption> Options => options;
    public UnityEvent OnNodeEnter => onNodeEnter;
    public BooleanEvent BooleanEvent => booleanEvent;
    public List<DialogNode> Branch1 => branch1;
    public List<DialogNode> Branch2 => branch2;
}

public enum BranchingType
{
    None,

    Options,
    BooleanEvent
}

[Serializable]
public class DialogOption
{
    public string buttonText;
    public UnityEvent onSelect;
}


[Serializable]
public class BooleanEvent : UnityEvent<bool> { }

#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(DialogNode))]
public class DialogNodeDrawer : PropertyDrawer
{
    private const float IndentWidth = 5f;
    private const float HeaderHeight = 20f;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        // Calculate indentation
        int depth = GetPropertyDepth(property);
        float indentAmount = depth * IndentWidth;

        // Draw background
        Rect backgroundRect = new Rect(position.x, position.y, position.width, GetPropertyHeight(property, label));
        EditorGUI.DrawRect(backgroundRect, new Color(0.76f, 0.76f, 0.76f, 0.1f));

        // Draw header
        Rect headerRect = new Rect(position.x + indentAmount, position.y, position.width - indentAmount, HeaderHeight);
        EditorGUI.DrawRect(headerRect, new Color(0.1f, 0.1f, 0.1f, 0.1f));
        EditorGUI.LabelField(headerRect, label, EditorStyles.boldLabel);

        // Content start position
        float yOffset = HeaderHeight + EditorGUIUtility.standardVerticalSpacing;

        // Draw fields
        var textProp = property.FindPropertyRelative("text");
        var branchingTypeProp = property.FindPropertyRelative("branchingType");
        var onNodeEnterProp = property.FindPropertyRelative("onNodeEnter");
        var optionsProp = property.FindPropertyRelative("options");
        var booleanEventProp = property.FindPropertyRelative("booleanEvent");
        var branch1Prop = property.FindPropertyRelative("branch1");
        var branch2Prop = property.FindPropertyRelative("branch2");

        EditorGUI.PropertyField(new Rect(position.x + indentAmount, position.y + yOffset, position.width - indentAmount, EditorGUI.GetPropertyHeight(textProp)), textProp);
        yOffset += EditorGUI.GetPropertyHeight(textProp) + EditorGUIUtility.standardVerticalSpacing;

        EditorGUI.PropertyField(new Rect(position.x + indentAmount, position.y + yOffset, position.width - indentAmount, EditorGUIUtility.singleLineHeight), branchingTypeProp);
        yOffset += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

        EditorGUI.PropertyField(new Rect(position.x + indentAmount, position.y + yOffset, position.width - indentAmount, EditorGUI.GetPropertyHeight(onNodeEnterProp)), onNodeEnterProp);
        yOffset += EditorGUI.GetPropertyHeight(onNodeEnterProp) + EditorGUIUtility.standardVerticalSpacing;

        BranchingType branchingType = (BranchingType)branchingTypeProp.enumValueIndex;

        if (branchingType == BranchingType.Options)
        {
            EditorGUI.PropertyField(new Rect(position.x + indentAmount, position.y + yOffset, position.width - indentAmount, EditorGUI.GetPropertyHeight(optionsProp)), optionsProp, true);
            yOffset += EditorGUI.GetPropertyHeight(optionsProp) + EditorGUIUtility.standardVerticalSpacing;
        }
        else if (branchingType == BranchingType.BooleanEvent)
        {
            EditorGUI.PropertyField(new Rect(position.x + indentAmount, position.y + yOffset, position.width - indentAmount, EditorGUI.GetPropertyHeight(booleanEventProp)), booleanEventProp);
            yOffset += EditorGUI.GetPropertyHeight(booleanEventProp) + EditorGUIUtility.standardVerticalSpacing;
        }

        if (branchingType != BranchingType.None)
        {
            EditorGUI.PropertyField(new Rect(position.x + indentAmount, position.y + yOffset, position.width - indentAmount, EditorGUI.GetPropertyHeight(branch1Prop)), branch1Prop, true);
            yOffset += EditorGUI.GetPropertyHeight(branch1Prop) + EditorGUIUtility.standardVerticalSpacing;

            EditorGUI.PropertyField(new Rect(position.x + indentAmount, position.y + yOffset, position.width - indentAmount, EditorGUI.GetPropertyHeight(branch2Prop)), branch2Prop, true);
        }

        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        float totalHeight = HeaderHeight + EditorGUIUtility.standardVerticalSpacing;

        var textProp = property.FindPropertyRelative("text");
        var branchingTypeProp = property.FindPropertyRelative("branchingType");
        var onNodeEnterProp = property.FindPropertyRelative("onNodeEnter");
        var optionsProp = property.FindPropertyRelative("options");
        var booleanEventProp = property.FindPropertyRelative("booleanEvent");
        var branch1Prop = property.FindPropertyRelative("branch1");
        var branch2Prop = property.FindPropertyRelative("branch2");

        totalHeight += EditorGUI.GetPropertyHeight(textProp) + EditorGUIUtility.standardVerticalSpacing;
        totalHeight += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing; // Branching Type
        totalHeight += EditorGUI.GetPropertyHeight(onNodeEnterProp) + EditorGUIUtility.standardVerticalSpacing;

        BranchingType branchingType = (BranchingType)branchingTypeProp.enumValueIndex;

        if (branchingType == BranchingType.Options)
        {
            totalHeight += EditorGUI.GetPropertyHeight(optionsProp) + EditorGUIUtility.standardVerticalSpacing;
        }
        else if (branchingType == BranchingType.BooleanEvent)
        {
            totalHeight += EditorGUI.GetPropertyHeight(booleanEventProp) + EditorGUIUtility.standardVerticalSpacing;
        }

        if (branchingType != BranchingType.None)
        {
            totalHeight += EditorGUI.GetPropertyHeight(branch1Prop) + EditorGUIUtility.standardVerticalSpacing;
            totalHeight += EditorGUI.GetPropertyHeight(branch2Prop) + EditorGUIUtility.standardVerticalSpacing;
        }

        return totalHeight;
    }

    private int GetPropertyDepth(SerializedProperty property)
    {
        int depth = 0;
        var parent = property.propertyPath;
        while (parent.LastIndexOf('.') > 0)
        {
            depth++;
            parent = parent.Substring(0, parent.LastIndexOf('.'));
        }
        return depth;
    }
}
#endif