using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;

namespace MoMa
{
    public class Packer
    {
        // The number of frames for each root transform sample
        // ++ Lower jiggling, -- root moves like hips
        public const int SampleWindow = 10;

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
            for (var currentFrame = 1; currentFrame < data.Length - 1; currentFrame += CharacterController.SkipFrames)
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

            // 1. Sample the root transform every SampleWindow frames
            for (
                int currentFrame = 0;
                currentFrame + SampleWindow < anim.frameList.Count;
                currentFrame += SampleWindow)
            {
                rootSamples.Add(SampleFromFrame(anim, currentFrame));
            }

            // 2. Use sample to determine intermediate values
            // Left padding
            for (
                int currentFrame = 0;
                currentFrame < SampleWindow / 2;
                currentFrame++)
            {
                (Vector3 position, Vector3 eulerRotation) = LerpFrame(
                    anim.frameList[0].boneDataDict[Bone.Type.hips],
                    rootSamples[0],
                    (float) currentFrame / (SampleWindow / 2));

                anim.frameList[currentFrame].boneDataDict[Bone.Type.root].position = position;
                anim.frameList[currentFrame].boneDataDict[Bone.Type.root].rotation =
                    Quaternion.Euler(eulerRotation);
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
                    (Vector3 position, Vector3 eulerRotation) = LerpFrame(
                        rootSamples[currentSample],
                        rootSamples[currentSample + 1],
                        currentFrame / SampleWindow);

                    anim.frameList[currentSample * SampleWindow + currentFrame].boneDataDict[Bone.Type.root].position =
                        position;
                    anim.frameList[currentSample * SampleWindow + currentFrame].boneDataDict[Bone.Type.root].rotation =
                        Quaternion.Euler(eulerRotation);
                }
            }

            // Right padding
            int rightPaddingFrames = SampleWindow / 2 + anim.frameList.Count % SampleWindow;
            for (
                int currentFrame = anim.frameList.Count - rightPaddingFrames;
                currentFrame < rightPaddingFrames;
                currentFrame++)
            {
                (Vector3 position, Vector3 eulerRotation) = LerpFrame(
                    rootSamples[rootSamples.Count - 1],
                    anim.frameList[anim.frameList.Count - 1].boneDataDict[Bone.Type.hips],
                    (currentFrame - anim.frameList.Count - rightPaddingFrames) / rightPaddingFrames);

                anim.frameList[currentFrame].boneDataDict[Bone.Type.root].position = position;
                anim.frameList[currentFrame].boneDataDict[Bone.Type.root].rotation =
                    Quaternion.Euler(eulerRotation);
            }

            // 3. Recalibrate hips to comply with root's rotation
            //for (
            //    int currentFrame = 0;
            //    currentFrame < anim.frameList.Count;
            //    currentFrame++)
            //{
            //    anim.frameList[currentFrame].boneDataDict[Bone.Type.hips].rotation =
            //        anim.frameList[currentFrame].boneDataDict[Bone.Type.hips].rotation *
            //        Quaternion.Inverse(anim.frameList[currentFrame].boneDataDict[Bone.Type.root].rotation);
            //}
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
                for (int i=1; i < anim.frameList.Count; i++)
                {
                    anim.frameList[i].boneDataDict[bt].SetLocalVelocity(lastLocalPosition);
                    lastLocalPosition = anim.frameList[i].boneDataDict[bt].localPosition;
                }

                // Set the velocity of the first Frame equal to the one in the second
                lastLocalPosition = anim.frameList[1].boneDataDict[bt].localPosition;
                anim.frameList[0].boneDataDict[bt].SetLocalVelocity(lastLocalPosition);
            }
        }

        private static Bone.Data SampleFromFrame(Animation anim, int startingSampleFrame)
        {
            Vector3 position = new Vector3(0, 0, 0);
            Vector3 eulerRotation = new Vector3(0, 0, 0);

            // In each window average position and 2D rotation
            for (int i = 0; i < SampleWindow; i++)
            {
                Bone.Data currentTransform = anim.frameList[startingSampleFrame + i].boneDataDict[Bone.Type.hips];
                position += currentTransform.position;
                eulerRotation += currentTransform.rotation.eulerAngles;
            }

            // Create the new root sample
            position /= SampleWindow;
            position.y = 0;
            eulerRotation.y /= SampleWindow;
            eulerRotation.x = 0;
            eulerRotation.z = 0;
            return new Bone.Data(position, Quaternion.Euler(eulerRotation));
        }

        private static (Vector3, Vector3) LerpFrame(Bone.Data a, Bone.Data b, float t)
        {
            Vector3 position;
            Vector3 eulerRotation;

            // Set position
            position = Vector3.Lerp(a.position, b.position, t);

            // Set rotation
            eulerRotation = Vector3.Lerp(a.rotation.eulerAngles, b.rotation.eulerAngles, t);
            eulerRotation.x = 0;
            eulerRotation.z = 0;

            return (position, eulerRotation);
        }
    }
}
