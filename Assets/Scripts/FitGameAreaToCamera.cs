using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Fits game area to main camera.
/// </summary>
[RequireComponent(typeof(GameArea))]
[AddComponentMenu("OzzGames/Fit GameArea to Camera")]
public class FitGameAreaToCamera : MonoBehaviour
{
    private GameArea Area
    {
        get
        {
            return GetComponent<GameArea>();
        }
    }

    private void Awake()
    {
        FitToCamera();
    }

    private void Reset()
    {
        FitToCamera();
    }


    private void FitToCamera(Camera cam)
    {
        Area.Size = new Vector2(cam.aspect * cam.orthographicSize * 2, cam.orthographicSize * 2);

        //Vector3 camPos = cam.transform.position;
        //camPos.z = 0; //this a 2D project, so everything is at z=0 except the camera
        //transform.position = camPos;

        //below line is much better than above three
        transform.position = (Vector2)cam.transform.position;

        transform.rotation = cam.transform.rotation;
    }

    private void FitToCamera()
    {
        FitToCamera(Camera.main);
    }
}
