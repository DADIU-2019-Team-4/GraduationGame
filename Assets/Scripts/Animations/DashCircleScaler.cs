using UnityEngine;

[ExecuteInEditMode]
public class DashCircleScaler : MonoBehaviour
{
    private InputManager _inputManager;
    public RectTransform OuterCircle;
    public SpriteRenderer InnerCircle;
    public float OuterScaleFactor = 2.019f;
    public float InnerScaleFactor = 2.5f;

    [Header("Use this to set the size of the input circles")]
    public float MoveThreshold = 50f;
    public float DashThreshold = 200f;

    private void Awake()
    {
        _inputManager = FindObjectOfType<InputManager>();
    }

    private void Start()
    {
        _inputManager.MoveThreshold = MoveThreshold;
        _inputManager.DashThreshold = DashThreshold;
    }

    // Update is called once per frame
    void Update()
    {
#if UNITY_EDITOR
        float newMoveScale = _inputManager.MoveThreshold * InnerScaleFactor;
        float newDashScale = _inputManager.DashThreshold * OuterScaleFactor;
        OuterCircle.sizeDelta = new Vector2(newDashScale, newDashScale);
        InnerCircle.size = new Vector2(newMoveScale, newMoveScale);

        _inputManager.MoveThreshold = MoveThreshold;
        _inputManager.DashThreshold = DashThreshold;
#endif
    }
}
