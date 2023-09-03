using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Milestone))]
public class MilestoneEditor : Editor
{
    Milestone comp;

    int countOfItem = 0;
    Element selectedElement;

    int _selected = 0;
    string[] _options = new string[2] { "Number of items required", "Specific item required" };

    public void OnEnable()
    {
        comp = (Milestone)target;
        selectedElement = (Element)ScriptableObject.CreateInstance("Element");
    }

    public override void OnInspectorGUI()
    {

        serializedObject.Update();

        comp.MilestoneSprite = (Sprite)EditorGUILayout.ObjectField(comp.MilestoneSprite, typeof(Sprite), false, GUILayout.Width(80f), GUILayout.Height(80f)) ;
        comp.MilestoneName = EditorGUILayout.TextField("Name", comp.MilestoneName);
        comp.MilestoneDescription = EditorGUILayout.TextField("Description", comp.MilestoneDescription);


        this._selected = EditorGUILayout.Popup("Condition type", _selected, _options);

        if(_selected == 0)
        {
            
            countOfItem = EditorGUILayout.IntField("Required number of items :", countOfItem);
            comp.condition = new ItemCountCondition(countOfItem);
        }

        if (_selected == 1)
        {

            selectedElement = (Element)EditorGUILayout.ObjectField(selectedElement, typeof(Element), false, GUILayout.Width(80f), GUILayout.Height(80f));
            comp.condition = new ItemUnlockedCondition(selectedElement);
        }

        serializedObject.ApplyModifiedProperties();

    }
}
