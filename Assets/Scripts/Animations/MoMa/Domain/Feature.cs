using UnityEngine;
using System.Collections;

namespace MoMa
{
    public class Feature
    {
        public readonly int frameNum;
        public readonly Trajectory.Snippet snippet;
        public readonly Pose pose;

        public int cooldownTimer = 0;

        public Feature(int frameNum, Trajectory.Snippet snippet, Pose pose)
        {
            this.frameNum = frameNum;
            this.snippet = snippet;
            this.pose = pose;
        }
    }
}