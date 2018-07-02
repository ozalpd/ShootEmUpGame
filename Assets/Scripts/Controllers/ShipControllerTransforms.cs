using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipControllerTransforms : AbstractVehicleController
{
    public float steeringMultiplier = 5;
    public Vector2 thrustMultiplier = new Vector2(0.25f, 0.25f);

    Vector3 correctedThrust;

    void Update()
    {
        correctedThrust = Thrust;
        correctedThrust.Scale(thrustMultiplier);

        transform.Translate(correctedThrust, Space.Self);
        transform.Rotate(0, 0, -Steering * steeringMultiplier, Space.Self);
    }
}
