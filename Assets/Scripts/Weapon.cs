using UnityEngine;
using System.Collections;

public class Weapon : MonoBehaviour
{
    public new string name;
    public GameObject projectile;
    int _projectileId;

    [Range(-5f, 5f)]
    public float firingOffset = 0.5f;
    [Range(0.01f, 10f)]
    public float firingRate = 1;
    float repeatRate;

    [Range(5f, 20f)]
    public float firingRange = 10;
    public bool automatic = true;

    [Tooltip("Fires projectiles at every emmitters once")]
    public bool fireAllAtOnce;
    public Transform[] emmitters;
    int _emmitterIndex = -1;

    Collider2D _shipCollider2D;

    void Awake()
    {
        _shipCollider2D = transform.parent.GetComponent<Collider2D>();
        _projectileId = projectile.GetInstanceID();
        ObjectPool.GetOrInitPool(projectile);

        if (string.IsNullOrEmpty(name))
            name = gameObject.name;
    }

    public bool Firing
    {
        set
        {
            if (_isFiring != value)
            {
                _isFiring = value;
                repeatRate = 1 / firingRate;

                if (_isFiring)
                {
                    if (automatic)
                        InvokeRepeating("fire", repeatRate, repeatRate);
                    else
                        fire();
                }
                else
                    CancelInvoke("fire");
            }
        }
        get { return _isFiring; }
    }
    bool _isFiring;

    void fire()
    {
        if (emmitters == null || emmitters.Length == 0)
            Debug.LogWarning("Weapon has no emmitters!");

        if (fireAllAtOnce)
        {
            for (int i = 0; i < emmitters.Length; i++)
            {
                fire(i);
            }
        }
        else
        {
            _emmitterIndex++;

            if (emmitters != null && !(_emmitterIndex < emmitters.Length))
                _emmitterIndex = 0;
            fire(_emmitterIndex);
        }
    }

    void fire(int emmitterIndex)
    {
        var pTransform = emmitterIndex > -1 ? emmitters[emmitterIndex] : transform;
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
