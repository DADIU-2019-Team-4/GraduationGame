using UnityEngine;

public class WaterSpray : IGameLoop
{
    public float OnTimer = 4;
    public float OffTimer = 5;
    private float timer;
    private ParticleSystem particleSystem;
    private BoxCollider boxCollider;
    private bool isActivated;
    public bool StartTurnedOn = true;

    private void Awake()
    {
        particleSystem = GetComponent<ParticleSystem>();
        boxCollider = GetComponent<BoxCollider>();
    }

    private void Start()
    {
        if (StartTurnedOn)
            isActivated = true;
        else
            TurnOff();
    }

    public override void GameLoopUpdate()
    {
        timer += Time.deltaTime;
        if (isActivated)
        {
            if (timer >= OnTimer)
                TurnOff();
        }
        else
        {
            if (timer >= OffTimer)
                TurnOn();
        }
    }

    private void TurnOn()
    {
        particleSystem.Play();
        isActivated = true;
        boxCollider.enabled = true;
        timer = 0;
    }

    private void TurnOff()
    {
        particleSystem.Stop();
        isActivated = false;
        boxCollider.enabled = false;
        timer = 0;
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            MovementController movementController = col.GetComponent<MovementController>();
            StartCoroutine(
                movementController.MoveBackRoutine(movementController.transform.position - movementController.transform.forward *
                                                   movementController.DamageBounceValue,
                    movementController.MoveDuration));
        }
    }
}
