using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ExecuteInEditMode]
public class AssignRandomOuterWall : MonoBehaviour
{

    private MeshRenderer meshRenderer;
    private MeshFilter meshFilter;
    private Renderer rend;
    public bool hasBeenAsigned = false;

    public List<Mesh> meshes;

    public Material bark;
    public Material window;
    
    private void Awake()
    {




    }
    private void OnEnable()
    {
        if (!hasBeenAsigned && gameObject.activeInHierarchy)
        {
            meshRenderer = GetComponent<MeshRenderer>();
            meshFilter = GetComponent<MeshFilter>();
            rend = GetComponent<Renderer>();

            int random = Mathf.RoundToInt(Random.value * (meshes.Count - 1));

            meshFilter.mesh = meshes[random];
            
            if(meshFilter.sharedMesh.name == "wall_outerTree01")
            {
                Material[] mat = new Material[2] { rend.sharedMaterial ,bark};
                rend.sharedMaterials = mat;
            }
            else if (meshFilter.sharedMesh.name == "cons_wall_window01")
            {
                Material[] mat = new Material[2] { rend.sharedMaterial, window};
                Debug.Log("YESYES");
                rend.sharedMaterials = mat;
            }

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
