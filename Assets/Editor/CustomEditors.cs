using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CustomEditors : MonoBehaviour
{

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

