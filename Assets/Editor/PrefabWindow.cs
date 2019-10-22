using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class PrefabWindow : EditorWindow
    {
        private static PrefabWindow _winInstance;
        public GameObject myGameObject;

    [MenuItem("Tools/PrefabSpawner %#F1")]
    public static void ShowWindow()
        {
            _winInstance = GetWindow<PrefabWindow>(true, "PrefabSpawner");
        }

        private void OnGUI()
        {
            EditorGUILayout.BeginVertical();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("My Player Prefab");
            myGameObject = EditorGUILayout.ObjectField(myGameObject, typeof(GameObject), true) as GameObject;
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Get Player Prefab"))
            {
                myGameObject = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Player.prefab") as GameObject;
            }

            if (GUILayout.Button("Instantiate Player Prefab in Scene"))
            {
                Instantiate(myGameObject);
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.EndVertical();
        }
    }
