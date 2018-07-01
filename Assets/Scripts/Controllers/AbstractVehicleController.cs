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

    protected Animator _animator;
    protected int _steeringHashId;
    protected int _thrustXHashId;
    protected int _thrustYHashId;

    protected virtual void Awake()
    {
        _animator = GetComponent<Animator>();
        if (_animator != null)
        {
            _thrustXHashId = Animator.StringToHash("ThrustX");
            _thrustYHashId = Animator.StringToHash("ThrustY");
            _steeringHashId = Animator.StringToHash("Steering");
            //Debug.Log("Steering ID:" + _steeringHashId);
            //Debug.Log("ThrustX ID:" + _thrustXHashId);
            //Debug.Log("ThrustY ID:" + _thrustYHashId);
        }
    }

    public Vector2 Thrust
    {
        get { return _thrust; }
        set
        {
            if (value != _thrust)
            {
                _thrust = value;

                if (_animator != null)
                {
                    _animator.SetFloat(_thrustXHashId, _thrust.x);
                    _animator.SetFloat(_thrustYHashId, _thrust.y);
                }
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

                if (_animator != null)
                {
                    _animator.SetFloat(_steeringHashId, _steering);
                }
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
