using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Makes the game object to be shootable by other game objects those are tagged as Projectile.
/// </summary>
public class Shootable : MonoBehaviour
{
    public int score = 5;
    public GameObject explosion;
    int _explosionId;

    private void Awake()
    {
        if (explosion != null)
        {
            _explosionId = explosion.GetInstanceID();
            ObjectPool.GetOrInitPool(explosion);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.tag.Equals("Projectile"))
            return;

        if (explosion != null)
        {
            ObjectPool.GetInstance(_explosionId, collision.contacts[0].point);
        }

        GameManager.Score += score;
        ObjectPool.Release(gameObject);
    }
}
