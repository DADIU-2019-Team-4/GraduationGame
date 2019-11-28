using System;
using System.Collections.Generic;
using UnityEngine;

namespace MoMa
{
    public class Packer
    {
        // The number of frames for each root transform sample
        // ++ Lower jiggling, -- root moves like hips
        public const int SampleWindow = 20;

        // Threshold of hips distance to determine idle or moving
        public const float DisplacementThreshold = SampleWindow * 0.01f;

        public static Animation Pack(string animationName, string directory, string filename)
        {
            // Initialize an empty Animation
            Animation anim = new Animation(animationName);

            // Load the raw Animation data from the specified file
            Packer.LoadRawAnimationFromFile(anim, directory, filename);

            // Compute the local position, rotation and velocity of every Bone in every Frame
            ComputeLocalTranform(anim);

            // Compute the feature Frames
            anim.ComputeFeatures();

            return anim;
        }

        private static void LoadRawAnimationFromFile(Animation anim, string directory, string filename)
        {
            Debug.Log("Loading Motion from file \"" + filename + "\"");

            // Open given file and split the lines
            TextAsset moCapAsset = Resources.Load<TextAsset>(directory + "/" + filename);

            if (moCapAsset == null)
            {
                Debug.LogError("Unable to load MoCap file: " + filename);
                return;
            }

            string[] data = moCapAsset.text.Split(
                new[] { "\r\n", "\r", "\n" },
                StringSplitOptions.None
            );

            // Read Bone names (Frame 0)
            var bonesLine = data[0];
            var bones = bonesLine.Split(',');

            // Add all frames to the current Animation (excl. first line (Titles) and last line (empty))
            for (var currentFrame = 1; currentFrame < data.Length - 1; currentFrame += SalamanderController.SkipFrames)
            {
                // Pass the current timestamp to the Frame constructor
                var dataFrame = data[currentFrame].Split(',');
                Frame newFrame = new Frame(float.Parse(dataFrame[0]));

                // Every bone has data in 7 columns (3 position, 4 rotation)
                foreach (Bone.Type bt in Enum.GetValues(typeof(Bone.Type)))
                {
                    int off = (int)bt * 7;
                    Bone.Data currentBoneData = new Bone.Data(
                        new Vector3(
                            float.Parse(dataFrame[off + 1]),    // Start from 1 because of the Timestamp on 0
                            float.Parse(dataFrame[off + 2]),
                            float.Parse(dataFrame[off + 3])
                            ),
                        new Quaternion(
                            float.Parse(dataFrame[off + 4]),
                            float.Parse(dataFrame[off + 5]),
                            float.Parse(dataFrame[off + 6]),
                            float.Parse(dataFrame[off + 7])
                            )
                    );

                    // Create the new Frame
                    newFrame.boneDataDict.Add(bt, currentBoneData);
                }

                // Add the new Frame to the current Motion
                anim.frameList.Add(newFrame);
            }

            // Root motion is not included in the file, so we approximate it
            ComputeRootTransform(anim);

            Debug.Log("Motion contains " + anim.frameList.Count + " frames");

            // Unload Asset to free memory
            Resources.UnloadAsset(moCapAsset);
        }

        private static void ComputeRootTransform(Animation anim)
        {
            List<Bone.Data> rootSamples = new List<Bone.Data>();
            Quaternion lastRotation = Quaternion.LookRotation(Vector3.forward, Vector3.up);

            // 1. Sample the root transform every SampleWindow frames
            for (
                int currentFrame = 0;
                currentFrame + SampleWindow < anim.frameList.Count;
                currentFrame += SampleWindow)
            {
                Bone.Data sample = SampleFromFrame(anim, currentFrame, lastRotation);
                lastRotation = sample.rotation;
                rootSamples.Add(sample);
            }

            // 2. Use sample to determine intermediate values
            // Left padding
            for (
                int currentFrame = 0;
                currentFrame < SampleWindow / 2;
                currentFrame++)
            {
                // Load current Frame's root data
                Bone.Data currentRootData = anim.frameList[currentFrame].boneDataDict[Bone.Type.root];

                // Lerp to find a smooth transform
                (Vector3 position, Quaternion rotation) = LerpFrame(
                    anim.frameList[0].boneDataDict[Bone.Type.hips],
                    rootSamples[0],
                    (float)currentFrame / (SampleWindow / 2));

                /// Store found value
                currentRootData.position = position;
                currentRootData.rotation = rotation;
            }

            // Intermediate frames
            for (
                int currentSample = 0;
                currentSample < rootSamples.Count - 1;
                currentSample++)
            {
                for (
                    int currentFrame = 0;
                    currentFrame < SampleWindow;
                    currentFrame++)
                {
                    // Load current Frame's root data
                    Bone.Data currentRootData = anim.frameList[
                        currentSample * SampleWindow + currentFrame + SampleWindow / 2
                        ].boneDataDict[Bone.Type.root];

                    // Lerp to find a smooth transform
                    (Vector3 position, Quaternion rotation) = LerpFrame(
                        rootSamples[currentSample],
                        rootSamples[currentSample + 1],
                        currentFrame / SampleWindow);

                    /// Store found value
                    currentRootData.position = position;
                    currentRootData.rotation = rotation;
                }
            }

            // Right padding
            int rightPaddingFrames = SampleWindow / 2 + anim.frameList.Count % SampleWindow;
            int firstPaddingFrame = anim.frameList.Count - rightPaddingFrames;
            for (
                int currentFrameOffset = 0;
                currentFrameOffset < rightPaddingFrames;
                currentFrameOffset++)
            {
                // Load current Frame's root data
                Bone.Data currentRootData = anim.frameList[
                    firstPaddingFrame + currentFrameOffset
                    ].boneDataDict[Bone.Type.root];

                // Lerp to find a smooth transform
                (Vector3 position, Quaternion rotation) = LerpFrame(
                    rootSamples[rootSamples.Count - 1],
                    anim.frameList[anim.frameList.Count - 1].boneDataDict[Bone.Type.hips],
                    currentFrameOffset / rightPaddingFrames);

                /// Store found value
                currentRootData.position = position;
                currentRootData.rotation = rotation;
            }

            // 3. Recalibrate hips to comply with root's rotation
            for (
                int currentFrame = 0;
                currentFrame < anim.frameList.Count;
                currentFrame++)
            {
                anim.frameList[currentFrame].boneDataDict[Bone.Type.hips].position -=
                    anim.frameList[currentFrame].boneDataDict[Bone.Type.root].position;
                anim.frameList[currentFrame].boneDataDict[Bone.Type.hips].rotation *=
                    Quaternion.Inverse(anim.frameList[currentFrame].boneDataDict[Bone.Type.root].rotation);
            }
        }

        private static void ComputeLocalTranform(Animation anim)
        {
            // Validate input
            if (anim.frameList.Count < 2)
            {
                Debug.LogError("The Animation does not have enough Frames to compute velocities");
                throw new Exception("The Animation does not have enough Frames to compute velocities");
            }

            // Find local Positions
            foreach (Frame frame in anim.frameList)
            {
                // Root's (hips) Position and Rotation
                Vector3 rootP = frame.boneDataDict[Bone.Type.root].position;
                Quaternion rootQ = frame.boneDataDict[Bone.Type.root].rotation;

                // For every bone of the Animation
                foreach (Bone.Type bt in Enum.GetValues(typeof(Bone.Type)))
                {
                    // Local position considers root as 0 and root's up as up
                    frame.boneDataDict[bt].SetLocalPosition(rootP, rootQ);
                }
            }

            // Find local Velocity
            foreach (Bone.Type bt in Enum.GetValues(typeof(Bone.Type)))
            {
                // Find the position of the first frame
                Vector3 lastLocalPosition = anim.frameList[0].boneDataDict[bt].localPosition;

                // Compute the velocities of all the Frames but the first
                for (int i = 1; i < anim.frameList.Count; i++)
                {
                    anim.frameList[i].boneDataDict[bt].SetLocalVelocity(lastLocalPosition);
                    lastLocalPosition = anim.frameList[i].boneDataDict[bt].localPosition;
                }

                // Set the velocity of the first Frame equal to the one in the second
                lastLocalPosition = anim.frameList[1].boneDataDict[bt].localPosition;
                anim.frameList[0].boneDataDict[bt].SetLocalVelocity(lastLocalPosition);
            }
        }

        private static Bone.Data SampleFromFrame(Animation anim, int startingSampleFrame, Quaternion lastRotation)
        {
            Quaternion rotation;
            Vector3 position = new Vector3(0, 0, 0);
            Vector3 displacement;
            Vector3 firstPosition;
            Vector3 lastPosition;

            // Average position: Average of eacch position in the window
            for (int i = 0; i < SampleWindow; i++)
            {
                position += anim.frameList[startingSampleFrame + i].boneDataDict[Bone.Type.hips].position;
            }
            position /= SampleWindow;

            // Average rotation: difference of the last position to the first
            firstPosition = anim.frameList[startingSampleFrame].boneDataDict[Bone.Type.hips].position;
            lastPosition = anim.frameList[startingSampleFrame + SampleWindow - 1].boneDataDict[Bone.Type.hips].position;
            displacement = (lastPosition - firstPosition);

            //Debug.Log("Displacement: " + displacement.magnitude);

            if (displacement.magnitude > DisplacementThreshold)
            {
                rotation = lastRotation;
            }
            else
            {
                displacement.y = 0;
                rotation = Quaternion.LookRotation(displacement, Vector3.up);
            }

            return new Bone.Data(position, rotation);
        }

        private static (Vector3, Quaternion) LerpFrame(Bone.Data a, Bone.Data b, float t)
        {
            return (Vector3.Lerp(a.position, b.position, t), Quaternion.Lerp(a.rotation, b.rotation, t));
        }
    }
}
