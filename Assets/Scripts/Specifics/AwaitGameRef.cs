using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AwaitGameRef : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        Game game = FindObjectOfType<Game>();
        if (game == null) return; // Await additive scene loading.

        // Adds the reference to looping components.
        IGameLoop[] gameLoops = GetComponents<IGameLoop>();
        foreach (IGameLoop gameLoop in gameLoops)
            gameLoop.OnEnable();

        // Clean up and start game.
        Destroy(this);
    }
}
