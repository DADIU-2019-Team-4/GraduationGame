using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SplineMesh;

[ExecuteInEditMode]
public class UpdateStartEndPoints : MonoBehaviour
{
    // Start is called before the first frame update

    private Transform startPoint;
    private Transform endPoint;
    private Spline spline;

    private void Awake()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        startPoint = transform.Find("StartPoint");
        endPoint = transform.Find("EndPoint");
        spline = GetComponent<Spline>();
        
        startPoint.position = transform.position + spline.nodes[0].Position;
        endPoint.position = transform.position + spline.nodes[spline.nodes.Count-1].Position;
    }
}
