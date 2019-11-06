using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "RoomInformation", menuName = "ScriptableObjects/PuzzlesInfo")]
public class RoomsStorage : ScriptableObject
{
    public bool firstPuzzleCompleted;
    public bool secondPuzzleCompleted;
    public bool thirdPuzzleCompleted;
    public bool levelCompleted;
}
