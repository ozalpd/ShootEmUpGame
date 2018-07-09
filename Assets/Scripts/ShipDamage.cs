using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Keeps damage of the player and shows an impact animation.
/// </summary>
public class ShipDamage : MonoBehaviour
{
    [Range(1, 100)]
    public float vulnerability = 5;

    public GameObject impact;
    int _impactId;
    public GameObject explosion;
    int _explosionId;
    public Transform[] explosionLocations;
    [Range(0.01f, 0.5f)]
    public float explosionWaits = 0.2f;
    float _explosionDuration;

    Collider2D _collider;
    Rigidbody2D _rigidbody;

    private void Awake()
    {
        _collider = GetComponent<Collider2D>();
        _rigidbody = GetComponent<Rigidbody2D>();

        _impactId = impact.GetInstanceID();
        ObjectPool.GetOrInitPool(impact, 50);//default value 200 is too much for this

        _explosionId = explosion.GetInstanceID();
        ObjectPool.GetOrInitPool(explosion, explosionLocations.Length);
        _explosionDuration = explosionWaits * (float)explosionLocations.Length;

        prevLives = GameManager.Lives;
        GameManager.LivesChanged += GameManager_LivesChanged;
    }

    public float Damage
    {
        get { return GameManager.Damage; }
        set { GameManager.Damage = value; }
    }

    //this class is not the right place for that event handler
    void GameManager_LivesChanged(int lives)
    {
        if (prevLives > lives)
            Die();
        prevLives = lives;
    }
    int prevLives;

    //this class is not the right place for that method
    public void Die()
    {
        StartCoroutine(ReSpawn());
    }

    private IEnumerator RunExplosions()
    {
        GameObject prevGO = null;
        Transform prevTr = null;
        foreach (var t in explosionLocations)
        {
            var go = ObjectPool.GetInstance(_explosionId, t.position, t.rotation);
            yield return new WaitForSeconds(explosionWaits);
            go.transform.position = t.position;

            if (prevGO != null)
                prevGO.transform.position = prevTr.transform.position;
            prevTr = t;
            prevGO = go;
        }
    }

    //this class is not the right place for that method
    private IEnumerator ReSpawn()
    {
        //disable colliders
        _colliders = GetComponentsInChildren<Collider2D>(false);
        foreach (var c in _colliders)
        {
            c.enabled = false;
        }

        //disable user controls
        GameManager.UserControlsEnabled = false;

        //run explosions
        //yield return 
            StartCoroutine(RunExplosions());
        yield return new WaitForSeconds(_explosionDuration * 0.75f);

        //disable renderers
        _renderers = GetComponentsInChildren<Renderer>(false);
        foreach (var r in _renderers)
        {
            r.enabled = false;
        }

        GetComponent<AbstractPlayerController>().ResetValues();

        yield return new WaitForSeconds(1);
        //enable renderers
        foreach (var r in _renderers)
        {
            r.enabled = true;
        }

        //enable user controls
        GameManager.UserControlsEnabled = true;

        //set animator Respawn  to true
        _animator = GetComponent<Animator>();
        if (_animator != null)
            _animator.SetBool("Respawn", true);

        //wait for 3 sec
        yield return new WaitForSeconds(3);

        //enable colliders
        foreach (var c in _colliders)
        {
            c.enabled = true;
        }

        //set animator Respawn to false
        if (_animator != null)
            _animator.SetBool("Respawn", false);
    }
    Renderer[] _renderers;
    Collider2D[] _colliders;
    Animator _animator;

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