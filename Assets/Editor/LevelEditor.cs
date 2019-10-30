using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class LevelEditor : EditorWindow
{
    // The window is selected if it already exists, else it's created.
    [MenuItem("Window/Custom Level Editor")]
    private static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(LevelEditor));
    }

    // Called to draw the MapEditor windows.
    private bool paintMode = false;

    // Called to draw the MapEditor windows.
    [SerializeField]
    private int paletteIndex;

    // Called to draw the MapEditor windows.
    private void OnGUI()
    {
        paintMode = GUILayout.Toggle(paintMode, "Start painting", "Button", GUILayout.Height(60f));

        // Get a list of previews, one for each of our prefabs
        List<GUIContent> paletteIcons = new List<GUIContent>();
        foreach (GameObject prefab in palette)
        {
            // Get a preview for the prefab
            Texture2D texture = AssetPreview.GetAssetPreview(prefab);
            paletteIcons.Add(new GUIContent(texture));
        }

        // Display the grid
        paletteIndex = GUILayout.SelectionGrid(paletteIndex, paletteIcons.ToArray(), 6);
    }



    // Does the rendering of the map editor in the scene view.
    private void OnSceneGUI(SceneView sceneView)
    {
        if (paintMode)
        {
            Vector3 cellCenter = GetSelectedCell(); // Refactoring, I moved some code in this function


            DisplayVisualHelp();
            HandleSceneViewInputs(cellCenter);

            // Refresh the view
            sceneView.Repaint();

            // We have a prefab selected and we are clicking in the scene view with the left button
            if (paletteIndex < palette.Count && Event.current.type == EventType.MouseDown && Event.current.button == 0)
            {
                // Create the prefab instance while keeping the prefab link
                GameObject prefab = palette[paletteIndex];
                GameObject gameObject = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
                gameObject.transform.position = cellCenter;

                // Allow the use of Undo (Ctrl+Z, Ctrl+Y).
                Undo.RegisterCreatedObjectUndo(gameObject, "");
            }
        }
    }

    private void HandleSceneViewInputs(Vector3 cellCenter)
    {
        // Filter the left click so that we can't select objects in the scene
        if (Event.current.type == EventType.Layout)
        {
            HandleUtility.AddDefaultControl(0); // Consume the event
        }
    }

    void OnFocus()
    {
        SceneView.onSceneGUIDelegate -= this.OnSceneGUI; // Don't add twice
        SceneView.onSceneGUIDelegate += this.OnSceneGUI;

        RefreshPalette(); // Refresh the palette (can be called uselessly, but there is no overhead.)
    }

    // A list containing the available prefabs.
    [SerializeField]
    private List<GameObject> palette = new List<GameObject>();

    private string path = "Assets/Prefabs/PipelineTest";

    private void RefreshPalette()
    {
        palette.Clear();

        string[] prefabFiles = System.IO.Directory.GetFiles(path, "*.prefab");
        foreach (string prefabFile in prefabFiles)
            palette.Add(AssetDatabase.LoadAssetAtPath(prefabFile, typeof(GameObject)) as GameObject);
    }

    void OnDestroy()
    {
        SceneView.onSceneGUIDelegate -= this.OnSceneGUI;
    }

    private Vector3 cellSize = new Vector3(2f, 2f, 2f);

    private void DisplayVisualHelp()
    {
        // Get the mouse position in world space such as z = 0
        Ray guiRay = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
        Vector3 mousePosition = guiRay.origin - guiRay.direction * (guiRay.origin.y / guiRay.direction.y);

        //Debug.Log(mousePosition);
       // Debug.Log("gui origin: "+ guiRay.origin + "    gui dir: "+ guiRay.direction);

        // Get the corresponding cell on our virtual grid
        Vector3Int cell = new Vector3Int(Mathf.RoundToInt(mousePosition.x / cellSize.x),0, Mathf.RoundToInt(mousePosition.z / cellSize.z));
        Vector3 cellCenter = cell * (int)cellSize.x;
        //Debug.Log("cell center: "+ cellCenter);

        // Vertices of our square
        Vector3 topLeft = cellCenter + Vector3.left * cellSize.x * 0.5f + Vector3.forward * cellSize.x * 0.5f;
        Vector3 topRight = cellCenter - Vector3.left * cellSize.x * 0.5f + Vector3.forward * cellSize.x * 0.5f;
        Vector3 bottomLeft = cellCenter - Vector3.right * cellSize.x * 0.5f - Vector3.forward * cellSize.x * 0.5f;
        Vector3 bottomRight = cellCenter + Vector3.right * cellSize.x * 0.5f - Vector3.forward * cellSize.x * 0.5f;

        // Rendering
        Handles.color = Color.green;
        Vector3[] lines = { topLeft, topRight, topRight, bottomRight, bottomRight, bottomLeft, bottomLeft, topLeft };
        Handles.DrawLines(lines);
    }

    private Vector3 GetSelectedCell()
    {
        // Get the mouse position in world space such as z = 0
        Ray guiRay = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
        Vector3 mousePosition = guiRay.origin - guiRay.direction * (guiRay.origin.y / guiRay.direction.y);

        // Get the corresponding cell on our virtual grid
        Vector3Int cell = new Vector3Int(Mathf.RoundToInt(mousePosition.x / cellSize.x),0, Mathf.RoundToInt(mousePosition.z / cellSize.z));
        return cell * (int)cellSize.x;
    }


}