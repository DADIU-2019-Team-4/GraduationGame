using SplineMesh;
using UnityEngine;

[ExecuteInEditMode]
public class ScaleSpline : MonoBehaviour
{
    private Spline spline;
    private BoxCollider[] boxColliders;

    private float startScale = 1;
    public float Scale = 1;
    public bool ScaleHitBoxes = false;

    private void OnEnable()
    {
        spline = GetComponent<Spline>();
        boxColliders = GetComponentsInChildren<BoxCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        if (spline == null)
            return;

        // Scales the spline
        foreach (CubicBezierCurve curve in spline.GetCurves())
        {
            curve.n1.Scale = Vector2.one * (startScale + (Scale - startScale));
            curve.n2.Scale = Vector2.one * (startScale + (Scale - startScale));
        }
    
        foreach (BoxCollider boxCollider in boxColliders)
        {
            if (ScaleHitBoxes)
            {
                boxCollider.size = new Vector3(Scale, Scale, Scale);
            }else
            {
                boxCollider.size = new Vector3(startScale, startScale, startScale);
            }
        }
    }
}
