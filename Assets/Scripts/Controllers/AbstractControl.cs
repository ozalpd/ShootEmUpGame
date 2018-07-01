using UnityEngine;

/// <summary>
/// Abstract control class for input control, touch control, etc.
/// </summary>
public abstract class AbstractControl : MonoBehaviour
{
    public IControllable controllable;
    public AbstractPlayerController playerController;

    private void Awake()
    {
        controllable = playerController;
    }
}