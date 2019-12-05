using UnityEngine;
using System.Collections.Generic;

namespace MoMa
{
    [System.Serializable]
    public class Animation : ScriptableObject
    {
        public string animationName;

        // Each feature refers to a set of frames
        public List<Frame> frameList = new List<Frame>();
        public List<Feature> featureList = new List<Feature>();

        public void ComputeFeatures()
        {
            // 1. Find fitted trajectory for the whole animation
            Trajectory trajectory = ComputeFittedTrajectory();

            // 2. Compute Features
            for (
                int currentPoint = SalamanderController.FeaturePastPoints-1;   // Left padding for past Points
                currentPoint < trajectory.points.Count - SalamanderController.FeaturePoints - SalamanderController.ClipBlendPoints;   // Right padding for future Points
                currentPoint += SalamanderController.FeatureEveryPoints
                )
            {
                // Find the first Frame of the current Point(s)
                int frameNum = currentPoint * SalamanderController.FramesPerPoint;

                // Built new Feature
                this.featureList.Add( new Feature(
                    frameNum,

                    // Compute the Trajectory Snippet relative to the current Frame
                    trajectory.GetLocalSnippet(currentPoint),

                    // Compute the Pose according to the current Frame
                    new Pose(this.frameList[frameNum])
                    )
                );
            }
        }

        private Trajectory ComputeFittedTrajectory()
        {
            Trajectory fittedTrajectory = new Trajectory();

            for (
                int frameNum = SalamanderController.FramesPerPoint / 2;
                frameNum < this.frameList.Count - SalamanderController.FramesPerPoint;
                frameNum += SalamanderController.FramesPerPoint
                )
            {
                // Find the median Point of all the frames in the current sample
                //Trajectory.Point point = Trajectory.Point.getMedianPoint(
                //    this.frameList.GetRange(frameNum - SalamanderController.FramesPerPoint / 2, SalamanderController.FramesPerPoint).ConvertAll(
                //        f => (f.boneDataDict[Bone.Type.root].position.GetXZVector2(), f.boneDataDict[Bone.Type.root].rotation.eulerAngles)
                //        )
                //    );
                // (TODO FINAL): set it to use root and not hips
                Trajectory.Point point = Trajectory.Point.getMedianPoint(
                    this.frameList.GetRange(frameNum - SalamanderController.FramesPerPoint / 2, SalamanderController.FramesPerPoint).ConvertAll(
                        f => (f.boneDataDict[Bone.Type.hips].position.GetXZVector2S())
                        )
                    );

                fittedTrajectory.points.Add(point);
            }

            return fittedTrajectory;
        }

        public class Clip
        {
            private const int BlendFrames = SalamanderController.ClipBlendPoints * SalamanderController.FramesPerPoint;
            private Frame[] _frames;
            private int _currentFrame = 0;

            public Clip(List<Frame> frameList)
            {
                this._frames = new Frame[frameList.Count];

                for (int i=0; i < frameList.Count; i++)
                {
                    this._frames[i] = frameList[i];
                }
            }

            public Frame Step()
            {
                // Return the current Frame and increase the Frame counter or null
                return _currentFrame < this._frames.Length ?
                    this._frames[this._currentFrame++] :
                    null;
            }

            public Clip BlendWith(Clip clip)
            {
                if (clip != null && this._frames != null)
                {
                    for (int i = 0; i < BlendFrames && i < this._frames.Length; i++)
                    {
                        this._frames[i].BlendWith(
                            clip._frames[clip._frames.Length - BlendFrames - 1 + i],
                            (float)i / BlendFrames
                            );
                    }
                }

                return this;
            }

            public bool isOver()
            {
                return (this._currentFrame + BlendFrames >= this._frames.Length);
            }
        }
    }
}
