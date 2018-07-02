using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchControl : AbstractUserControl
{
    bool _released;
    Vector2 lastTouchDelta = Vector2.zero;

#if (UNITY_IOS || UNITY_ANDROID) && !UNITY_EDITOR
    protected override void Awake()
    {
        base.Awake();
        enabled = true;
    }
#endif

    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touchZero = Input.GetTouch(0);
            switch (touchZero.phase)
            {
                case TouchPhase.Began:
                    _released = false;
                    break;

                case TouchPhase.Moved:
                    lastTouchDelta = touchZero.deltaPosition;
                    controllable.Move(lastTouchDelta);
                    break;

                case TouchPhase.Ended:
                    //controllable.Move(Vector2.zero);
                    _released = true;
                    break;

                default:
                    break;
            }

            controllable.Firing = (Input.touchCount > 1);
        }

        if (_released && lastTouchDelta.magnitude > 0)
        {
            lastTouchDelta = Vector2.Lerp(lastTouchDelta, Vector2.zero, Time.deltaTime * 5.0f);

            if (lastTouchDelta.magnitude < 0.05f)
                lastTouchDelta = Vector2.zero;

            controllable.Move(lastTouchDelta);
        }
    }
}
