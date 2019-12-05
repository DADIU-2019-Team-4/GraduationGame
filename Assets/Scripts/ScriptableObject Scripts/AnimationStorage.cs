using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AnimationStorage", menuName = "ScriptableObjects/AnimationStorage")]
public class AnimationStorage : ScriptableObject
{
    public TextAsset[] MoMaCSV;
    public GameObject[] Models;

}
