using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Milestone))]
public class MilestoneEditor : Editor
{
    /*-----------------Milestone Info--------------------------------------------*/
    SerializedProperty MilestoneSpriteProp;
    SerializedProperty MilestoneNameProp;
    SerializedProperty MilestoneDescProp;
    SerializedProperty MilestoneRewardProp;
    /*---------------------------------------------------------------------------*/

    /*-----------------Condition Info--------------------------------------------*/
    SerializedProperty ConditionTypeProp;

    SerializedProperty NecessaryAmountProp;
    SerializedProperty SelectedElementProp;
    SerializedProperty SelectedElementsProp;
    SerializedProperty NeededMilestonesProp;

    /*---------------------------------------------------------------------------*/


    string[] dropDownOptions = new string[4] { "Number of items required", "Specific item required", "Multiple items required", "Multiple different unlocked milestones required" };

    public void OnEnable()
    {
        MilestoneNameProp = serializedObject.FindProperty("MilestoneName");
        MilestoneDescProp = serializedObject.FindProperty("MilestoneDescription");
        MilestoneSpriteProp = serializedObject.FindProperty("MilestoneSprite");
        MilestoneRewardProp = serializedObject.FindProperty("reward");

        ConditionTypeProp = serializedObject.FindProperty("ConditionType");
        NecessaryAmountProp = serializedObject.FindProperty("NecessaryAmount");
        SelectedElementProp = serializedObject.FindProperty("SelectedElement");
        SelectedElementsProp = serializedObject.FindProperty("SelectedElements");
        NeededMilestonesProp = serializedObject.FindProperty("NeededMilestones");

    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(MilestoneSpriteProp);
        EditorGUILayout.PropertyField(MilestoneNameProp);
        EditorGUILayout.PropertyField(MilestoneDescProp);
        EditorGUILayout.PropertyField(MilestoneRewardProp);

        ConditionTypeProp.intValue = EditorGUILayout.Popup(ConditionTypeProp.intValue, dropDownOptions);

        switch (ConditionTypeProp.intValue) {
            case 0:
                EditorGUILayout.PropertyField(NecessaryAmountProp);
                break;
            case 1:
                EditorGUILayout.PropertyField(SelectedElementProp);
                break;
            case 2:
                EditorGUILayout.PropertyField(SelectedElementsProp);
                break;
            case 3:
                EditorGUILayout.PropertyField(NeededMilestonesProp);
                break;
        }

        serializedObject.ApplyModifiedProperties();


    }
}
