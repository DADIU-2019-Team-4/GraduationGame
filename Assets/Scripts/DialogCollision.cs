using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class DialogCollision : MonoBehaviour
{
    private Canvas canvas;
    private float dialogActivetime;

    private void Start()
    {
        canvas = gameObject.GetComponentInChildren<Canvas>();
        canvas.enabled = false;
    }

    private void Update()
    {
        if (canvas.enabled && dialogActivetime < 3.0f)
            dialogActivetime = dialogActivetime + Time.deltaTime;
        if (dialogActivetime >= 3.0f)
            DisableDialog();
    }
    public void EnableDialog()
    { 
        canvas.enabled = true;
        dialogActivetime = 0.0f;
    }
    public void DisableDialog()
    {
            canvas.enabled = false;
    }

}
