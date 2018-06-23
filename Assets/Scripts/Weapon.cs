using UnityEngine;
using System.Collections;

public class Weapon : MonoBehaviour
{
    public GameObject projectile;

    [Range(-5f, 5f)]
    public float firingOffset = 0.5f;
    [Range(0.01f, 10f)]
    public float firingRate = 1;
    [Range(1f, 10f)]
    public float firingRange = 5;

    public Transform[] emmitters;
    int _emmitterIdx = -1;
    Collider2D _shipCollider2D;

    void Awake()
    {
        _shipCollider2D = transform.parent.GetComponent<Collider2D>();
    }

    void fire()
    {
        if (emmitters != null && emmitters.Length > 0)
            _emmitterIdx++;

        if (emmitters != null && !(_emmitterIdx < emmitters.Length))
            _emmitterIdx = 0;

        var pTransform = _emmitterIdx > -1 ? emmitters[_emmitterIdx] : transform;
        var position = pTransform.TransformPoint(Vector3.up * firingOffset);
        var go = (GameObject)Instantiate(projectile, position, pTransform.rotation);
        var proj = go.GetComponent<Projectile>();
        if (proj != null)
            proj.range = firingRange;
        var projectileCollider = go.GetComponent<Collider2D>();
        if (projectileCollider != null)
            Physics2D.IgnoreCollision(_shipCollider2D, projectileCollider);
    }
}
