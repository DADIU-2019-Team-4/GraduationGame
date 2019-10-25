using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CustomEditors : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

[CustomEditor(typeof(WallShooter))]
public class SomeScriptEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();


        WallShooter wallShooter = (WallShooter)target;
        if (GUILayout.Button("spawn 'very cool' plane"))
        {
            wallShooter.SpawnPlane();
        }
    }
}

