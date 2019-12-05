using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceBoxReferences : MonoBehaviour
{
    public void Start()
    {
        var g = FindObjectOfType<GameController>();
        g.NullifyBoxCollection();
        g.GetAllBoxReferencesInLevel();
    }
}
