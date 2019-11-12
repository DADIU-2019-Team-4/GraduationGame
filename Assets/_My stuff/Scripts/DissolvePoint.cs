using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DissolvePoint : MonoBehaviour
{
    public Vector3 DissolvePosStart;
    public void OnValidate()
    {
        GetComponent<Renderer>().sharedMaterial.SetVector("_StartPoint", DissolvePosStart);
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
