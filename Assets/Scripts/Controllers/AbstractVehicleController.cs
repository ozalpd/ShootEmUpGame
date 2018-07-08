using System.Linq;
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
    protected int _shieldHashId;
    protected int _steeringHashId;
    protected int _thrustXHashId;
    protected int _thrustYHashId;

    protected List<Weapon> _weapons;

    protected virtual void Awake()
    {
        _animator = GetComponent<Animator>();
        if (_animator != null)
        {
            _thrustXHashId = Animator.StringToHash("ThrustX");
            _thrustYHashId = Animator.StringToHash("ThrustY");
            _steeringHashId = Animator.StringToHash("Steering");

            _shieldHashId = _animator.GetLayerIndex("Shield"); //If no layer Shield, throws an error
        }

        _weapons = new List<Weapon>();
        if (Weapon == null)
        {
            Debug.LogWarning("A Weapon instance could not be found!");
        }
    }

    public override bool Firing
    {
        set
        {
            Weapon.Firing = value && !Shielding;
        }
        get { return Weapon.Firing; }
    }

    public override bool Shielding
    {
        set
        {
            if (_isProtecting != value)
            {
                _isProtecting = value;
                _animator.SetLayerWeight(_shieldHashId, _isProtecting ? 1f : 0f);
            }

            if (_isProtecting)
                Firing = false;
        }
        get { return _isProtecting; }
    }
    bool _isProtecting;

    public float Steering
    {
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
        get { return _steering; }
    }
    float _steering;

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

    protected Weapon Weapon
    {
        get
        {
            if (_weapon == null)
                _weapon = GetComponentInChildren<Weapon>();

            return _weapon;
        }
        set
        {
            _weapon = value;
            if (!_weapons.Contains(_weapon))
                _weapons.Add(_weapon);
        }
    }
    Weapon _weapon;

    public override void ResetPlayer()
    {
        Steering = 0;
        Thrust = Vector2.zero;
        Firing = Shielding = false;

        transform.rotation = Quaternion.identity;
        transform.position = Vector3.zero;

        Weapon = _weapons.ElementAt(0);
        gameObject.SendMessage("Repair");
    }

    public override void SwitchWeapon(Weapon weapon)
    {
        if (Weapon.name.Equals(weapon.name))
            return;

        var inListWeapon = _weapons.FirstOrDefault(w => w.name.Equals(weapon.name));

        bool isFiring = Firing;
        Firing = false; //first, we need to stop current one
        Weapon.gameObject.SetActive(false);

        if (inListWeapon != null)
        {
            inListWeapon.gameObject.SetActive(true);
            Weapon = inListWeapon;
        }
        else
        {
            var weaponGO = Instantiate(weapon.gameObject, transform);
            weaponGO.transform.localRotation = Quaternion.identity;
            weaponGO.transform.localPosition = Vector3.zero;

            Weapon = weaponGO.GetComponent<Weapon>();
        }

        Weapon.Firing = isFiring;
    }

    public override void Move(Vector2 movement)
    {
        movement = Vector2.ClampMagnitude(movement, 1.0f);
        movement.y = Mathf.Clamp(movement.y, -rearThrustLimit, 1);

        Steering = steeringCurve.Evaluate(movement.y) * movement.x;
        Thrust = new Vector2(glideCurve.Evaluate(movement.y) * movement.x, movement.y);

        enabled = movement != Vector2.zero; //saving a small cpu usage
    }
}
