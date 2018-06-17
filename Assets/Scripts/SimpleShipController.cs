using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleShipController : MonoBehaviour
{
    [Range(0.25f,25)]
    public float thrustMultiplier = 2;
    [Range(0.05f, 2)]
    public float steerMultiplier = 0.5f;

    //moved _delta here to make GC less busy
    Vector2 _delta = Vector2.zero;
    Vector2 _force = Vector2.zero;
    float _torque;

    Rigidbody2D _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();   
    }

    void Update()
    {
#if (UNITY_IOS || UNITY_ANDROID) && (REMOTE || !UNITY_EDITOR)
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            speed = 0.25f;
            if (touch.phase == TouchPhase.Moved)
            {
                delta = touch.deltaPosition;
            }
        }
#else
        _delta.x = Input.GetAxis("Horizontal");
        _delta.y = Input.GetAxis("Vertical");
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
