using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Defines a rectengular game area.
/// </summary>
[AddComponentMenu("OzzGames/Game Area")]
[RequireComponent(typeof(RectTransform))]
public class GameArea : MonoBehaviour
{
    public Color gizmoColor = new Color(0, 0, 1, 0.2f);
    Color gizmoWireColor;

    private void OnValidate()
    {
        gizmoWireColor = new Color(gizmoColor.r, gizmoColor.g, gizmoColor.b, 1);
    }

    private void OnDrawGizmos()
    {
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.color = gizmoColor;
        Gizmos.DrawCube(Area.center, new Vector3(Area.width, Area.height, 0));

        Gizmos.color = gizmoWireColor;
        Gizmos.DrawWireCube(Area.center, new Vector3(Area.width, Area.height, 0));
    }

    public static GameArea Main
    {
        get
        {
            if (_mainArea == null)
            {
                var areas = FindObjectsOfType<GameArea>();
                if (areas.Length == 1)
                {
                    _mainArea = areas[0];
                }
                else if (areas.Length > 0)
                {
                    if (false)
                    {
                        //This is what the instructor told
                        List<GameArea> listAreas = areas.ToList();
                        listAreas.Sort((f1, f2) => f2.Area.size.magnitude.CompareTo(f1.Area.size.magnitude));
                        _mainArea = listAreas[0];
                    }
                    else //This is what I like to use
                        _mainArea = areas.OrderByDescending(a => a.Area.size.magnitude).First();
                }
            }
            if (_mainArea == null)
            {
                GameObject go = new GameObject("GameArea Main");
                go.AddComponent<GameArea>(); //instantiating first one
                //go.AddComponent<FitGameAreaToCamera>(); //We are replacing this with Unity Canvas system
                Canvas canvas = go.AddComponent<Canvas>();
                canvas.renderMode = RenderMode.ScreenSpaceCamera;
                canvas.planeDistance = -Camera.main.transform.position.z;
                canvas.worldCamera = Camera.main;
            }
            return _mainArea;
        }
    }
    static GameArea _mainArea;


    public Rect Area
    {
        get { return RectTransform.rect; }
        set
        {
            RectTransform.sizeDelta = new Vector2(value.x, value.y);
        }
    }

    public RectTransform RectTransform
    {
        get
        {
            if (_rectTransform == null)
                _rectTransform = GetComponent<RectTransform>();
            return _rectTransform;
        }
        set { _rectTransform = value; }
    }
    private RectTransform _rectTransform;

    public Vector2 Size
    {
        get { return Area.size; }
        set
        {
            RectTransform.sizeDelta = new Vector2(value.x, value.y);
        }
    }

    /// <summary>
    /// Gets a random position in GameArea.
    /// </summary>
    /// <returns>A random position.</returns>
    /// <param name="player">Player's transformation.</param>
    /// <param name="minDistToPlayer">Minimum distance to player's position.</param>
    public Vector3 GetRandomPosition(Transform player = null, float minDistToPlayer = 0, bool fromTop = false)
    {
        Vector3 randPos = new Vector3(
                        Random.Range(Area.xMin, Area.xMax),
                        fromTop ? Area.yMax : Random.Range(Area.yMin, Area.yMax),
                        0);

        //Transforms position from local space to world space.
        randPos = transform.TransformPoint(randPos);

        if (minDistToPlayer < Area.height && minDistToPlayer < Area.width)
        {
            if (player != null && Vector2.Distance(randPos, player.position) < minDistToPlayer)
            {
                //below line did not work well.
                //randPos = minDistToPlayer * (randPos - player.position).normalized;

                //To get a fresh one might be a better choice
                randPos = GetRandomPosition(player, minDistToPlayer);
            }
        }
        else
        {
            Debug.LogWarning("The parameter minDistToPlayer for GetRandomPosition is too big for this game area!");
        }
        return randPos;
    }
}