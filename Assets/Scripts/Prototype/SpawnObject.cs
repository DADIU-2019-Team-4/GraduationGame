using UnityEngine;

[ExecuteInEditMode]
public class SpawnObject : MonoBehaviour
{
    private Grid grid;

    // Start is called before the first frame update
    void Start()
    {
        grid = FindObjectOfType<Grid>();
        Vector3Int cell = grid.WorldToCell(transform.position);
        transform.position = grid.GetCellCenterWorld(cell);
    }
}
