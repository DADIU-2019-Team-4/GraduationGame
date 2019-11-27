
using System;
using System.Collections.Generic;
using UnityEngine;

namespace MoMa
{
    public class MovementComponent
    {
        public int playerId = 0;

        private Transform _transform;
        [SerializeField]
        private Vector3 _velocity = new Vector3();
        private List<Target> _targets = new List<Target>();
        private float _speed;

        public MovementComponent(Transform transform)
        {
            this._transform = transform;
        }

        // Update is called once per frame
        public void Update()
        {
            // Move until you reach FireGirl (in 1 move distance)
            if (_targets.Count > 1)
            {
                this._speed = Input.GetKey(KeyCode.LeftShift) ?
                    SalamanderController.RunningSpeed :
                    SalamanderController.WalkingSpeed;

                // Move to target position (modifies the velocity)
                _transform.position = Step(
                    _transform.position,
                    _targets[0].position,
                    ref _velocity
                    );

                // If the target is reached, remove target
                if (_transform.position.GetXZVector2() == _targets[0].position)
                {
                    _targets.RemoveAt(0);
                }

                // Rotate to face the direction moving
                Vector2 direction = _velocity.GetXZVector2().normalized;
                float rotationAngle = Vector2.SignedAngle(Vector2.up, direction);
                _transform.eulerAngles = new Vector3(0, -rotationAngle, 0);
            }
        }

        public void UpdateTargets(MovementController.EventType type, Vector2 position)
        {
            switch (type)
            {
                case MovementController.EventType.Move:
                case MovementController.EventType.Dash:
                    // Consider all movements Move (not Dash)
                    _targets.Add(new Target(MovementController.EventType.Move, position));
                    break;

                case MovementController.EventType.Die:
                    break;
                case MovementController.EventType.Win:
                    break;
                case MovementController.EventType.Respawn:
                    break;
                case MovementController.EventType.EnterFuse:
                    break;
                case MovementController.EventType.ExitFuse:
                    break;
                case MovementController.EventType.EnterPaperPlane:
                    break;
                case MovementController.EventType.ExitPaperPlane:
                    break;
                default:
                    break;
            }
        }

        public List<(Vector3, Quaternion)> GetFuture(int afterFrames)
        {
            // Initialize the simulated position and velocity to the value of the current ones
            Vector3 simulatedPosition = _transform.position;
            Vector3 simulatedVelocity = _velocity;
            List<(Vector3, Quaternion)> future = new List<(Vector3, Quaternion)>();
            int currentTarget = 0;

            for (int i = 0; i < afterFrames; i++)
            {
                // Calculate next position (if one exists)
                if (_targets.Count > 0)
                {
                    simulatedPosition = Step(
                        simulatedPosition,
                        _targets[currentTarget].position,
                        ref simulatedVelocity
                        );
                }

                // Add it to the list
                future.Add((simulatedPosition, Quaternion.Euler(simulatedVelocity.normalized)));

                // If the current target is reached, use the next one (if one exists)
                if (
                    currentTarget + 1 < _targets.Count &&
                    simulatedPosition.GetXZVector2() == _targets[currentTarget].position
                    )
                {
                    currentTarget++;
                }
            }

            return future;
        }

        private Vector3 Step(Vector3 current, Vector3 target, ref Vector3 currentVelocity)
        {
            Vector3 destination = Vector3.SmoothDamp(
                current,
                target,
                ref currentVelocity,
                (target-current).magnitude / _speed,
                _speed
                );

            return destination;
        }

        private class Target
        {
            public MovementController.EventType type;
            public Vector2 position;

            public Target(MovementController.EventType type, Vector2 position)
            {
                this.type = type;
                this.position = position;
            }
        }
    }
}