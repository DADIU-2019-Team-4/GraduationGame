using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace MoMa
{
    [System.Serializable]
    public class Frame : ISerializationCallbackReceiver
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

        #region Serialization methods

        public List<Bone.Type> _keys = new List<Bone.Type>();
        public List<Bone.Data> _values = new List<Bone.Data>();

        public void OnBeforeSerialize()
        {
            _keys.Clear();
            _values.Clear();

            foreach (var kvp in boneDataDict)
            {
                _keys.Add(kvp.Key);
                _values.Add(kvp.Value);
            }
        }

        public void OnAfterDeserialize()
        {
            boneDataDict = new Dictionary<Bone.Type, Bone.Data>();

            for (var i = 0; i != Math.Min(_keys.Count, _values.Count); i++)
                boneDataDict.Add(_keys[i], _values[i]);

            _keys.Clear();
            _values.Clear();
        }

        #endregion
    }
}