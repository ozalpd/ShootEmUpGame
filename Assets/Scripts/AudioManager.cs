using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public AudioMixer mixer;

    private void Start()
    {
        setMixerVolume("VolumeMusic", PlayerSettings.MusicVolume);
        setMixerVolume("VolumeSFX", PlayerSettings.SfxVolume);

        PlayerSettings.MusicVolumeChanged += (float volume) =>
        {
            setMixerVolume("VolumeMusic", volume);
        };

        PlayerSettings.SfxVolumeChanged += (volume) =>
        {
            setMixerVolume("VolumeSFX", volume);
        };
    }

    void setMixerVolume(string mixerName, float volume)
    {
        if (!(volume > 0))
            volume = 0.0000025f;
        float dbVol = 20 * Mathf.Log10(volume);
        mixer.SetFloat(mixerName, dbVol);
        //Debug.Log("volume: " + volume + " dbVol: " + dbVol);
    }
}
