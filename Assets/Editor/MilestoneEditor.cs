using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Milestone))]
public class MilestoneEditor : Editor
{
    /*-----------------Milestone Info--------------------------------------------*/
    SerializedProperty IsCompletedProp;
    SerializedProperty MilestoneSpriteProp;
    SerializedProperty MilestoneNameProp;
    SerializedProperty MilestoneDescProp;
    SerializedProperty MilestoneRewardIDProp;
    SerializedProperty MilestoneRewardProp;
    /*---------------------------------------------------------------------------*/

    /*-----------------Condition Info--------------------------------------------*/
    SerializedProperty ConditionTypeProp;

    SerializedProperty NecessaryAmountProp;
    SerializedProperty SelectedElementIDProp;
    SerializedProperty SelectedElementProp;
    SerializedProperty SelectedElementsIDsProp;
    SerializedProperty SelectedElementsProp;
    SerializedProperty NeededMilestonesProp;

    SerializedProperty UnlockedPanelProp;

    /*---------------------------------------------------------------------------*/


    string[] dropDownOptions = new string[4] { "Number of items required", "Specific item required", "Multiple items required", "Multiple different unlocked milestones required" };

    public void OnEnable()
    {
        IsCompletedProp = serializedObject.FindProperty("IsCompleted");
        MilestoneNameProp = serializedObject.FindProperty("MilestoneName");
        MilestoneDescProp = serializedObject.FindProperty("MilestoneDescription");
        MilestoneSpriteProp = serializedObject.FindProperty("MilestoneSprite");
        MilestoneRewardIDProp = serializedObject.FindProperty("rewardID");
        MilestoneRewardProp = serializedObject.FindProperty("reward");

        ConditionTypeProp = serializedObject.FindProperty("ConditionType");
        NecessaryAmountProp = serializedObject.FindProperty("NecessaryAmount");
        SelectedElementIDProp = serializedObject.FindProperty("SelectedElementID");
        SelectedElementProp = serializedObject.FindProperty("SelectedElement");
        SelectedElementsIDsProp = serializedObject.FindProperty("SelectedElementsIDs");
        SelectedElementsProp = serializedObject.FindProperty("SelectedElements");
        NeededMilestonesProp = serializedObject.FindProperty("NeededMilestones");

        UnlockedPanelProp = serializedObject.FindProperty("UnlockedPanel");

    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(IsCompletedProp);
        EditorGUILayout.PropertyField(MilestoneSpriteProp);
        EditorGUILayout.PropertyField(MilestoneNameProp);
        EditorGUILayout.PropertyField(MilestoneDescProp);
        EditorGUILayout.PropertyField(MilestoneRewardIDProp);
        EditorGUILayout.PropertyField(MilestoneRewardProp);
        EditorGUILayout.PropertyField(UnlockedPanelProp);

        ConditionTypeProp.intValue = EditorGUILayout.Popup(ConditionTypeProp.intValue, dropDownOptions);

        switch (ConditionTypeProp.intValue) {
            case 0:
                EditorGUILayout.PropertyField(NecessaryAmountProp);
                break;
            case 1:
                EditorGUILayout.PropertyField(SelectedElementIDProp);
                EditorGUILayout.PropertyField(SelectedElementProp);
                break;
            case 2:
                EditorGUILayout.PropertyField(SelectedElementsIDsProp);
                EditorGUILayout.PropertyField(SelectedElementsProp);
                break;
            case 3:
                EditorGUILayout.PropertyField(NeededMilestonesProp);
                break;
        }

        serializedObject.ApplyModifiedProperties();


    }
}
