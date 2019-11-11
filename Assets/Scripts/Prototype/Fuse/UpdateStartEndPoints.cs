using UnityEngine;
using SplineMesh;

[ExecuteInEditMode]
public class UpdateStartEndPoints : MonoBehaviour
{
    private Spline spline;

    private Transform startPoint;
    private Transform endPoint;

    private void OnEnable()
    {
        spline = GetComponent<Spline>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!Application.isPlaying)
        {
            if (spline == null)
                return;

            startPoint = transform.Find("StartPoint");
            endPoint = transform.Find("EndPoint");

            startPoint.position = transform.position + spline.nodes[0].Position;
            endPoint.position = transform.position + spline.nodes[spline.nodes.Count - 1].Position;
        }

        
    }
}
