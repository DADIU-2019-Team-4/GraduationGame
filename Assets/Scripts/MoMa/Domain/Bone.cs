using UnityEngine;
using System.Collections;

public class Bone
{
    // This is the Rokoko Newton format (not Mixamo)
    public enum Type
    {
        root,
        hips,
        leftThigh,
        leftShin,
        leftFoot,
        leftToe,
        leftToeTip,
        rightThigh,
        rightShin,
        rightFoot,
        rightToe,
        rightToeTip,
        spine1,
        spine2,
        spine3,
        spine4,
        leftShoulder,
        leftArm,
        leftForeArm,
        leftHand,
        neck,
        head,
        rightShoulder,
        rightArm,
        rightForeArm,
        rightHand
    }

    [System.Serializable]
    public class Data
    {
        public Vector3 position;
        public Quaternion rotation;
        public Vector3 localPosition;
        public Vector3 localVelocity;

        public Data(Vector3 position, Quaternion rotation)
        {
            this.position = position;
            this.rotation = rotation;
        }

        public void SetLocalPosition(Vector3 originPosition, Quaternion originRotation)
        {
            this.localPosition = originRotation * (this.position - originPosition);
        }

        // The next position refers to the next Feature Frame NOT actual Frame
        public void SetLocalVelocity(Vector3 nextLocalPosition)
        {
            this.localVelocity = nextLocalPosition - this.localPosition;
        }

        public Data BlendWith(Data data, float weight)
        {
            this.position = this.position  * (1-weight) + data.position * weight;

            // TODO: Not sure if weight here is used correcltly
            this.rotation = Quaternion.Slerp(data.rotation, this.rotation, weight);

            // Check head's rotation (in Euler)
            //Debug.Log()

            this.localPosition = this.localPosition * (1 - weight) + data.localPosition * weight;
            this.localVelocity = this.localVelocity * (1 - weight) + data.localVelocity * weight;

            return this;
        }
    }
}
