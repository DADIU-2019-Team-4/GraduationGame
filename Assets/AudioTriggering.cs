using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioTriggering : MonoBehaviour
{
    public GameObject door;

    private void OnTriggerEnter(Collider other)
    {
        if (!door.activeSelf)
            if (other.CompareTag("Player"))
            {
                AkSoundEngine.SetState("roomOneOne", "Room0101_outro");
                Destroy(gameObject);
            }
    }

}
