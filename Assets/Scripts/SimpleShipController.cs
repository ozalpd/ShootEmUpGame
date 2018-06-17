using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleShipController : MonoBehaviour
{
    //Transform transform;

    private void Awake()
    {
        //transform = GetComponent<Transform>();   
    }

    void Start()
    {

    }

    void Update()
    {
        float x = 0f;
        float y = 0f;
        float speed = 0.5f;
#if (UNITY_IOS || UNITY_ANDROID) && (REMOTE || !UNITY_EDITOR)
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            speed = 0.25f;
            if (touch.phase == TouchPhase.Moved)
            {
                x = touch.deltaPosition.x * speed;
                y = touch.deltaPosition.y * speed;
            }
        }
#else
        x = Input.GetAxis("Horizontal") * speed;
        y = Input.GetAxis("Vertical") * speed;
#endif
        transform.Translate(x, y, 0f);
    }
}
