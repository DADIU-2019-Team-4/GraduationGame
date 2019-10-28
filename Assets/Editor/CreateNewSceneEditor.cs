using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CreateNewSceneEditor : EditorWindow
{
    public string name;

    [MenuItem("Tools/New Scene")]
    public static void ShowWindow()
    {
        GetWindow<CreateNewSceneEditor>(false, "New Scene", true);

       

    }

    void OnGUI()
        {
            //EditorGUILayout.Toggle("Mute All Sounds", false);
            //EditorGUILayout.IntField("Player Lifes", 3);
            name = EditorGUILayout.TextField("Scene Name", name);

        GUILayout.FlexibleSpace();

        if (GUILayout.Button("CreateScene"))
            {
                FileUtil.CopyFileOrDirectory("Assets/Scenes/Final Scenes/BaseScene.unity", "Assets/Scenes/Final Scenes/" + name +".unity");
                Debug.Log("nothing here");
            }
        
        }

    
}
