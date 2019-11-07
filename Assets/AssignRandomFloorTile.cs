using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class AssignRandomFloorTile : MonoBehaviour
{

    private MeshRenderer meshRenderer;
    private MeshFilter meshFilter;
    public bool hasBeenAsigned = false;

    public List<Mesh> meshes;


    private void Awake()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        if (!hasBeenAsigned)
        {
            meshRenderer = GetComponent<MeshRenderer>();
            meshFilter = GetComponent<MeshFilter>();

            int random = Mathf.RoundToInt(Random.value * (meshes.Count - 1));

            meshFilter.mesh = meshes[random];

            //transform.Rotate(new Vector3(0, 90 * Mathf.RoundToInt(Random.Range(0, 3)), 0));
        }


        hasBeenAsigned = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
