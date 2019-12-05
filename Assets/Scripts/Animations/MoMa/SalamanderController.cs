using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine.Profiling;

namespace MoMa
{
    public class SalamanderController : MonoBehaviour
    {
        #region Vars
        // Flags whether we should recalculate Animations with Packer or load the existing ones (stored as Assets)
        public const bool RecalculateAnimationsFlag = true;

        // Fine-tuning
        public const float RecalculationThreshold = 0.7f; // The maximum diff of two Trajectories before recalculating the Animation
        //public const float RecalculationThreshold = Mathf.Infinity; // The maximum diff of two Trajectories before recalculating the Animation
        public const int CooldownTime = 500; // Number of frames that a Frame is on cooldown after being played
        public const int CandidateFramesSize = 20; // Number of candidate frames for a transition (tradeoff: fidelity/speed)
        public const int ClipBlendPoints = 0; // Each Animation Clip is blended with the next one for smoother transition. The are both played for this num of Frames

        // Frame/Point/Feature ratios
        // FeaturePoints % FeatureEveryPoints should be 0
        public const int SkipFrames = 3;  // 3; Take 1 Frame every SkipFrames in the Animation file
        public const int FeaturePoints = 4;  // 3; Trajectory.Points per Feature. The lower the number, the shorter time the Feature covers
        public const int FeaturePastPoints = 2;  // The number of Points in the past that is used in a Snippet. The lower the number, the lower the fidelity
        public const int FeatureEveryPoints = 3;  // Trajectory.Points per Feature. The lower the nuber, the shorter time the Feature covers
        // FramesPerPoint % 2 should be 0
        public const int FramesPerPoint = 12;    // 4; Animation.Frames per Trajectory.Point. The lower the number, the denser the Trajectory points will be.

        public const int FramesPerFeature = FramesPerPoint * FeaturePoints;  // Animation.Frames per Feature
        public const int FeatureStep = FeaturePoints / FeatureEveryPoints;  // Features overlap generally. This is the distance between two matching Features.
        public const int SnippetSize = FeaturePoints + FeaturePastPoints;

        // Movement
        public const float DefaultDampTime = 1f;
        public const float StopDampTime = 3f;
        public const float WalkingSpeed = 2.40f;    // 1.70f;
        public const float RunningSpeed = 2.4f;

        private MovementComponent _mc;
        private FollowerComponent _fc;
        private RuntimeComponent _rc;
        private AnimationComponent _ac;
        private Trajectory _trajectory = new Trajectory();
        private Transform _model;
        private int _currentFrame = 0;
        private bool _inCutscene = false;

        #endregion

        void Start()
        {
            List<string> animationFiles = new List<string>();
            animationFiles.Add("take-1_DEFAULT_C26");
            animationFiles.Add("take-2_DEFAULT_C26");
            animationFiles.Add("take-3_DEFAULT_C26");
            animationFiles.Add("take-4_DEFAULT_C26");
            animationFiles.Add("take-5_DEFAULT_C26");
            animationFiles.Add("take-6_DEFAULT_C26");

            // We assume that the Character has the correct structure
            this._model = this.gameObject.transform;
            this._mc = new MovementComponent(this._model);
            this._fc = new FollowerComponent(this._model);
            this._ac = new AnimationComponent(this._model);
            this._rc = new RuntimeComponent(this._fc, animationFiles, RecalculateAnimationsFlag);

            if (this._model == null)
            {
                throw new System.Exception("SalamanderController was unable to find the model.");
            }

            // Initialize Trajectory's past to the initial position
            for (int i = 0; i < FeaturePastPoints; i++)
            {
                this._trajectory.points.Add(new Trajectory.Point(new Vector2(0f, 0f), Quaternion.identity));
            }
        }

        void FixedUpdate()
        {
            if (!_inCutscene)
                StartCoroutine(UpdateCoroutine());
        }

        /// <summary>
        /// When true, the Salamander uses the Animator, instead of MoMa
        /// </summary>
        public void InCutscene(bool inCutscene)
        {
            _inCutscene = inCutscene;
        }

        public void AddTarget(MovementController.EventType type, Vector3 position)
        {
            //Debug.Log("Adding target: " + position + " Type: " + type);
            _mc.AddTarget(type, position);
        }

        private IEnumerator UpdateCoroutine()
        {
            // Update MovementComponent
            _mc.Update();

            // Add Point to Trajectory, removing the oldest point
            if (_currentFrame % FramesPerPoint == 0)
            {
                this._trajectory.points.Add(
                    new Trajectory.Point(
                        new Vector2(this._model.position.x, this._model.position.z),
                        this._model.rotation
                        )
                    );
                this._trajectory.points.RemoveAt(0);

                // Reset current Frame
                _currentFrame = 0;
            }

            _currentFrame++;

            // Load new Animation.Clip
            if (_ac.IsOver())
            {
                // Find and load next Animation.Clip
                Trajectory.Snippet snippet = GetCurrentSnippet();
                _ac.LoadClip(this._rc.QueryClip(snippet));
            }

            // Play Animation.Frame
            _ac.Step();

            yield return null;
        }

        private Trajectory.Snippet GetCurrentSnippet()
        {
            Trajectory.Snippet snippet;
            int futureFramesNumber = FramesPerPoint * FeaturePoints;

            // Get simulated future
            List<(Vector3, Quaternion)> futureTransforms = this._mc.GetFuture(futureFramesNumber);

            // Convert the (many) Frames to (few) Point and add them to the Trajectory
            for (int i = 0; i < FeaturePoints; i++)
            {
                //Trajectory.Point point = Trajectory.Point.getMedianPoint(futureFrames.GetRange(i * Trajectory.FramesPerPoint, Trajectory.FramesPerPoint));
                //Trajectory.Point point = new Trajectory.Point(futureFrames[i * Feature.FramesPerPoint + Feature.FramesPerPoint / 2].GetXZVector2());
                Trajectory.Point point = new Trajectory.Point(
                    futureTransforms[(i + 1) * FramesPerPoint - 1].Item1.GetXZVector2(),
                    futureTransforms[(i + 1) * FramesPerPoint - 1].Item2
                    );
                this._trajectory.points.Add(point);
            }

            // Compute the Trajectory Snippet
            snippet = this._trajectory.GetLocalSnippet(FeaturePastPoints - 1);

            // Remove future Points from Trajectory
            this._trajectory.points.RemoveRange(FeaturePastPoints, FeaturePoints);

            return snippet;
        }
    }
}
