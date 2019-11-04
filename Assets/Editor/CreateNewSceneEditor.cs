using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CreateNewSceneEditor : EditorWindow
{
    public string name;
    public string folder;

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
        folder = EditorGUILayout.TextField("Folder to save in (not working)", folder);

        GUILayout.FlexibleSpace();

        if (GUILayout.Button("CreateScene"))
            {
                if (folder == "")
                {
                folder = "Final Scenes";
                }
                folder = "Final Scenes";
                FileUtil.CopyFileOrDirectory("Assets/Scenes/Final Scenes/BaseScene.unity", "Assets/Scenes/"+folder+"/" + name +".unity");
                AssetDatabase.Refresh();
                this.Close();
            }
        
        }

    
}
