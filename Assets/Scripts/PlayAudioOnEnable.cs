using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
/// <summary>
/// Plays AudioSource on enable.
/// </summary>
public class PlayAudioOnEnable : MonoBehaviour
{
    AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        _audioSource.Play();
    }
}
