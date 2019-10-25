using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.Reflection;
using System.ComponentModel.Design;

    [CustomEditor(typeof(TabEditor))]
public class TabExampleEditor : Editor
{
    private TabEditor myTarget;
    private SerializedObject soTarget;
    //There should be another way to get variables from TabEditor without having predefined variables
    //TODO: Find a way to create view with variables from target script;
    public List<SerializedProperty> group1;
    public List<SerializedProperty> group2;
    public List<SerializedProperty> group3;
    public List<SerializedProperty> group4;
    //bool showPosition = true;

    private void OnEnable()
    {
        group1 = new List<SerializedProperty>();
        group2 = new List<SerializedProperty>();
        group3 = new List<SerializedProperty>();
        group4 = new List<SerializedProperty>();
        myTarget = (TabEditor)target;
        soTarget = new SerializedObject(target);

        var bindingFlags = BindingFlags.Public | BindingFlags.Instance;
        var fields = myTarget.GetType().GetFields(bindingFlags);
        for (int i = 0; i <= fields.Length - 3; i++)
        {
            var splitArray = fields[i].Name.Split(char.Parse("_"));
            if (splitArray[1] == "group1")
                group1.Add(soTarget.FindProperty(fields[i].Name));
            if (splitArray[1] == "group2")
                group2.Add(soTarget.FindProperty(fields[i].Name));
            if (splitArray[1] == "group3")
                group3.Add(soTarget.FindProperty(fields[i].Name));
            if (splitArray[1] == "group4")
                group4.Add(soTarget.FindProperty(fields[i].Name));
        }
        Debug.Log(group1.Count);
        Debug.Log(group2.Count);
        Debug.Log(group3.Count);
        Debug.Log(group4.Count);

    }
    public override void OnInspectorGUI()
    {
        soTarget.Update();
        EditorGUI.BeginChangeCheck();
        myTarget.toolbarTab = GUILayout.Toolbar(myTarget.toolbarTab, new string[] { "Group1", "Group2", "Group3", "Group4" });

        switch (myTarget.toolbarTab)
        {
            case 0:
                myTarget.currentTab = "Group1";
                break;
            case 1:
                myTarget.currentTab = "Group2";
                break;
            case 2:
                myTarget.currentTab = "Group3";
                break;
            case 3:
                myTarget.currentTab = "Group4";
                break;
        }
        if (EditorGUI.EndChangeCheck())
        {
            soTarget.ApplyModifiedProperties();
            GUI.FocusControl(null);
        }
        EditorGUI.BeginChangeCheck();
        switch (myTarget.currentTab)
        {
            case "Group1":
                //showPosition = EditorGUILayout.BeginFoldoutHeaderGroup(showPosition, "All strings");
                //if (showPosition)
                //{
                    for (int i = 0; i <= group1.Count - 1; i++)
                        EditorGUILayout.PropertyField(group1[i]);
                //}
                //EditorGUILayout.EndFoldoutHeaderGroup();
                break;
            case "Group2":
                    for (int i = 0; i <= group2.Count - 1; i++)
                        EditorGUILayout.PropertyField(group2[i]);
                break;
            case "Group3":
                    for (int i = 0; i <= group3.Count - 1; i++)
                        EditorGUILayout.PropertyField(group3[i]);
                break;
            case "Group4":
                    for (int i = 0; i <= group4.Count - 1; i++)
                        EditorGUILayout.PropertyField(group4[i]);
                break;
        }
        if (EditorGUI.EndChangeCheck())
        {
            soTarget.ApplyModifiedProperties();
        }
    }
}
