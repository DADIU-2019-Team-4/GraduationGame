using UnityEngine;
using System.Collections.Generic;

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
    private bool hasStarted;

    private List<AudioEvent> audioEvents;

    public ParticleSystem ParticlesOnCollision;

    private void Awake()
    {
        particleSystem = GetComponent<ParticleSystem>();
        audioEvents = new List<AudioEvent>(GetComponents<AudioEvent>());
    }

    private void Start()
    {
        particleSystem.Stop();
    }

    private void Update()
    {
        if (StartOffsetTimer > 0)
        {
            StartOffsetTimer -= Time.deltaTime;

        }
        else if (!hasStarted)
        {
            if (StartTurnedOn)
                TurnOn();
            else
                TurnOff();

            hasStarted = true;
        }

        else
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

    }

    private void TurnOn()
    {
        particleSystem.Play();
        isActivated = true;
        timer = 0;
        AudioEvent.SendAudioEvent(AudioEvent.AudioEventType.WaterSprayOn, audioEvents, gameObject);
    }

    private void TurnOff()
    {
        particleSystem.Stop();
        isActivated = false;
        timer = 0;
        AudioEvent.SendAudioEvent(AudioEvent.AudioEventType.WaterSprayOff, audioEvents, gameObject);
    }

    private void OnParticleCollision(GameObject col)
    {
        if (col.CompareTag("Player"))
        {
            MovementController movementController = col.GetComponent<MovementController>();
            if (!movementController.DamageCoolDownActivated)
            {
                movementController.DamageCoolDownActivated = true;

                // damage player
                InteractibleObject interactibleObject = GetComponent<InteractibleObject>();
                if (interactibleObject != null && interactibleObject.type == InteractibleObject.InteractType.Damage)
                    interactibleObject.Interact(transform.position);
            }

            if (movementController.IsFuseMoving)
                return;

            // bump player back
            if (ParticlesOnCollision != null)
            {
                Vector3 position = movementController.transform.position;
                position.y = 0.5f;
                Instantiate(ParticlesOnCollision, position, Quaternion.identity);
            }

            Vector3 targetPos = movementController.transform.position -
                                (movementController.transform.forward * movementController.DamageBounceValue);
            targetPos.y = 0;
            StartCoroutine(movementController.MoveBackRoutine(targetPos, movementController.MoveDuration));
        }
    }
}
