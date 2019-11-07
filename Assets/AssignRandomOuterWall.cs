using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ExecuteInEditMode]
public class AssignRandomOuterWall : MonoBehaviour
{

    private MeshRenderer meshRenderer;
    private MeshFilter meshFilter;
    public bool hasBeenAsigned = false;

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
            


            hasBeenAsigned = true;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
