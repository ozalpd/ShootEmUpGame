using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbstractShipControllable : MonoBehaviour, IControllable
{
    public Vector2 Thrust
    {
        get { return _thrust; }
        set
        {
            if (value != _thrust)
            {
                _thrust = value;
            }
        }
    }
    Vector2 _thrust;

    public float Steering
    {
        get { return _steering; }
        set
        {
            if (value != _steering)
            {
                _steering = value;
            }
        }
    }
    float _steering;

    public void Move(Vector2 movement)
    {
        Steering = movement.x;
        Thrust = movement;
    }
}
