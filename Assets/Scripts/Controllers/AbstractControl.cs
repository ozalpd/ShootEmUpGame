using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractControl : MonoBehaviour
{
    protected IControllable controllable;
    public AbstractShipControllable shipControllable;

    private void Awake()
    {
        controllable = shipControllable;
    }
}