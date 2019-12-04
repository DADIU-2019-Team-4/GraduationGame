
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoMa
{
    public class MovementComponent : MonoBehaviour
    {
        public static readonly Vector3 StartingOffset = new Vector3(0, 0, -1);
        public static readonly Vector3 DefaultScale = new Vector3(2, 2, 2);

        public int playerId = 0;

        private Transform _transform;
        [SerializeField]
        private Vector3 _velocity = new Vector3();
        private List<Target> _targets = new List<Target>();
        private float _speed;
        private bool _disappeared = false;

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
                switch (_targets[0].type)
                {
                    case MovementController.EventType.Move:
                    case MovementController.EventType.Dash:
                    case MovementController.EventType.Die:
                    case MovementController.EventType.Win:

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
                        if (_transform.position == _targets[0].position)
                        {
                            _targets.RemoveAt(0);
                        }

                        _transform.rotation = Quaternion.LookRotation(_velocity, Vector3.up);

                        //Vector2 direction = _velocity.GetXZVector2().normalized;
                        //float rotationAngle = Vector2.SignedAngle(Vector2.up, direction);
                        //_transform.eulerAngles = new Vector3(0, -rotationAngle, 0);
                        break;
                    case MovementController.EventType.EnterPaperPlane:
                    case MovementController.EventType.EnterFuse:
                        Disappear();
                        _targets.RemoveAt(0);
                        break;
                    case MovementController.EventType.ExitPaperPlane:
                    case MovementController.EventType.ExitFuse:
                        Reappear(_targets[0].position);
                        _targets.RemoveAt(0);
                        break;
                    default:
                        Debug.LogWarning("Sally's MovementComponent recieved an unexpected target: " + _targets[0].type);
                        break;
                }
            }
        }

        public void AddTarget(MovementController.EventType type, Vector3 position)
        {
            switch (type)
            {
                case MovementController.EventType.Move:
                case MovementController.EventType.Dash:
                    // Consider all movements Move (not Dash)
                    _targets.Add(new Target(MovementController.EventType.Move, position));
                    break;
                case MovementController.EventType.Die:
                    // Ideally, there would be some other animation
                    _targets.Add(new Target(MovementController.EventType.Move, position));
                    break;
                case MovementController.EventType.Win:
                    // Ideally, there would be some other animation
                    _targets.Add(new Target(MovementController.EventType.Move, position));
                    break;
                case MovementController.EventType.Respawn:
                    Respawn(position);
                    break;
                case MovementController.EventType.EnterFuse:
                    _targets.Add(new Target(MovementController.EventType.EnterFuse, position));
                    break;
                case MovementController.EventType.ExitFuse:
                    _targets.Add(new Target(MovementController.EventType.ExitFuse, position));
                    break;
                case MovementController.EventType.EnterPaperPlane:
                    _targets.Add(new Target(MovementController.EventType.EnterPaperPlane, position));
                    break;
                case MovementController.EventType.ExitPaperPlane:
                    _targets.Add(new Target(MovementController.EventType.ExitPaperPlane, position));
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
                if (_targets.Count > 1)
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
                    simulatedPosition == _targets[currentTarget].position
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
                (target - current).magnitude / _speed,
                _speed
                );

            return destination;
        }

        /// <summary>
        /// Respawns Sally behind Lucy
        /// </summary>
        private void Respawn(Vector3 position)
        {
            _targets.Clear();
            _transform.position = position + StartingOffset;
            _transform.rotation = Quaternion.identity;
            _disappeared = false;
        }

        /// <summary>
        /// Hides Sally until Reappear is called. Handles Fuses and PaperPlanes
        /// </summary>
        private void Disappear()
        {
            // Only run the first time this is called (to handle intro scene)
            if (!_disappeared)
            {
                _disappeared = true;
                _transform.localScale = new Vector3(0, 0, 0);
            }
        }

        /// <summary>
        /// Shows Sally again after Disappear has hiden her. Handles Fuses and PaperPlanes
        /// </summary>
        private void Reappear(Vector3 position)
        {
            // Only run the first time this is called (to handle intro scene)
            if (_disappeared)
            {
                _transform.position = position;

                _disappeared = false;
                _transform.localScale = DefaultScale;
            }
        }

        private class Target
        {
            public MovementController.EventType type;
            public Vector3 position;

            public Target(MovementController.EventType type, Vector3 position)
            {
                this.type = type;
                this.position = position;
            }
        }
    }
}