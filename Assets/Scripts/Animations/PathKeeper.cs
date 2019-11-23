using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Stores the path of the Object attached to
/// </summary>
public class PathKeeper : MonoBehaviour
{
    private List<Vector2> _positions = new List<Vector2>();

    /// <summary>
    /// Logs a position
    /// </summary>
    public void LogPosition(Vector2 position)
    {
        _positions.Add(position);
    }

    //  TODO: If the MovementController of the Player is recreated on every level, remove this. Otherwise, use this before the level starts
    /// <summary>
    /// Resets the current path and starts a new one
    /// </summary>
    public void ResetPath()
    {
        _positions = new List<Vector2>();
    }

    /// <summary>
    /// Returns the current path
    /// </summary>
    public Vector2[] GetPath()
    {
        return _positions.ToArray();
    }
}
