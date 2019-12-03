using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetCameraReference : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var cam = GetComponent<CinemachineVirtualCamera>();
        var player = GameObject.FindGameObjectWithTag("Player");
        cam.Follow = player.transform;
        cam.LookAt = player.transform;
    }


}
