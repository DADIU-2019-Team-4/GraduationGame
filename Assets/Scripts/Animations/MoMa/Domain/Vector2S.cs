using UnityEngine;
using System.Collections;

public class Vector2S
{
    public float x = 0f;
    public float y = 0f;

    public float magnitude {
        get {
            return new Vector2(x, y).magnitude;
        }
    }

    public Vector2S()
    {
        this.x = 0f;
        this.y = 0f;
    }

    public Vector2S(float x, float y)
    {
        this.x = x;
        this.y = y;
    }
    public Vector2S(Vector2 v)
    {
        this.x = v.x;
        this.y = v.y;
    }

    public static explicit operator Vector2(Vector2S vs) => new Vector2(vs.x, vs.y);
    public static implicit operator Vector2S(Vector2 v) => new Vector2S(v);

    public static Vector2S operator -(Vector2S a, Vector2S b) =>
        new Vector2S(a.x - b.x, a.y - b.y);

    public static Vector2S operator +(Vector2S a, Vector2S b) =>
        new Vector2S(a.x + b.x, a.y + b.y);

    public static Vector2S operator /(Vector2S a, int b) =>
        new Vector2S(a.x / b, a.y / b);
}
