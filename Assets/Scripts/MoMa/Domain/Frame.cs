using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace MoMa
{
    [System.Serializable]
    public class Frame
    {
        private List<Bone.Type> blendableBones = new List<Bone.Type>()
        {
            Bone.Type.root,
            Bone.Type.hips,
            Bone.Type.leftThigh,
            Bone.Type.leftShin,
            Bone.Type.leftFoot,
            Bone.Type.leftToe,
            Bone.Type.leftToeTip,
            Bone.Type.rightThigh,
            Bone.Type.rightShin,
            Bone.Type.rightFoot,
            Bone.Type.rightToe,
            Bone.Type.rightToeTip,
            Bone.Type.spine1,
            Bone.Type.spine2,
            Bone.Type.spine3,
            Bone.Type.spine4,
            Bone.Type.leftShoulder,
            Bone.Type.leftArm,
            Bone.Type.leftForeArm,
            Bone.Type.leftHand,
            Bone.Type.neck,
            Bone.Type.head,
            Bone.Type.rightShoulder,
            Bone.Type.rightArm,
            Bone.Type.rightForeArm,
            Bone.Type.rightHand
        };

        public float timestamp;
        public IDictionary<Bone.Type, Bone.Data> boneDataDict;

        public Frame(float timestamp)
        {
            this.timestamp = timestamp;
            this.boneDataDict = new Dictionary<Bone.Type, Bone.Data>();
        }

        public Frame BlendWith(Frame frame, float weight)
        {
            foreach (Bone.Type bone in blendableBones)
            {
                this.boneDataDict[bone].BlendWith(frame.boneDataDict[bone], weight);
            }

            return this;
        }
    }
}