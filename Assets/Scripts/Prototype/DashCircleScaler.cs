using UnityEngine;

[ExecuteInEditMode]
public class DashCircleScaler : MonoBehaviour
{
    private InputManager inputManager;
    public RectTransform InnerCircle;
    public RectTransform OuterCircle;

    private float ScaleFactor = 2.019f;
    public float MoveThreshold = 72f;
    public float DashThreshold = 322f;

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

        inputManager.MoveThreshold = MoveThreshold;
        inputManager.DashThreshold = DashThreshold;
    }
}
