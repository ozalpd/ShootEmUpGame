using UnityEngine;
using System.Collections;

public class Weapon : MonoBehaviour
{
    public GameObject projectile;

    [Range(-5f, 5f)]
    public float firingOffset = 0.5f;
    [Range(0.01f, 10f)]
    public float firingRate = 1;
    [Range(5f, 20f)]
    public float firingRange = 10;

    public Transform[] emmitters;
    int _emmitterIndex = -1;
    Collider2D _shipCollider2D;
    int _projectileId;

    void Awake()
    {
        _shipCollider2D = transform.parent.GetComponent<Collider2D>();
        _projectileId = projectile.GetInstanceID();
        ObjectPool.GetOrInitPool(projectile);
    }

    void fire()
    {
        if (emmitters != null && emmitters.Length > 0)
            _emmitterIndex++;

        if (emmitters != null && !(_emmitterIndex < emmitters.Length))
            _emmitterIndex = 0;

        var pTransform = _emmitterIndex > -1 ? emmitters[_emmitterIndex] : transform;
        var position = pTransform.TransformPoint(Vector3.up * firingOffset);
        var go = ObjectPool.GetInstance(_projectileId, position, pTransform.rotation);

        var projInstance = go.GetComponent<Projectile>();
        if (projInstance != null)
            projInstance.range = firingRange;

        var projectileCollider = go.GetComponent<Collider2D>();
        if (projectileCollider != null)
            Physics2D.IgnoreCollision(_shipCollider2D, projectileCollider);
    }
}
