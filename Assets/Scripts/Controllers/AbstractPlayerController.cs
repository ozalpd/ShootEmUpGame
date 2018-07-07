﻿using UnityEngine;

public abstract class AbstractPlayerController : MonoBehaviour, IControllable
{
    public abstract bool Firing { get; set; }
    public abstract bool Shielding { get; set; }
    public abstract void Move(Vector2 movement);
    public abstract void SwitchWeapon(Weapon weapon);
}