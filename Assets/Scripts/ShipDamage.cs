using UnityEngine;

public class ShipDamage : MonoBehaviour
{
    Collider2D _collider;
    Rigidbody2D _rigidbody;

    private void Awake()
    {
        _collider = GetComponent<Collider2D>();
        _rigidbody = GetComponent<Rigidbody2D>();
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

        Damage += damage;
    }
}