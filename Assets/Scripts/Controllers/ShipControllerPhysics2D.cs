using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class ShipControllerPhysics2D : AbstractVehicleController
{
    public float steeringMultiplier = 10;
    public Vector2 thrustMultiplier = new Vector2(10f, 10f);

    Rigidbody2D _rigidbody;
    Vector3 correctedThrust = new Vector3();

    protected override void Awake()
    {
        base.Awake();
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    protected void FixedUpdate()
    {
        _rigidbody.AddTorque(-Steering * steeringMultiplier, ForceMode2D.Force);

        correctedThrust = (100 * Time.fixedDeltaTime) * (Vector3)Thrust;
        //correctedThrust = (Vector3)Thrust;
        correctedThrust.Scale(thrustMultiplier);
        _rigidbody.AddRelativeForce(correctedThrust, ForceMode2D.Force);
    }
}
