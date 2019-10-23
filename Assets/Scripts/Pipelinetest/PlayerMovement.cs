using System.Collections;
using TMPro;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public int DashMultiplier = 2500;
    public int MoveMultiplier = 1000;
    public float CoolDownValue = 0.1f;
    public float ChargeThreshold = 0.25f;
    public float DashDuration = 0.1f;
    public float MoveDuration = 0.2f;

    //public float moveSpeed = 3f;
    //public float range = 2f;

    private Rigidbody rigidBody;
    private Material material;
    private Vector3 spawnPos;
    private readonly int maxNegativeY = -5;
    private bool isMoving;
    private float colorValue = 1;

    private Vector3 lastPosition;
    private Grid grid;
    private TrailRenderer trailRenderer;

    public float Timer { get; set; }

    public bool IsDashCharged { get; set; }

    // Start is called before the first frame update
    private void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        material = GetComponent<Renderer>().material;
        grid = FindObjectOfType<Grid>();
        trailRenderer = GetComponent<TrailRenderer>();
        trailRenderer.enabled = false;
        spawnPos = rigidBody.position;

        transform.position = grid.GetCellCenterWorld(new Vector3Int(-1, 0, -4));
    }

    // Update is called once per frame
    private void Update()
    {
        if (rigidBody.position.y < maxNegativeY)
        {
            rigidBody.position = spawnPos;
            rigidBody.velocity = Vector3.zero;
        }
    }

    public void ChargeDash()
    {
        material.color = new Color(1, colorValue, colorValue, 1);
        colorValue -= 0.05f;
    }

    public void StartDash(Vector3 dashDirection)
    {
        if (isMoving)
            return;

        trailRenderer.enabled = true;
        isMoving = true;

        rigidBody.velocity = Vector3.zero;
        rigidBody.AddForce(dashDirection * DashMultiplier);

        ResetDash();

        StartCoroutine(StartCoolDown(DashDuration));
    }

    public void ResetDash()
    {
        material.SetColor("_Color", Color.black);
        colorValue = 1f;
        IsDashCharged = false;
    }

    public void StartMove(Vector3 moveDirection)
    {
        if (isMoving)
            return;

        isMoving = true;

        rigidBody.velocity = Vector3.zero;
        rigidBody.AddForce(moveDirection * MoveMultiplier);
        StartCoroutine(StartCoolDown(MoveDuration));
    }

    IEnumerator StartCoolDown(float duration)
    {
        yield return  new WaitForSeconds(duration);
        rigidBody.velocity = Vector3.zero;
        trailRenderer.enabled = false;

        yield return new WaitForSeconds(CoolDownValue);
        isMoving = false;
    }

    //public void ContinuousMovement(Vector3 touchPosition)
    //{
    //    RaycastHit hit;
    //    Ray ray = Camera.main.ScreenPointToRay(touchPosition);
    //    if (Physics.Raycast(ray, out hit))
    //    {
    //        Vector3 targetPos = hit.point;
    //        if (Vector3.Distance(targetPos, lastPosition) > range)
    //        {
    //            targetPos.y = transform.position.y;
    //            transform.LookAt(targetPos);
    //            transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
    //        }

    //        if (lastPosition == transform.position)
    //        {
    //            Timer += Time.deltaTime;
    //            if (Timer >= ChargeThreshold)
    //            {
    //                ChargeDash();
    //                IsDashCharged = true;
    //            }
    //        }
    //        else
    //        {
    //            if (!IsDashCharged)
    //            {
    //                ResetDash();
    //                Timer = 0;
    //            }
    //            else
    //            {
    //                Vector3 dashDirection = (transform.position - lastPosition).normalized;
    //                StartDash(dashDirection);
    //            }
    //        }

    //        lastPosition = transform.position;
    //    }
    //}
}
