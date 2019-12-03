using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace MoMa
{
    [System.Serializable]
    public class Pose : ISerializationCallbackReceiver
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
                Vector3S expectedLocalPosition = limbDataDict[bone].localPosition + limbDataDict[bone].localVelocity;
                Vector3S candidateLocalPosition = candidate.limbDataDict[bone].localPosition;
                diff1 += (expectedLocalPosition - candidateLocalPosition).magnitude;
                diff2 += (this.limbDataDict[bone].localVelocity - candidate.limbDataDict[bone].localVelocity).magnitude;
            }

            return (diff1, diff2);
        }

        [System.Serializable]
        public class Limb
        {
            public Vector3S localPosition;
            public Vector3S localVelocity;

            public Limb(Vector3S localPosition, Vector3S localVelocity)
            {
                this.localPosition = localPosition;
                this.localVelocity = localVelocity;
            }
        }

        #region Serialization methods

        public List<Bone.Type> _keys = new List<Bone.Type>();
        public List<Limb> _values = new List<Limb>();

        public void OnBeforeSerialize()
        {
            _keys.Clear();
            _values.Clear();

            foreach (var kvp in limbDataDict)
            {
                _keys.Add(kvp.Key);
                _values.Add(kvp.Value);
            }
        }

        public void OnAfterDeserialize()
        {
            limbDataDict = new Dictionary<Bone.Type, Limb>();

            for (var i = 0; i != Math.Min(_keys.Count, _values.Count); i++)
                limbDataDict.Add(_keys[i], _values[i]);

            _keys.Clear();
            _values.Clear();
        }

        #endregion
    }
}
