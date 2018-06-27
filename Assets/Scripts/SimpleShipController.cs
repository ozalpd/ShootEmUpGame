using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleShipController : MonoBehaviour
{
    [Range(0.25f, 25)]
    public float thrustMultiplier = 2;
    [Range(0.05f, 2)]
    public float steerMultiplier = 0.5f;

    Vector2 _delta = Vector2.zero;
    Vector2 _force = Vector2.zero;
    float _torque;

    Rigidbody2D _rigidbody;
    Weapon _weapon;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _weapon = GetComponentInChildren<Weapon>();
    }

    bool Firing
    {
        set
        {
            if (_isFiring != value)
            {
                _isFiring = value;
                float repeatRate = 1 / _weapon.firingRate;

                if (_isFiring)
                    _weapon.InvokeRepeating("fire", 0.001f, repeatRate);
                else
                    _weapon.CancelInvoke("fire");
            }
        }
        get { return _isFiring; }
    }
    bool _isFiring;

    void Update()
    {
#if (UNITY_IOS || UNITY_ANDROID) && (REMOTE || !UNITY_EDITOR)
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            float speed = 0.5f;
            if (touch.phase == TouchPhase.Moved)
            {
                _delta = touch.deltaPosition * speed;
            }

            Firing = (Input.touchCount > 1);
        }
#else
        _delta.x = Input.GetAxis("Horizontal");
        _delta.y = Input.GetAxis("Vertical");
        Firing = Input.GetButton("Fire2");
#endif
        //transform.Translate(delta.x, delta.y, 0f);
        //driving ship with above control become impossible
        //after adding rigidbody to ship and asteroids 

        _force.y = _delta.y * thrustMultiplier;
        _torque = -_delta.x * steerMultiplier;

        _rigidbody.AddRelativeForce(_force);
        _rigidbody.AddTorque(_torque);
    }
}
