using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace MoMa
{
    public class Pose
    {
        // Position of limbs relative to the root
        public IDictionary<Bone.Type, Limb> limbDataDict = new Dictionary<Bone.Type, Limb>();

        public Pose(Frame frame)
        {
            // TODO: Shall I re-include hands?
            foreach (Bone.Type bone in new Bone.Type[] { Bone.Type.head, Bone.Type.leftFoot, Bone.Type.rightFoot, Bone.Type.leftHand, Bone.Type.rightHand })
            //foreach (Bone.Type bone in new Bone.Type[] { Bone.Type.head, Bone.Type.leftFoot, Bone.Type.rightFoot} )
            {
                limbDataDict.Add(bone, new Limb(frame.boneDataDict[bone].localPosition, frame.boneDataDict[bone].localVelocity));
            }
        }

        public (float, float) CalcDiff(Pose candidate)
        {
            float diff1 = 0f;
            float diff2 = 0f;

            foreach (Bone.Type bone in limbDataDict.Keys)
            {
                Vector3 expectedLocalPosition = this.limbDataDict[bone].localPosition + this.limbDataDict[bone].localVelocity;
                Vector3 candidateLocalPosition = candidate.limbDataDict[bone].localPosition;
                diff1 += (expectedLocalPosition - candidateLocalPosition).magnitude;
                diff2 += (this.limbDataDict[bone].localVelocity - candidate.limbDataDict[bone].localVelocity).magnitude;
            }

            return (diff1, diff2);
        }

        public class Limb
        {
            public Vector3 localPosition;
            public Vector3 localVelocity;

            public Limb(Vector3 localPosition, Vector3 localVelocity)
            {
                this.localPosition = localPosition;
                this.localVelocity = localVelocity;
            }
        }
    }
}
