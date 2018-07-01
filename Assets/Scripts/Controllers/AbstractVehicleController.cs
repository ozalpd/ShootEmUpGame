using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Abstract vehicle controller.
/// </summary>
public class AbstractVehicleController : AbstractPlayerController
{
    public AnimationCurve steeringCurve;
    public AnimationCurve glideCurve;

    public float rearThrustLimit = 0.2f;

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
            if (!Mathf.Approximately(value, _steering))
            {
                _steering = value;
            }
        }
    }
    float _steering;

    public override void Move(Vector2 movement)
    {
        movement = Vector2.ClampMagnitude(movement, 1.0f);
        movement.y = Mathf.Clamp(movement.y, -rearThrustLimit, 1);

        Steering = steeringCurve.Evaluate(movement.y) * movement.x;
        Thrust = new Vector2(glideCurve.Evaluate(movement.y) * movement.x, movement.y);

        enabled = movement != Vector2.zero; //saving a small cpu usage
    }
}
