using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

namespace MoMa
{
    public class AnimationComponent
    {
        private const string PlayerTag = "Player"; // The Tag that the Player's GameObject has in the game

        private IDictionary<Bone.Type, Transform> _bones;
        private Transform _model;
        private Animation.Clip _clip;

        public AnimationComponent(Transform model)
        {
            this._model = model;
            AttachToTransforms(this._model);
        }

        public bool IsOver()
        {
            return this._clip != null ?
                this._clip.isOver() :
                true;
        }

        public void Step()
        {
            // Play the next Frame of the Clip
            Frame frame = this._clip.Step();

            foreach (Bone.Type bone in this._bones.Keys)
            {
                // This changes the rig to the one used by Rokoko
                //this._bones[bone].SetPositionAndRotation(frame.boneDataDict[bone].position, frame.boneDataDict[bone].rotation);
                //this._bones[bone].SetPositionAndRotation(frame.boneDataDict[bone].localPosition, frame.boneDataDict[bone].rotation);

                // This keeps the rig's proportions
                this._bones[bone].rotation = frame.boneDataDict[bone].rotation;
            }

            this._model.localRotation = Quaternion.Inverse(frame.boneDataDict[Bone.Type.hips].rotation);
        }

        public void LoadClip(Animation.Clip clip)
        {
            this._clip = clip;
        }

        #region Rig-attaching methods
        private void AttachToTransforms(Transform model)
        {
            // Create an empty Dictionary
            this._bones = new Dictionary<Bone.Type, Transform>();

            #region Load all _bones
            // Load core
            this._bones.Add(Bone.Type.root, model);
            this._bones.Add(Bone.Type.hips, this._bones[Bone.Type.root].GetChild(0));

            // Load left foot
            this._bones.Add(Bone.Type.leftThigh, this._bones[Bone.Type.hips].GetChild(0));
            this._bones.Add(Bone.Type.leftShin, this._bones[Bone.Type.leftThigh].GetChild(0));
            this._bones.Add(Bone.Type.leftFoot, this._bones[Bone.Type.leftShin].GetChild(0));
            this._bones.Add(Bone.Type.leftToe, this._bones[Bone.Type.leftFoot].GetChild(0));
            this._bones.Add(Bone.Type.leftToeTip, this._bones[Bone.Type.leftToe].GetChild(0));

            // Load right foot
            this._bones.Add(Bone.Type.rightThigh, this._bones[Bone.Type.hips].GetChild(1));
            this._bones.Add(Bone.Type.rightShin, this._bones[Bone.Type.rightThigh].GetChild(0));
            this._bones.Add(Bone.Type.rightFoot, this._bones[Bone.Type.rightShin].GetChild(0));
            this._bones.Add(Bone.Type.rightToe, this._bones[Bone.Type.rightFoot].GetChild(0));
            this._bones.Add(Bone.Type.rightToeTip, this._bones[Bone.Type.rightToe].GetChild(0));

            // Load spine
            this._bones.Add(Bone.Type.spine1, this._bones[Bone.Type.hips].GetChild(2));
            this._bones.Add(Bone.Type.spine2, this._bones[Bone.Type.spine1].GetChild(0));
            this._bones.Add(Bone.Type.spine3, this._bones[Bone.Type.spine2].GetChild(0));
            this._bones.Add(Bone.Type.spine4, this._bones[Bone.Type.spine3].GetChild(0));

            // Load head
            this._bones.Add(Bone.Type.neck, this._bones[Bone.Type.spine4].GetChild(1));
            this._bones.Add(Bone.Type.head, this._bones[Bone.Type.neck].GetChild(0));

            // Load left arm
            this._bones.Add(Bone.Type.leftShoulder, this._bones[Bone.Type.spine4].GetChild(0));
            this._bones.Add(Bone.Type.leftArm, this._bones[Bone.Type.leftShoulder].GetChild(0));
            this._bones.Add(Bone.Type.leftForeArm, this._bones[Bone.Type.leftArm].GetChild(0));
            this._bones.Add(Bone.Type.leftHand, this._bones[Bone.Type.leftForeArm].GetChild(0));

            // Load right arm
            this._bones.Add(Bone.Type.rightShoulder, this._bones[Bone.Type.spine4].GetChild(2));
            this._bones.Add(Bone.Type.rightArm, this._bones[Bone.Type.rightShoulder].GetChild(0));
            this._bones.Add(Bone.Type.rightForeArm, this._bones[Bone.Type.rightArm].GetChild(0));
            this._bones.Add(Bone.Type.rightHand, this._bones[Bone.Type.rightForeArm].GetChild(0));

            #endregion
        }

        private void AttachToNewtonTransforms(Transform model)
        {
            // Create an empty Dictionary
            this._bones = new Dictionary<Bone.Type, Transform>();

            #region Load all _bones
            // Load core
            this._bones.Add(Bone.Type.root, model);
            this._bones.Add(Bone.Type.hips, this._bones[Bone.Type.root].GetChild(0));

            // Load left foot
            this._bones.Add(Bone.Type.leftThigh, this._bones[Bone.Type.hips].GetChild(0));
            this._bones.Add(Bone.Type.leftShin, this._bones[Bone.Type.leftThigh].GetChild(0));
            this._bones.Add(Bone.Type.leftFoot, this._bones[Bone.Type.leftShin].GetChild(0));
            this._bones.Add(Bone.Type.leftToe, this._bones[Bone.Type.leftFoot].GetChild(0));
            this._bones.Add(Bone.Type.leftToeTip, this._bones[Bone.Type.leftToe].GetChild(0));

            // Load right foot
            this._bones.Add(Bone.Type.rightThigh, this._bones[Bone.Type.hips].GetChild(1));
            this._bones.Add(Bone.Type.rightShin, this._bones[Bone.Type.rightThigh].GetChild(0));
            this._bones.Add(Bone.Type.rightFoot, this._bones[Bone.Type.rightShin].GetChild(0));
            this._bones.Add(Bone.Type.rightToe, this._bones[Bone.Type.rightFoot].GetChild(0));
            this._bones.Add(Bone.Type.rightToeTip, this._bones[Bone.Type.rightToe].GetChild(0));

            // Load spine
            this._bones.Add(Bone.Type.spine1, this._bones[Bone.Type.hips].GetChild(2));
            this._bones.Add(Bone.Type.spine2, this._bones[Bone.Type.spine1].GetChild(0));
            this._bones.Add(Bone.Type.spine3, this._bones[Bone.Type.spine2].GetChild(0));
            this._bones.Add(Bone.Type.spine4, this._bones[Bone.Type.spine3].GetChild(0));

            // Load head
            this._bones.Add(Bone.Type.neck, this._bones[Bone.Type.spine4].GetChild(1));
            this._bones.Add(Bone.Type.head, this._bones[Bone.Type.neck].GetChild(0));

            // Load left arm
            this._bones.Add(Bone.Type.leftShoulder, this._bones[Bone.Type.spine4].GetChild(0));
            this._bones.Add(Bone.Type.leftArm, this._bones[Bone.Type.leftShoulder].GetChild(0));
            this._bones.Add(Bone.Type.leftForeArm, this._bones[Bone.Type.leftArm].GetChild(0));
            this._bones.Add(Bone.Type.leftHand, this._bones[Bone.Type.leftForeArm].GetChild(0));

            // Load right arm
            this._bones.Add(Bone.Type.rightShoulder, this._bones[Bone.Type.spine4].GetChild(2));
            this._bones.Add(Bone.Type.rightArm, this._bones[Bone.Type.rightShoulder].GetChild(0));
            this._bones.Add(Bone.Type.rightForeArm, this._bones[Bone.Type.rightArm].GetChild(0));
            this._bones.Add(Bone.Type.rightHand, this._bones[Bone.Type.rightForeArm].GetChild(0));

            #endregion
        }
        #endregion
    }
}