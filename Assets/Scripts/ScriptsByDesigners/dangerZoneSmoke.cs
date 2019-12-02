using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dangerZoneSmoke : MonoBehaviour
{

    SmokeOnGirl smokeOnGirl; 


    // Start is called before the first frame update
    void Start()
    {
        smokeOnGirl = GameObject.FindGameObjectWithTag("Player").GetComponent<SmokeOnGirl>(); 
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other != null)
        {
            if (other.CompareTag("Player"))
            {
                smokeOnGirl.startSmoke();
            }
        }
      
            
    }

    private void OnTriggerExit(Collider other)
    {
        if (other != null)
        {
            if (other.CompareTag("Player"))
            {
                smokeOnGirl.stopSmoke();
            }
        }
       
    }
}
