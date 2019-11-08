using UnityEngine;

[ExecuteInEditMode]
public class DashCircleScaler : MonoBehaviour
{
    private InputManager inputManager;
    public RectTransform InnerCircle;
    public RectTransform OuterCircle;
    public float ScaleFactor = 2.019f;

    [Header("Use this to set the size of the input circles")]
    public float MoveThreshold = 72f;
    public float DashThreshold = 322f;

    private void Awake()
    {
        inputManager = FindObjectOfType<InputManager>();
    }

    private void Start()
    {
        inputManager.MoveThreshold = MoveThreshold;
        inputManager.DashThreshold = DashThreshold;
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
