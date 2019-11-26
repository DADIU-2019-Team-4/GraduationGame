﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameAttachToggler : MonoBehaviour
{

    private MovementController _movementController;
    private AttachToPlane _attachToPlane;
    [Header("Things to toggle")]
    public GameObject Flame;
    public GameObject Player;
    public GameObject Trail;

    // Start is called before the first frame update
    void Start()
    {
        _movementController = GetComponent<MovementController>();
        _attachToPlane = GetComponent<AttachToPlane>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_movementController.IsFuseMoving)
        {

        }
    }


    public void FlameOn()
    {
        Player.SetActive(false);
        Flame.SetActive(true);
        Trail.SetActive(false);
    }

    public void FlameOff()
    {
        Player.SetActive(true);

        Flame.SetActive(false);
        Trail.SetActive(true);
    }
}
