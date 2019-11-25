using UnityEngine;
using System.Collections;

public class PathFinder : MonoBehaviour
{
    private PathKeeper _pathKeeper;

    // Use this for initialization
    void Awake()
    {
        _pathKeeper = GameObject.FindWithTag("Player").GetComponent<PathKeeper>();

        if (!_pathKeeper) throw new System.Exception("PathFinder was unable to find PathKeeper or the Player");
    }

    // Update is called once per frame
    void Update2()
    {
        Debug.Log(_pathKeeper.GetPath().Length);
    }
}
