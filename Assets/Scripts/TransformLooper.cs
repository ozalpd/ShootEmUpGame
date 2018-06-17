using UnityEngine;

/// <summary>
/// Purpose is looping objects transforms accross to a rectangle.
/// When the object leaves the area on the right, it come backs on the left.
/// </summary>
[AddComponentMenu("OzzGames/Transform Looper")]
public class TransformLooper : MonoBehaviour
{
    Rect area
    {
        get
        {
            return gameArea.Area;
        }
    }
    public GameArea gameArea;

    //If we initiliazed this variable in the method fixPosition,
    //GC had to clear it for every screen refresh.
    Vector3 areaPosition;

    void Update()
    {
        fixPosition();
    }

    private void fixPosition()
    {
        areaPosition = gameArea.transform.InverseTransformPoint(transform.position);

        if (area.Contains(areaPosition))
            return;

        if (areaPosition.x < area.xMin)
        {
            areaPosition.x = area.xMax;
        }
        else if (areaPosition.x > area.xMax)
        {
            areaPosition.x = area.xMin;
        }

        if (areaPosition.y < area.yMin)
        {
            areaPosition.y = area.yMax;
        }
        else if (areaPosition.y > area.yMax)
        {
            areaPosition.y = area.yMin;
        }

        transform.position = gameArea.transform.TransformPoint(areaPosition);
    }
}
