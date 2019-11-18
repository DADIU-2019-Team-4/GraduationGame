using UnityEngine;

public class WaterSpray : MonoBehaviour
{
    public float OnTimer = 4;
    public float OffTimer = 5;
    private float timer;
    private ParticleSystem particleSystem;
    private BoxCollider boxCollider;
    private bool isActivated;
    public bool StartTurnedOn = true;

    public float StartOffsetTimer;

    public ParticleSystem ParticlesOnCollision;

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

        timer -= StartOffsetTimer;
    }

    private void Update()
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
            if (ParticlesOnCollision != null)
            {
                Vector3 position = movementController.transform.position;
                position.y = 0.5f;
                Instantiate(ParticlesOnCollision, position, Quaternion.identity);
            }

            StartCoroutine(
                movementController.MoveBackRoutine(movementController.transform.position - movementController.transform.forward *
                                                   movementController.DamageBounceValue, MovementController.MoveDuration));

        }
    }
}
