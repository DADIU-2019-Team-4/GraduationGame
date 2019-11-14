using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace MoMa
{
    [System.Serializable]
    public class Frame
    {
        public float timestamp;
        public IDictionary<Bone.Type, Bone.Data> boneDataDict;

        public Frame(float timestamp)
        {
            this.timestamp = timestamp;
            this.boneDataDict = new Dictionary<Bone.Type, Bone.Data>();
        }

        public Frame BlendWith(Frame frame, float weight)
        {
            foreach (Bone.Type bone in this.boneDataDict.Keys)
            {
                this.boneDataDict[bone].BlendWith(frame.boneDataDict[bone], weight);
            }

            return this;
        }
    }
}