using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeSlowdown : MonoBehaviour
{

    private float slowdownFactor = 0.5f;
    private float slowdownLength = 2f;

    private GameController gameController;

    void Awake()
    {
        gameController = FindObjectOfType<GameController>();
    }
 

    // Update is called once per frame
    void Update()
    {
        if (gameController == null)
            gameController = FindObjectOfType<GameController>();
        if (gameController == null || !gameController.IsPlaying) return;

        Time.timeScale += (1f / slowdownLength) * Time.unscaledDeltaTime;
        Time.timeScale = Mathf.Clamp(Time.timeScale, 0f, 1f);
    }

    public void doSlowmotion()
    {
        Time.timeScale = slowdownFactor;
        Time.fixedDeltaTime = Time.timeScale * .02f; 
    }

}
