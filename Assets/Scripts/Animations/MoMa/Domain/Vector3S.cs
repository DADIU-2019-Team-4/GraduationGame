using UnityEngine;

[System.Serializable]
public class Vector3S
{
    public float x = 0f;
    public float y = 0f;
    public float z = 0f;

    public float magnitude
    {
        get
        {
            return new Vector3(x, y, z).magnitude;
        }
    }

    public Vector3S()
    {
        this.x = 0f;
        this.y = 0f;
        this.z = 0f;
    }

    public Vector3S(float x, float y, float z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }

    public Vector3S(Vector3 v)
    {
        this.x = v.x;
        this.y = v.y;
        this.z = v.z;
    }

    public Vector2S GetXZVector2S() => new Vector2S(x, z);

    public static explicit operator Vector3(Vector3S vs) => new Vector3(vs.x, vs.y, vs.z);
    public static implicit operator Vector3S(Vector3 v) => new Vector3S(v);

    public static Vector3S operator -(Vector3S a, Vector3S b) =>
        new Vector3S(a.x - b.x, a.y - b.y, a.z - b.z );

    public static Vector3S operator +(Vector3S a, Vector3S b) =>
        new Vector3S(a.x + b.x, a.y + b.y, a.z + b.z);

    public static Vector3S operator /(Vector3S a, int b) =>
        new Vector3S(a.x/b, a.y/b, a.z/b);
}
