using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class AssignRandomFloorTile : MonoBehaviour
{

    private MeshRenderer meshRenderer;
    private MeshFilter meshFilter;
    public bool hasBeenAsigned = false;
    public float rotationY;

    public List<Mesh> meshes;

    

    private void Awake()
    {
        


        
    }
    private void OnEnable()
    {
        if (!hasBeenAsigned && gameObject.activeInHierarchy)
        {
            meshRenderer = GetComponent<MeshRenderer>();
            meshFilter = GetComponent<MeshFilter>();

            int random = Mathf.RoundToInt(Random.value * (meshes.Count - 1));

            meshFilter.mesh = meshes[random];

            rotationY = 90 * Mathf.RoundToInt(Random.Range(0, 4));
            

            hasBeenAsigned = true;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        transform.eulerAngles = new Vector3(0, rotationY, 0);
    }

    // Update is called once per frame
    void Update()
    {
    }
}
