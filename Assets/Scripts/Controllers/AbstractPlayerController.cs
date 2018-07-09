using UnityEngine;

public abstract class AbstractPlayerController : MonoBehaviour, IControllable
{
    public abstract bool Firing { get; set; }
    public abstract bool Shielding { get; set; }
    public abstract void Move(Vector2 movement);
    public abstract void SwitchWeapon(Weapon weapon);
    /// <summary>
    /// Resets properties, variables and transformations.
    /// </summary>
    public virtual void ResetPlayer()
    {
        ResetValues();
    }
    /// <summary>
    /// Sets properties and variables to default values.
    /// </summary>
    public abstract void ResetValues();
}