using UnityEngine;
using System.Collections;

[System.Serializable]
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
        public Vector3S position;
        public QuaternionS rotation;
        public Vector3S localPosition;
        public Vector3S localVelocity;

        public Data(Vector3S position, QuaternionS rotation)
        {
            this.position = position;
            this.rotation = rotation;
        }

        public void SetLocalPosition(Vector3S originPosition, QuaternionS originRotation)
        {
            this.localPosition =  originRotation * (Vector3) (this.position - originPosition);
        }

        // The next position refers to the next Feature Frame NOT actual Frame
        public void SetLocalVelocity(Vector3S nextLocalPosition)
        {
            this.localVelocity = nextLocalPosition - this.localPosition;
        }

        public Data BlendWith(Data targetData, float weight)
        {
            this.position = Vector3.Slerp((Vector3) this.position , (Vector3) targetData.position, weight);

            // TODO: Not sure if weight here is used correcltly
            //this.rotation = Quaternion.Slerp(this.rotation, targetData.rotation, weight);
            //this.rotation = Quaternion.Euler(Vector3.Lerp(this.rotation.eulerAngles, targetData.rotation.eulerAngles, weight));

            this.localPosition = Vector3.Slerp((Vector3) localPosition, (Vector3) targetData.localPosition, weight);
            this.localVelocity = Vector3.Slerp((Vector3) localVelocity, (Vector3) targetData.localPosition, weight);
            //this.localPosition = this.localPosition * (1 - weight) + targetData.localPosition * weight;
            //this.localVelocity = this.localVelocity * (1 - weight) + targetData.localVelocity * weight;

            return this;
        }
    }
}
