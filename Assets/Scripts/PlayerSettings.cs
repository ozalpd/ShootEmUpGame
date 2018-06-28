using UnityEngine;
using System;

public static class PlayerSettings
{
    public delegate void VolumeChange(float volume);

    public static float MusicVolume
    {
        get
        {
            if (_musicVolume == null)
                _musicVolume = PlayerPrefs.GetFloat("MusicVolume", 0.8f);
            return _musicVolume.Value;
        }
        set
        {
            if (_musicVolume != value)
            {
                _musicVolume = value;
                PlayerPrefs.SetFloat("MusicVolume", _musicVolume.Value);

                if (MusicVolumeChanged != null)
                    MusicVolumeChanged(_musicVolume.Value);
            }
        }
    }
    static float? _musicVolume;
    public static event VolumeChange MusicVolumeChanged;


    public static float SfxVolume
    {
        get
        {
            if (_sfxVolume == null)
                _sfxVolume = PlayerPrefs.GetFloat("SfxVolume", 0.8f);
            return _sfxVolume.Value;
        }
        set
        {
            if (_sfxVolume != value)
            {
                _sfxVolume = value;
                PlayerPrefs.SetFloat("SfxVolume", _sfxVolume.Value);

                if (SfxVolumeChanged != null)
                    SfxVolumeChanged(_sfxVolume.Value);
            }
        }
    }
    static float? _sfxVolume;
    public static event VolumeChange SfxVolumeChanged;
}