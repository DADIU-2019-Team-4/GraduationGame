using UnityEngine;
using System.Collections;

[System.Serializable]
public class QuaternionS
{
    private float _x;
    private float _y;
    private float _z;
    private float _w;

    public Vector3S eulerAngles
    {
        get
        {
            return new Quaternion(_x, _y, _z, _w).eulerAngles;
        }
    }

    public QuaternionS (float x, float y, float z, float w)
    {
        _x = x;
        _y = y;
        _z = z;
        _w = w;
    }

    public QuaternionS(Quaternion q)
    {
        _x = q.x;
        _y = q.y;
        _z = q.z;
        _w = q.w;
    }

    public static explicit operator Quaternion(QuaternionS qs) => new Quaternion(qs._x, qs._y, qs._z, qs._w);
    public static implicit operator QuaternionS(Quaternion q) => new QuaternionS(q);

    public static Vector3S operator *(QuaternionS q, Vector3S v) =>
        new Vector3S((Quaternion) q * (Vector3) v);

    public static QuaternionS operator *(QuaternionS q1, QuaternionS q2) =>
        new QuaternionS((Quaternion) q1 * (Quaternion) q2);
}
