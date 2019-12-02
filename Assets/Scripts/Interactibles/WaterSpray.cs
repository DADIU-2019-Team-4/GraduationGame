using UnityEngine;
using System.Collections.Generic;

public class WaterSpray : MonoBehaviour
{
    public float OnTimer = 4;
    public float OffTimer = 5;
    private float _timer;
    private ParticleSystem _particleSystem;
    private BoxCollider _boxCollider;
    private List<ParticleCollisionEvent> _collisionEvents = new List<ParticleCollisionEvent>();
    private bool _isActivated;
    public bool StartTurnedOn = true;

    public float StartOffsetTimer;
    private bool _hasStarted;

    private List<AudioEvent> _audioEvents;

    public ParticleSystem ParticlesOnCollision;
    public float WaterForce = 10;
    private float _minColliderXValue = 0.1f;
    private float _maxColliderXValue = 3f;
    private float _colliderExpandTimer;
    private float _colliderExpandSpeed = 5;

    private void Awake()
    {
        _particleSystem = GetComponent<ParticleSystem>();
        _audioEvents = new List<AudioEvent>(GetComponents<AudioEvent>());
        _boxCollider = GetComponent<BoxCollider>();
    }

    private void Start()
    {
        _particleSystem.Stop();
        _boxCollider.enabled = false;
    }

    private void Update()
    {
        if (StartOffsetTimer > 0)
        {
            StartOffsetTimer -= Time.deltaTime;
        }
        else if (!_hasStarted)
        {
            if (StartTurnedOn)
                TurnOn();
            else
                TurnOff();

            _hasStarted = true;
        }
        else
        {
            _timer += Time.deltaTime;
            if (_isActivated)
            {
                ExpandCollider();
                if (_timer >= OnTimer)
                    TurnOff();
            }
            else
            {
                if (_timer >= OffTimer)
                    TurnOn();
            }
        }

    }

    private void TurnOn()
    {
        _particleSystem.Play();
        _boxCollider.enabled = true;
        _isActivated = true;
        _timer = 0;
        AudioEvent.SendAudioEvent(AudioEvent.AudioEventType.WaterSprayOn, _audioEvents, gameObject);
    }

    private void TurnOff()
    {
        _particleSystem.Stop();
        _boxCollider.size = new Vector3(_minColliderXValue, _boxCollider.size.y, _boxCollider.size.z);
        _colliderExpandTimer = 0;
        _boxCollider.enabled = false;
        _isActivated = false;
        _timer = 0;
        AudioEvent.SendAudioEvent(AudioEvent.AudioEventType.WaterSprayOff, _audioEvents, gameObject);
    }

    private void ExpandCollider()
    {
        _colliderExpandTimer += Time.deltaTime * _colliderExpandSpeed;
        float value = Mathf.Lerp(_minColliderXValue, _maxColliderXValue, _colliderExpandTimer);
        _boxCollider.size = new Vector3(value, _boxCollider.size.y, _boxCollider.size.z);
    }

    private void OnParticleCollision(GameObject col)
    {
        if (col.CompareTag("Player"))
        {
            MovementController movementController = col.GetComponent<MovementController>();
            Rigidbody playerRigidbody = col.GetComponent<Rigidbody>();
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
                AudioEvent.SendAudioEvent(AudioEvent.AudioEventType.HurtPlayer, _audioEvents, gameObject);
            }

            if (movementController.IsMoving)
            {
                Vector3 targetPos = movementController.transform.position -
                                    movementController.transform.forward * movementController.DamageBounceValue;
                targetPos.y = 0;
                StartCoroutine(movementController.MoveBackRoutine(targetPos, MovementController.MoveDuration));
            }
            else
            {
                _particleSystem.GetCollisionEvents(col, _collisionEvents);
                Vector3 collisionForce = _collisionEvents[0].velocity;
                collisionForce.y = 0;
                playerRigidbody.AddForce(collisionForce * WaterForce);
            }
        }
    }
}
