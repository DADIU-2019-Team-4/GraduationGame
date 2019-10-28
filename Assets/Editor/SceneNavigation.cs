using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;

public class SceneNavigation : EditorWindow
{
    List<string> scenesMain;
    List<string> scenesAdditive;
    List<string> activeScenesList;
    GUIStyle horizontalLine;

    // Add menu item named "My Window" to the Window menu
    [MenuItem("Tools/Scene Navigation")]
    public static void ShowWindow()
    {
        //Show existing window instance. If one doesn't exist, make one.
        EditorWindow.GetWindow(typeof(SceneNavigation));
    }

    void OnGUI()
    {
        setHorizontalLine();
        scenesMain = ReadNames("main");
        var activeScene = SceneManager.GetActiveScene().name;
        scenesAdditive = ReadNames("additive");
        if (!activeScenesList.Contains(activeScene))
            activeScenesList.Add(activeScene);
        GUILayout.Label("Editor part:", EditorStyles.boldLabel);
        DrawList("Editor");
        GUILayout.Label("Playmode part:", EditorStyles.boldLabel);
        DrawList("Playmode");
        GUILayout.TextArea("The tool for loading/unloading main and aditive scenes that are in build settings. \n" +
            "The scenes should be saved in Assest/Scenes/ folder to fully fucntion in Editor part. \n" +
            "If new functionality is needed, please inform Vlad about changes that needs to be done. \n"
            +"P.S. All scenes cannot be removed, so the last one left in inspector won't unload till you won't add another additive scene.");
    }
    private static List<string> ReadNames(string value)
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
        return temp;
    }
    void OnInspectorUpdate()
    {
        Repaint();
    }
    void DrawUI(string type, int value, string load,List<string> scenes)
    {
        for (int i = 0; i <= scenes.Count - 1 ; i++)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(scenes[i], EditorStyles.miniBoldLabel);
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
                            if (activeScenesList != null)
                            {
                                if (!activeScenesList.Contains(scenes[i]))
                                    activeScenesList.Add(scenes[i]);
                            }
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
        HorizontalLine(Color.white);
    }
     void HorizontalLine(Color color)
    {
        var c = GUI.color;
        GUI.color = color;
        GUILayout.Box(GUIContent.none, horizontalLine);
        GUI.color = c;
    }
    void setHorizontalLine()
    {
        horizontalLine = new GUIStyle();
        horizontalLine.normal.background = EditorGUIUtility.whiteTexture;
        horizontalLine.margin = new RectOffset(0, 0, 4, 4);
        horizontalLine.fixedHeight = 1;
    }
    void DrawList(string type)
    {
        HorizontalLine(Color.white);
        GUILayout.Label("Base scenes:");
        DrawUI(type, 0, "Load:", scenesMain);
        GUILayout.Label("Additive scenes:");
        DrawUI(type, 1, "Load:", scenesAdditive);
        GUILayout.Label("Loaded Scenes:", EditorStyles.boldLabel);
        if (activeScenesList != null)
            DrawUI(type, 0, "Unload:", activeScenesList);
    }
}
