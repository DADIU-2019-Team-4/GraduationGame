using UnityEngine;
using SplineMesh;

[ExecuteInEditMode]
public class UpdateStartEndPoints : MonoBehaviour
{
    private Spline spline;

    private Transform startPoint;
    private Transform endPoint;

    // Update is called once per frame
    void Update()
    {
        startPoint = transform.Find("StartPoint");
        endPoint = transform.Find("EndPoint");
        spline = GetComponent<Spline>();

        if (spline == null)
            return;

        startPoint.position = transform.position + spline.nodes[0].Position;
        endPoint.position = transform.position + spline.nodes[spline.nodes.Count - 1].Position;
    }
}
