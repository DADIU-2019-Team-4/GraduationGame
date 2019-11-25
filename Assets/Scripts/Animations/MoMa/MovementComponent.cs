
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
        private Vector3 _target = new Vector3(0, 0, 0);
        private float _speed;

        public MovementComponent(Transform transform)
        {
            this._transform = transform;
        }

        // Update is called once per frame
        public void Update()
        {
            this._speed = Input.GetKey(KeyCode.LeftShift) ?
                CharacterController.RunningSpeed :
                CharacterController.WalkingSpeed;

            // Move to target position (modifies the velocity)
            _transform.position = Step(
                _transform.position,
                _target,
                ref _velocity
                );

            // Rotate to face the direction moving
            Vector2 direction = _velocity.GetXZVector2().normalized;
            float rotationAngle = Vector2.SignedAngle(Vector2.up, direction);
            _transform.eulerAngles = new Vector3(0, -rotationAngle, 0);
        }

        public void UpdateTarget(Vector3 newTarget)
        {
            _target = newTarget;
        }

        public List<(Vector3, Quaternion)> GetFuture(int afterFrames)
        {
            // Initialize the simulated position and velocity to the value of the current ones
            Vector3 simulatedPosition = _transform.position;
            Vector3 simulatedVelocity = _velocity;
            List<(Vector3, Quaternion)> future = new List<(Vector3, Quaternion)>();

            for (int i = 0; i < afterFrames; i++)
            {
                // Calculate next position
                simulatedPosition = Step(
                    simulatedPosition,
                    _target,
                    ref simulatedVelocity
                    );

                // Add it to the list
                future.Add((simulatedPosition, Quaternion.Euler(simulatedVelocity.normalized)));
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
    }
}