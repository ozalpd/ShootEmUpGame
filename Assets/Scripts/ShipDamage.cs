using UnityEngine;

/// <summary>
/// Keeps damage of player object.
/// </summary>
public class ShipDamage : MonoBehaviour
{
    [Range(1, 100)]
    public float vulnerability = 5;

    public GameObject impact;
    int _impactId;

    Collider2D _collider;
    Rigidbody2D _rigidbody;

    private void Awake()
    {
        _collider = GetComponent<Collider2D>();
        _rigidbody = GetComponent<Rigidbody2D>();

        _impactId = impact.GetInstanceID();
        ObjectPool.GetOrInitPool(impact, 50);//default value 200 is too much for this
    }

    public float Damage
    {
        get { return GameManager.Damage; }
        set { GameManager.Damage = value; }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        float damage = collision.relativeVelocity.magnitude;
        if (collision.collider.sharedMaterial != null)
        {
            var sharedMaterial = collision.collider.sharedMaterial;
            damage *= _collider.sharedMaterial.friction
                               * sharedMaterial.friction
                               * (1 / sharedMaterial.bounciness)
                               * (1 / _collider.sharedMaterial.friction);
        }

        if (collision.rigidbody != null)
        {
            damage *= collision.rigidbody.mass / _rigidbody.mass;
        }

        if (damage > 0.01f)
        {
            ObjectPool.GetInstance(_impactId, collision.contacts[0].point);
        }

        Damage += vulnerability * damage;
    }
}