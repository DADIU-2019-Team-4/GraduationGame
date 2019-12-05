using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace MoMa
{
    public class RuntimeComponent
    {
        private List<Animation> _anim = new List<Animation>();
        private List<Feature> _onCooldown = new List<Feature>();
        private int _currentAnimation = 0;
        private int _currentFeature = 0;
        private Animation.Clip _currentClip;
        private FollowerComponent _fc;
        private float _maxTrajectoryDiff;

        public RuntimeComponent(FollowerComponent fc, List<string> files, bool recalculateFlag = true)
        {
            Debug.Log("MoMa: " + (recalculateFlag ? " Recalculate animations using Packer" : " Using pre-calculated animations"));

            foreach (string filename in files)
            {
                Animation loadedAnimation = recalculateFlag ?
                    // Recalculate animations using Packer and .csv files in Resource
                    Packer.Pack(filename, filename) :
                    // Don't recalculate animations. Load the existing assets
                    LoadPackedAnimationFile(filename);

                this._anim.Add(loadedAnimation);

                Debug.Log("Loaded MoMa file \"" + filename + "\" with " + loadedAnimation.frameList.Count + " frames");
            }

            // This exists for dubugging. Maybe it needs to be removed.
            #if UNITY_EDITOR
            this._fc = fc;
            #endif
        }

        public Animation.Clip QueryClip(Trajectory.Snippet currentSnippet)
        {
            // 1. Reduce cooldowns (after the previous Clip has finished)
            ReduceCooldowns();

            // 2. Check if the next Clip is fitting (or the first one, if we reach the end)
            // The next Clip is NOT necesserily the product of the next Feature
            this._currentFeature = (this._currentFeature + SalamanderController.FeatureStep) % this._anim[this._currentAnimation].featureList.Count;

            this._maxTrajectoryDiff = currentSnippet.CalcDiff(this._anim[this._currentAnimation].featureList[this._currentFeature].snippet);

            if (this._maxTrajectoryDiff > SalamanderController.RecalculationThreshold)
            {
                (this._currentAnimation, this._currentFeature) = QueryFeature(currentSnippet);

                //Debug.Log("Recalculating");
                //Debug.Log("File: " + this._currentAnimation + " Clip: " +  this._currentFeature);
            }
            else
            {
                //Debug.Log("Not Recalculating");
            }

            // 3. Construct the Clip, blend it with the current one and return it
            Animation.Clip nextClip = new Animation.Clip(
                this._anim[this._currentAnimation].frameList.GetRange(
                    this._anim[this._currentAnimation].featureList[this._currentFeature].frameNum + SalamanderController.FramesPerPoint,
                    SalamanderController.FramesPerPoint * (SalamanderController.FeaturePoints + SalamanderController.ClipBlendPoints)
                    )
                );

            nextClip.BlendWith(_currentClip);
            _currentClip = nextClip;

            // 4. Put the current Feature on cooldown
            PutOnCooldown(this._anim[this._currentAnimation].featureList[this._currentFeature]);

            return nextClip;
        }

        private (int, int) QueryFeature(Trajectory.Snippet currentSnippet)
        {
            List<CandidateFeature> candidateFeatures = new List<CandidateFeature>();
            Tuple<float, CandidateFeature> winnerFeature = new Tuple<float, CandidateFeature>(Mathf.Infinity, null);
            Pose currentPose = _anim[_currentAnimation].featureList[_currentFeature].pose;
            float maxPosePositionDiff = 0;
            float maxPoseVelocityDiff = 0;

            // Draw current snippet
            _fc?.DrawPath(currentSnippet);

            // Initialize candidate features to the next clip
            for (int i = 0; i < SalamanderController.CandidateFramesSize; i++)
            {
                candidateFeatures.Add(new CandidateFeature(
                    _anim[_currentAnimation].featureList[_currentFeature],
                    _maxTrajectoryDiff,
                    _currentAnimation,
                    _currentFeature)
                    );
            }

            // 1. Find the Clips with the most fitting Trajectories
            for (int i = 0; i < _anim.Count; i++)
            {
                for (int j = 0; j < _anim[i].featureList.Count; j++)
                {
                    Feature feature = _anim[i].featureList[j];

                    // Consider only active Frames (not on cooldown)
                    if (feature.cooldownTimer == 0)
                    {
                        // A. Add candidate Feature to the best candidates list
                        float diff = currentSnippet.CalcDiff(feature.snippet);

                        // Experiment
                        // If the Trajectory is good enough to consider
                        if (diff <= _maxTrajectoryDiff)
                        {
                            // Remove worst candidate
                            candidateFeatures.RemoveAt(candidateFeatures.Count - 1);
                            _maxTrajectoryDiff = candidateFeatures[candidateFeatures.Count - 1].trajectoryDiff;


                            // Add candidate to the right position
                            for (int k = 0; k < candidateFeatures.Count; k++)
                            {
                                if (candidateFeatures[k].trajectoryDiff > diff)
                                {
                                    candidateFeatures.Insert(k, new CandidateFeature(feature, diff, i, j));
                                    break;
                                }
                            }
                        }




                        /*
                        if (diff <= this._maxTrajectoryDiff)
                        {
                            CandidateFeature candidateFeature = new CandidateFeature(
                                feature, diff, i, j
                                );
                            candidateFeatures.Add(candidateFeature);
                        }

                        // B. Sort candidates based on their diff
                        candidateFeatures.Sort(
                            (firstObj, secondObj) =>
                            {
                                return firstObj.trajectoryDiff.CompareTo(secondObj.trajectoryDiff);
                            }
                        );
                        */
                    }
                }
            }

            //// C. Keep only a predefined number of best candidates
            //if (candidateFeatures.Count <= 0)
            //{
            //    Debug.LogError("Unable to find any Animation Frame to transition to");
            //    return (0, 0);
            //}
            //else if (candidateFeatures.Count > SalamanderController.CandidateFramesSize)
            //{
            //    candidateFeatures.RemoveRange(SalamanderController.CandidateFramesSize, candidateFeatures.Count - SalamanderController.CandidateFramesSize);
            //}

            // 2. Compute the difference in Pose for each Clip (position and velocity)
            for (int i = 0; i < candidateFeatures.Count; i++)
            {
                (float posePositionDiff, float poseVelocityDiff) = currentPose.CalcDiff(candidateFeatures[i].feature.pose);
                candidateFeatures[i].posePositionDiff = posePositionDiff;
                candidateFeatures[i].poseVelocityDiff = poseVelocityDiff;

                // Keep the maximum values of the differences, in order to normalise
                maxPosePositionDiff = posePositionDiff > maxPosePositionDiff ?
                     posePositionDiff :
                     maxPosePositionDiff;

                maxPoseVelocityDiff = poseVelocityDiff > maxPoseVelocityDiff ?
                    poseVelocityDiff :
                    maxPoseVelocityDiff;

                // Draw all alternative paths
                //this._fc.DrawAlternativePath(candidateFeatures[i].feature.snippet, i, candidateFeatures[i].trajectoryDiff);
            }

            // 3. Normalize and add differences
            for (int i = 0; i < candidateFeatures.Count; i++)
            {
                if (maxPosePositionDiff > 0) candidateFeatures[i].posePositionDiff /= maxPosePositionDiff;
                if (maxPoseVelocityDiff > 0) candidateFeatures[i].poseVelocityDiff /= maxPoseVelocityDiff;

                float totalPostDiff = candidateFeatures[i].posePositionDiff + candidateFeatures[i].poseVelocityDiff;

                winnerFeature = winnerFeature.Item1 > totalPostDiff ?
                    new Tuple<float, CandidateFeature>(totalPostDiff, candidateFeatures[i]) :
                    winnerFeature;
            }

            //Debug.Log("Playing animation from \"" + this._anim[winnerFeature.Item2.animationNum].animationName + "\"");

            // Draw picked animation's path
            this._fc?.DrawAlternativePath(winnerFeature.Item2.feature.snippet, 1, winnerFeature.Item2.trajectoryDiff);

            // 4. Return the Feature's index
            return (winnerFeature.Item2.animationNum, winnerFeature.Item2.clipNum);
        }

        private void PutOnCooldown(Feature feature)
        {
            _onCooldown.Add(feature);
            feature.cooldownTimer = SalamanderController.CooldownTime;
        }

        private void ReduceCooldowns()
        {
            // Traverse the List in reverse order to remove elements at the same time
            for (int i = _onCooldown.Count - 1; i >= 0; i--)
            {
                Feature feature = _onCooldown[i];

                // Reduce the timer by the amount af frames passed since last reduction
                feature.cooldownTimer = (feature.cooldownTimer > 0) ?
                    feature.cooldownTimer - 1 :
                    0;

                // When the counter reaches 0, remove the feature from the _onCooldown list
                if (feature.cooldownTimer == 0)
                {
                    _onCooldown.Remove(feature);
                }
            }
        }

        private Animation LoadPackedAnimationFile(string filename)
        {
            Animation anim = null;

            anim = (Animation) AssetDatabase.LoadAssetAtPath(
                Packer.AssetPath + "/" + filename + Packer.AssetExtention, typeof(Animation)
            );

            if (anim == null)
            {
                Debug.LogError("Unable to open MoMa Animation asset: " + Packer.AssetPath + "/" + filename + Packer.AssetExtention);
            }

            return anim;
        }

        private class CandidateFeature
        {
            public Feature feature;
            public float trajectoryDiff;
            public float posePositionDiff;
            public float poseVelocityDiff;
            public int animationNum;
            public int clipNum;

            public CandidateFeature(Feature feature, float trajectoryDiff, int animationNum, int clipNum)
            {
                this.feature = feature;
                this.trajectoryDiff = trajectoryDiff;
                this.animationNum = animationNum;
                this.clipNum = clipNum;
                this.posePositionDiff = Mathf.Infinity;
                this.poseVelocityDiff = Mathf.Infinity;
            }
        }
    }
}
