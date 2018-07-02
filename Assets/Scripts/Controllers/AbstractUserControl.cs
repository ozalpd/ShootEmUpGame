using UnityEngine;

/// <summary>
/// Abstract control class for input control, touch control, etc.
/// </summary>
public abstract class AbstractUserControl : MonoBehaviour
{
    public IControllable controllable;
    public AbstractPlayerController playerController;

    protected virtual void Awake()
    {
        controllable = playerController;
    }
}