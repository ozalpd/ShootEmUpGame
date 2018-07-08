using UnityEngine;

public abstract class AbstractPlayerController : MonoBehaviour, IControllable
{
    public abstract bool Firing { get; set; }
    public abstract bool Shielding { get; set; }
    public abstract void Move(Vector2 movement);
    /// <summary>
    /// Sets properties and variables to default values.
    /// </summary>
    public abstract void SetToDefaults();
    public abstract void SwitchWeapon(Weapon weapon);
    /// <summary>
    /// Resets the player including transformations.
    /// </summary>
    public virtual void ResetPlayer()
    {
        SetToDefaults();
    }
}