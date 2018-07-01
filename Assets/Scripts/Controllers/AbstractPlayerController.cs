using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractPlayerController : MonoBehaviour, IControllable
{
    public abstract void Move(Vector2 movement);
}