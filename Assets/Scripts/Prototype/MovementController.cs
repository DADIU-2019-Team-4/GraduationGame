using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MovementController : MonoBehaviour
{
    public float CoolDownValue = 0.1f;
    public float ChargeThreshold = 0.25f;
    public float DashDuration = 0.1f;
    public float MoveDuration = 0.2f;

    public int MoveDistance = 1;
    public int DashDistance = 2;
    public int MoveCost = 1;
    public int DashCost = 3;

    public int PickUpValue = 3;
    public int AmountOfMoves = 10;
    public TMP_Text MovesText;
    public GameObject OutOfMovesText;
    public GameObject WinText;
    public GameObject RestartButton;

    private Rigidbody rigidBody;
    private Material material;
    private float colorValue = 1;
    private float changeTextColorDuration = 0.2f;

    private Grid grid;
    private TrailRenderer trailRenderer;
    private Vector3Int previousCell;

    private bool isMoving;
    private bool isDashing;
    private bool isOutOfMoves;
    private bool reachedGoal;

    public float Timer { get; set; }

    public bool IsDashCharged { get; set; }

    // Start is called before the first frame update
    private void Start()
    {
        MovesText.text = AmountOfMoves.ToString();

        rigidBody = GetComponent<Rigidbody>();
        material = GetComponent<Renderer>().material;
        grid = FindObjectOfType<Grid>();
        trailRenderer = GetComponent<TrailRenderer>();
        trailRenderer.enabled = false;

        Vector3Int cell = grid.WorldToCell(transform.position);
        transform.position = grid.GetCellCenterWorld(cell);
    }

    private void MakeRestartButtonVisible()
    {
        if (reachedGoal)
            WinText.SetActive(true);
        else
            OutOfMovesText.SetActive(true);

        RestartButton.SetActive(true);
    }

    public void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ChargeDash()
    {
        material.color = new Color(1, colorValue, colorValue, 1);
        colorValue -= 0.05f;
    }

    public void StartMove(Vector3Int moveDirection)
    {
        if (isOutOfMoves || reachedGoal)
            return;

        if (isMoving)
            return;

        UpdateMovesText(MoveCost);

        var startCell = grid.WorldToCell(transform.position);
        previousCell = startCell;
        var difference = moveDirection * MoveDistance;
        var targetCell = startCell + difference;

        StartCoroutine(MoveRoutine(targetCell, MoveDuration));
    }

    public void StartDash(Vector3Int dashDirection)
    {
        if (isOutOfMoves || reachedGoal)
            return;

        if (isMoving)
            return;

        int movesLeft = AmountOfMoves - DashCost;
        if (movesLeft < 0)
        {
            StartCoroutine(ChangeTextColorRoutine());
            return;
        }

        UpdateMovesText(DashCost);

        isDashing = true;
        trailRenderer.enabled = true;

        var startCell = grid.WorldToCell(transform.position);
        var difference = dashDirection * DashDistance;
        var targetCell = startCell + difference;

        StartCoroutine(MoveRoutine(targetCell, DashDuration));

        ResetDash();
    }

    public void ResetDash()
    {
        material.SetColor("_Color", Color.black);
        colorValue = 1f;
        IsDashCharged = false;
    }

    private IEnumerator MoveRoutine(Vector3Int target, float duration)
    {
        isMoving = true;

        var toPosition = grid.GetCellCenterWorld(target);
        rigidBody.DOMove(toPosition, duration);

        yield return new WaitForSeconds(duration);

        trailRenderer.enabled = false;

        yield return new WaitForSeconds(CoolDownValue);
        isMoving = false;

        if (isDashing)
            isDashing = false;
    }

    private IEnumerator ChangeTextColorRoutine()
    {
        MovesText.DOColor(Color.red, changeTextColorDuration);
        yield return new WaitForSeconds(changeTextColorDuration);
        MovesText.DOColor(Color.white, changeTextColorDuration);
        yield return new WaitForSeconds(changeTextColorDuration);
    }

    private void UpdateMovesText(int cost)
    {
        AmountOfMoves -= cost;
        MovesText.text = AmountOfMoves.ToString();
        if (AmountOfMoves <= 0)
        {
            isOutOfMoves = true;
            MakeRestartButtonVisible();
        }
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("Goal"))
        {
            reachedGoal = true;
            MakeRestartButtonVisible();
        }
        else if (col.gameObject.CompareTag("Obstacle"))
        {
            if (!isDashing)
            {
                AmountOfMoves += MoveCost;
                MovesText.text = AmountOfMoves.ToString();
                StartCoroutine(MoveRoutine(previousCell, MoveDuration));
            }
        }
        else if (col.gameObject.CompareTag("PickUp"))
        {
            AmountOfMoves += PickUpValue;
            MovesText.text = AmountOfMoves.ToString();
            Destroy(col.gameObject);
        }
    }
}
