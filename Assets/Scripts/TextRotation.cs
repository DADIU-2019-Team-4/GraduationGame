using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextRotation : MonoBehaviour
{
    Transform cameraMain;
    private void OnEnable()
    {
        cameraMain = GameObject.Find("Main Camera").transform; 
    }
    public  void Update()
    {
        gameObject.transform.rotation = cameraMain.transform.rotation;
    }
}
