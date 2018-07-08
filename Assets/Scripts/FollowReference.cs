using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Follows the reference transform
/// </summary>
public class FollowReference : MonoBehaviour
{
    public Transform reference;

    public float speed = 1f;
    [Tooltip("Angular rotation speed. If this value is zero, has no effect")]
    public float rotationSpeed;

    void Update()
    {
        if (rotationSpeed > 0)
            transform.rotation = Quaternion.RotateTowards(transform.rotation, reference.rotation, rotationSpeed * Time.deltaTime);

        transform.position = Vector3.MoveTowards(transform.position, reference.position, speed * Time.deltaTime);
    }
}
