﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputControl : AbstractControl
{
    Vector2 _movement;


    void Update()
    {
        _movement.x = Input.GetAxis("Horizontal");
        _movement.y = Input.GetAxis("Vertical");

        controllable.Move(_movement);
    }
}