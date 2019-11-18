using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraFindPlayer : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        // Await MainPlayerScene
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null) return;

        // Set values.
        var camera = GameObject.FindGameObjectWithTag("VirtualCamera").GetComponent<CinemachineVirtualCamera>();
        camera.Follow = camera.LookAt = player.transform;

        // Disable this script.
        Destroy(this);
    }
}
