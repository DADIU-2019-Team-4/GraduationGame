using UnityEngine;
using System.Collections;

public class ExampleBehavior : MonoBehaviour
{
    [DraggablePoint] public Vector3 SpawnPosition;
    [DraggablePoint] public Vector3 SpawnPosition2;
    [DraggablePoint] public Vector3 SpawnPosition3;
    public GameObject SpawnableObject;

    public void Spawn()
    {
        Instantiate(SpawnableObject, SpawnPosition, Quaternion.identity);
    }
}