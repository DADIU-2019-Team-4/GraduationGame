using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "QAVariables", menuName = "ScriptableObjects/QA")]
public class DataContainer : ScriptableObject
{
    public int NormalDashCount;
    public int ChargedDashCount;
    public int DestroyedObjectsCount;
    public int RopeUseCount;
    public int DeathsCount;
    public List<string> levelName;
    public List<Vector3> deathPlace;
}