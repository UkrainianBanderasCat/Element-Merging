using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Milestone))]
public class MilestoneEditor : Editor
{
    Milestone comp;
    Element selectedElement;

    int _selected = 0;
    string[] _options = new string[2] { "Number of items required", "Specific item required" };

    SerializedProperty necessaryAmountProp;
    SerializedProperty elementRequiredProp;

    public void OnEnable()
    {
        necessaryAmountProp = serializedObject.FindProperty("necessaryAmount");
        elementRequiredProp = serializedObject.FindProperty("elementRequired");
        comp = (Milestone)target;
        selectedElement = (Element)ScriptableObject.CreateInstance("Element");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        comp.MilestoneSprite = (Sprite)EditorGUILayout.ObjectField(comp.MilestoneSprite, typeof(Sprite), false, GUILayout.Width(80f), GUILayout.Height(80f));
        comp.MilestoneName = EditorGUILayout.TextField("Name", comp.MilestoneName);
        comp.MilestoneDescription = EditorGUILayout.TextField("Description", comp.MilestoneDescription);

        this._selected = EditorGUILayout.Popup("Condition type", _selected, _options);

        if (_selected == 0) // Number of items required
        {
            EditorGUILayout.PropertyField(necessaryAmountProp, new GUIContent("Necessary Amount"));
            elementRequiredProp.objectReferenceValue = null;
        }
        else if (_selected == 1) // Specific item required
        {
            elementRequiredProp.objectReferenceValue = EditorGUILayout.ObjectField(new GUIContent("Element Required"), elementRequiredProp.objectReferenceValue, typeof(Element), false);
            necessaryAmountProp.intValue = 0;
        }

        serializedObject.ApplyModifiedProperties();
    }
}