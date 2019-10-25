using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;

public class SceneNavigation : EditorWindow
{
    string[] scenesMain;
    string[] scenesAdditive;
    List<string> activeScenesList;

    // Add menu item named "My Window" to the Window menu
    [MenuItem("Tools/Scene Navigation")]
    public static void ShowWindow()
    {
        //Show existing window instance. If one doesn't exist, make one.
        EditorWindow.GetWindow(typeof(SceneNavigation));
    }

    void OnGUI()
    {

        GUILayout.Label("Scenes:", EditorStyles.boldLabel);
        scenesMain = ReadNames("main");
        scenesAdditive = ReadNames("additive");
        GUILayout.Label("Editor part:", EditorStyles.boldLabel);
        DrawUI("Editor", 0, "Load:", scenesMain);
        GUILayout.Label("List of additive scenes :", EditorStyles.boldLabel);
        DrawUI("Editor", 1, "Load:", scenesAdditive);
        GUILayout.Label("Loaded Scenes:", EditorStyles.boldLabel);
        DrawUI("Editor", 0, "Unload:", activeScenesList.ToArray());

        GUILayout.Label("Playmode part:", EditorStyles.boldLabel);
        DrawUI("Playmode", 0, "Load:",scenesMain);
        GUILayout.Label("List of additive scenes :", EditorStyles.boldLabel);
        DrawUI("Playmode", 1, "Load:", scenesAdditive);
        GUILayout.Label("Loaded Scenes:", EditorStyles.boldLabel);
        DrawUI("Playmode", 1, "Unload:", activeScenesList.ToArray());
    }
    private static string[] ReadNames(string value)
    {
        List<string> temp = new List<string>();
        foreach (EditorBuildSettingsScene S in EditorBuildSettings.scenes)
        {
            if (S.enabled)
            {
                string name = S.path.Substring(S.path.LastIndexOf('/') + 1);
                name = name.Substring(0, name.Length - 6);
                string[] tempArray = name.Split(char.Parse("_"));
                if (tempArray[1] == value)
                    temp.Add(name);
            }
        }
        return temp.ToArray();
    }
    void OnInspectorUpdate()
    {
        Repaint();
    }
    void DrawUI(string type, int value, string load,string[] scenes)
    {
        for (int i = 0; i <= scenes.Length - 1; i++)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(scenes[i]);
            if (load == "Load:")
            {
                if (GUILayout.Button(load + scenes[i], GUILayout.Width(200f), GUILayout.Height(15f)))
                {
                    switch (value)
                    {
                        case 0:
                            if (type == "Editor")
                                EditorSceneManager.OpenScene("Assets/Scenes/" + scenes[i] + ".unity", OpenSceneMode.Single);
                            else
                                SceneManager.LoadScene(scenes[i], LoadSceneMode.Single);
                            activeScenesList.Clear();
                            activeScenesList.Add(scenes[i]);
                            break;
                        case 1:
                            if (type == "Editor")
                                EditorSceneManager.OpenScene("Assets/Scenes/" + scenes[i] + ".unity", OpenSceneMode.Additive);
                            else
                                SceneManager.LoadScene(scenes[i], LoadSceneMode.Additive);
                            activeScenesList.Add(scenes[i]);
                            break;
                    }
                }
            }
            if (load == "Unload:")
            {
                if (GUILayout.Button(load + scenes[i], GUILayout.Width(200f), GUILayout.Height(15f)))
                {
                    switch (value)
                    {
                        case 0:
                            if (type == "Editor")
                                EditorSceneManager.CloseScene(SceneManager.GetSceneByName(scenes[i]), true);
                            else
                                SceneManager.UnloadSceneAsync(scenes[i], UnloadSceneOptions.None);
                            activeScenesList.Remove(scenes[i]);
                            break;
                        case 1:
                            if (type == "Editor")
                                EditorSceneManager.CloseScene(SceneManager.GetSceneByName(scenes[i]), true);
                            else
                                SceneManager.UnloadSceneAsync(scenes[i], UnloadSceneOptions.None);
                            activeScenesList.Remove(scenes[i]);
                            break;
                    }
                }
            }
            EditorGUILayout.EndHorizontal();
            GUILayout.Space(2f);
        }
        GUILayout.Space(10f);
    }
}
