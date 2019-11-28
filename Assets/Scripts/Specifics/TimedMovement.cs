using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TimedMovement : IGameLoop
{
    public float moveTime = 10f;
    private float initialTime;
    public bool inSafeZone = true;
    public TMP_Text timeText;
    public float addToTimer = 2;

    // Start is called before the first frame update
    void Start()
    {
        initialTime = moveTime;
    }

    // Update is called once per frame
    public override void GameLoopUpdate()
    {
        if (!inSafeZone)
        {
            moveTime -= Time.deltaTime;
            timeText.text = Mathf.Round(moveTime) + "";
            if (moveTime <= 0) this.GetComponent<MovementController>().CheckFireLeft();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("safeZone"))
        {
            print("in");
            inSafeZone = true;
            moveTime = initialTime;
            timeText.text = moveTime + "";
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("safeZone"))
            inSafeZone = false;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("PickUp"))
        {
            moveTime += addToTimer;
        }
    }
}
