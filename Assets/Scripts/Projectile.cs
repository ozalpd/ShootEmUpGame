using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour
{
    public float speed = 0.5f;

    [System.NonSerialized]
    public float range = 3;
    float _distance = 0;

    AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        _audioSource.Play();
        _distance = 0;
    }

    private void FixedUpdate()
    {
        transform.Translate(0, speed * Time.fixedDeltaTime, 0, transform);
        _distance += speed * Time.fixedDeltaTime;

        if (_distance > range)
            ObjectPool.Release(gameObject);
    }
}