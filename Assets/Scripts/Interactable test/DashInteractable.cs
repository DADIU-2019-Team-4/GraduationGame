using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class DashInteractable : MonoBehaviour
{
    [Header("Interactable")]
    public int test1;
    public int test2;
    public UnityEvent OnInteract;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public abstract void Interact(GameObject player);
    
}
