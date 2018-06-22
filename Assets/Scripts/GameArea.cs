using UnityEngine;

/// <summary>
/// Defines a rectengular game area.
/// </summary>
[AddComponentMenu("OzzGames/Game Area")]
public class GameArea : MonoBehaviour
{
    public GameArea()
    {
        if (_firstInstance == null)
            _firstInstance = this;
    }

    public Vector2 size;
    public Color gizmoColor = new Color(0, 0, 1, 0.2f);
    Color gizmoWireColor;

    public Rect Area
    {
        get { return _area; }
        set { _area = value; }
    }
    [SerializeField]
    [HideInInspector]
    Rect _area;

    public static GameArea Main
    {
        get
        {
            if (_firstInstance == null)
            {
                GameObject go = new GameObject("GameArea Main");
                go.AddComponent<GameArea>(); //instantiating first one
                go.AddComponent<FitGameAreaToCamera>();
            }
            return _firstInstance;
        }
    }
    static GameArea _firstInstance;

    public Vector2 Size
    {
        get { return Area.size; }
        set
        {
            size = value;
            Area = new Rect(size.x * -0.5f, size.y * -0.5f, size.x, size.y);
        }
    }

    private void Awake()
    {
        Size = size;
    }

    private void OnValidate()
    {
        gizmoWireColor = new Color(gizmoColor.r, gizmoColor.g, gizmoColor.b, 1);
        Size = size;
    }

    private void OnDrawGizmos()
    {
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.color = gizmoColor;
        Gizmos.DrawCube(Vector3.zero, new Vector3(Area.width, Area.height, 0));
        Gizmos.color = gizmoWireColor;
        Gizmos.DrawWireCube(Vector3.zero, new Vector3(Area.width, Area.height, 0));
        Size = size;
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