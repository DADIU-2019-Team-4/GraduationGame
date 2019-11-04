using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class LevelEditor : EditorWindow
{
    // The window is selected if it already exists, else it's created.
    [MenuItem("Tools/Custom Level Editor")]

    


    private static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(LevelEditor));
    }

    // Called to draw the MapEditor windows.
    private bool paintMode = false;
    private bool deleteMode = false;

    private int floorXScale = 1;
    private int floorZScale = 1;

    static private float gridScale = 1;

    private float objectRotation = 0;

    private Vector3 currentCellSelected;
    private Vector3 previousCellSelected;

    // Called to draw the MapEditor windows.
    [SerializeField]
    private int paletteIndex;

    // Called to draw the MapEditor windows.
    private void OnGUI()
    {
            paintMode = GUILayout.Toggle(paintMode, "Start painting", "Button", GUILayout.Height(60f));
        
            deleteMode = GUILayout.Toggle(deleteMode, "Delete", "Button", GUILayout.Height(60f));

            floorXScale = EditorGUILayout.IntField("floor x",floorXScale);
            floorZScale = EditorGUILayout.IntField("floor z", floorZScale);
            objectRotation = EditorGUILayout.FloatField("rotation of objects", objectRotation);

        if (paintMode)
            deleteMode = false;

        // Get a list of previews, one for each of our prefabs
        List<GUIContent> paletteIcons = new List<GUIContent>();
        foreach (GameObject prefab in palette)
        {
            // Get a preview for the prefab
            Texture2D texture = AssetPreview.GetAssetPreview(prefab);
            
            paletteIcons.Add(new GUIContent(texture,prefab.transform.name));
        }

        // Display the grid
        paletteIndex = GUILayout.SelectionGrid(paletteIndex, paletteIcons.ToArray(), 4);


        //roate 90 with r
        
    }


    private bool mouseDown = false;
    private Vector3 clickedCell;
    private Vector3 releasedCell;

    private Color selectColor = Color.green;

    // Does the rendering of the map editor in the scene view.
    private void OnSceneGUI(SceneView sceneView)
    {
        if (Event.current.type == EventType.KeyDown && Event.current.character == 'r')
        {
            Debug.Log("pressed r");
            objectRotation += 90;

            if (objectRotation >=360)
            {
                objectRotation = 0;
            }
            Repaint();
        }

        if (paintMode)
        {
            Vector3 cellCenter = GetSelectedCell(); // Refactoring, I moved some code in this function

            previousCellSelected = currentCellSelected;
            currentCellSelected = cellCenter;
            
            DisplayVisualHelp();
            HandleSceneViewInputs(cellCenter);
            

            if (Event.current.type == EventType.MouseDown && Event.current.button == 0 && !Event.current.alt)
            {
                mouseDown = true;
                selectColor = Color.red;
                clickedCell = cellCenter;
            }else if(Event.current.type == EventType.MouseUp && Event.current.button == 0 && !Event.current.alt)
            {
                mouseDown = false;
                selectColor = Color.green;
                InstanciateObjects(clickedCell,cellCenter);
            }

            // Refresh the view
            sceneView.Repaint();
            
            void InstanciateObjects(Vector3 from, Vector3 to)
            {
                //create parent object for all instantiated objects
                if (GameObject.Find("Level Objects") == null)
                {
                    GameObject go = Instantiate(new GameObject(), new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
                    go.transform.name = "Level Objects";
                }

                float distance = (from - to).magnitude;

                for(int i=0; i<= distance; i++)
                {

                    Vector3 pos;
                    if (distance > 0)
                    {
                        pos = Vector3.Lerp(from, to, i / distance);
                    }
                    else
                    {
                        pos = from;
                    }
                    

                    Vector3Int cell = new Vector3Int(Mathf.RoundToInt(pos.x / cellSize.x), 0, Mathf.RoundToInt(pos.z / cellSize.z));
                    Vector3 spawnPos = cell * (int)cellSize.x;


                    // Create the prefab instance while keeping the prefab link
                    GameObject prefab = palette[paletteIndex];

                    GameObject gameObject = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
                    gameObject.transform.position = spawnPos;

                    Transform[] children = gameObject.GetComponentsInChildren<Transform>();
                    foreach(Transform child in children)
                    {
                        if (child.GetComponent<MeshRenderer>() != null)
                        {
                            Debug.Log(child.GetComponent<MeshRenderer>().bounds.min.y);
                            child.transform.Translate(new Vector3(0, child.transform.position.y - child.GetComponent<MeshRenderer>().bounds.min.y, 0));
                        }
                    }
                    
                    
                    gameObject.transform.Rotate(new Vector3(0, objectRotation, 0));

                    if (prefab.name == "Floor Unit")
                    {
                        gameObject.transform.localScale = new Vector3(gameObject.transform.localScale.x * floorXScale * (gridScale / 2), 0.1f, gameObject.transform.localScale.z * floorZScale * (gridScale / 2));

                        if (floorXScale % 2 == 0)
                        {
                            gameObject.transform.Translate(new Vector3(1f, 0, 0));
                        }
                        if (floorZScale % 2 == 0)
                        {
                            gameObject.transform.Translate(new Vector3(0, 0, 1f));
                        }

                    }

                    //draw less often if it is a wall
                    if(prefab.name == "InnerWall")
                    {
                        i += 2;
                    }

                    //set parent    
                    gameObject.transform.parent = GameObject.Find("Level Objects").transform;


                    // Allow the use of Undo (Ctrl+Z, Ctrl+Y).
                    Undo.RegisterCreatedObjectUndo(gameObject, "");
                }
                
               
            }
        }

       

        if (deleteMode) //delete objects
        {
            GameObject gameObject = GetClickedObject();

            if(gameObject!= null)
            {
                Undo.DestroyObjectImmediate(gameObject);
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
        SceneView.duringSceneGui -= this.OnSceneGUI; // Don't add twice
        SceneView.duringSceneGui += this.OnSceneGUI;

        RefreshPalette(); // Refresh the palette (can be called uselessly, but there is no overhead.)
    }

    // A list containing the available prefabs.
    [SerializeField]
    private List<GameObject> palette = new List<GameObject>();

    private string path = "Assets/Prefabs/Prototype/LevelEditorPrefabs";

    private void RefreshPalette()
    {
        palette.Clear();

        string[] prefabFiles = System.IO.Directory.GetFiles(path, "*.prefab");
        foreach (string prefabFile in prefabFiles)
            palette.Add(AssetDatabase.LoadAssetAtPath(prefabFile, typeof(GameObject)) as GameObject);
    }

    void OnDestroy()
    {
        SceneView.duringSceneGui -= this.OnSceneGUI;
        Debug.Log("OnDestroy");
    }

    private Vector3 cellSize = new Vector3(gridScale, gridScale, gridScale);

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
        Handles.color = selectColor;
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

    private GameObject GetClickedObject()
    {
        Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray,out hit,1000) && Event.current.type == EventType.MouseDown && Event.current.button == 0)
        {
            Debug.Log("Deleting:  " + hit.transform.gameObject.name);
            return hit.transform.gameObject;
        }

        return null;
    }

    
}