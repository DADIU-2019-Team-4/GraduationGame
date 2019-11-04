using SplineMesh;
using UnityEngine;

[ExecuteInEditMode]
public class ScaleSpline : MonoBehaviour
{
    private Spline spline;

    private float startScale = 1;
    public float Scale = 1;

    // Update is called once per frame
    void Update()
    {
        spline = GetComponent<Spline>();
        if (spline == null)
            return;

        // Scales the spline
        foreach (CubicBezierCurve curve in spline.GetCurves())
        {
            curve.n1.Scale = Vector2.one * (startScale + (Scale - startScale));
            curve.n2.Scale = Vector2.one * (startScale + (Scale - startScale));
        }
    }
}
