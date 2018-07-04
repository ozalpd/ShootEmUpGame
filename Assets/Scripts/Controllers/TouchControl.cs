using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchControl : AbstractUserControl
{
    [Range(1, 200)]
    public int touchSensitivity = 50;

    bool _released;
    Vector2 _touchBeginning;
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
                    _touchBeginning = touchZero.position;
                    break;

                case TouchPhase.Moved:
                    lastTouchDelta = (touchZero.position - _touchBeginning) * (1f / (float)touchSensitivity);
                    controllable.Move(lastTouchDelta);
                    break;

                case TouchPhase.Ended:
                    //controllable.Move(Vector2.zero);
                    _released = true;
                    break;

                default:
                    break;
            }

            Debug.DrawLine(Camera.main.ScreenToWorldPoint(_touchBeginning), Camera.main.ScreenToWorldPoint(touchZero.position));
            //Debug.Log("_touchBeginning: " + _touchBeginning);
            //Debug.Log("touchZero.position: " + touchZero.position);
            //Debug.Log("lastTouchDelta: " + lastTouchDelta);
        }

        controllable.Firing = (Input.touchCount > 1);

        if (_released && lastTouchDelta.magnitude > 0)
        {
            lastTouchDelta = Vector2.Lerp(lastTouchDelta, Vector2.zero, Time.deltaTime * 5.0f);

            if (lastTouchDelta.magnitude < 0.05f)
                lastTouchDelta = Vector2.zero;

            controllable.Move(lastTouchDelta);
        }
    }
}
