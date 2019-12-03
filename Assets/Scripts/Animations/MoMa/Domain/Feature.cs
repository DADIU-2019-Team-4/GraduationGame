using UnityEngine;
using System.Collections;

namespace MoMa
{
    [System.Serializable]
    public class Feature
    {
        public int frameNum;
        public Trajectory.Snippet snippet;
        public Pose pose;

        public int cooldownTimer = 0;

        public Feature(int frameNum, Trajectory.Snippet snippet, Pose pose)
        {
            this.frameNum = frameNum;
            this.snippet = snippet;
            this.pose = pose;
        }
    }
}