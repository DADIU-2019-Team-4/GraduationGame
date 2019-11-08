using UnityEngine;

[ExecuteInEditMode]
public class DashCircleScaler : MonoBehaviour
{
    private InputManager inputManager;
    public RectTransform InnerCircle;
    public RectTransform OuterCircle;

    public float ScaleFactor = 2.019f;

    private void Awake()
    {
        inputManager = FindObjectOfType<InputManager>();
    }

    // Update is called once per frame
    void Update()
    {
        float newMoveScale = inputManager.MoveThreshold * ScaleFactor;
        float newDashScale = inputManager.DashThreshold * ScaleFactor;
        InnerCircle.sizeDelta = new Vector2(newMoveScale, newMoveScale);
        OuterCircle.sizeDelta = new Vector2(newDashScale, newDashScale);
    }
}
