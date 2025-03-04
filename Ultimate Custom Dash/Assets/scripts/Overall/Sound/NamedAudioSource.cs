using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NamedAudioSource
{
    public NamedAudioSource(string _name, AudioSource _source, bool _SoundEffect)
    {
        name = _name;
        source = _source;
        SoundEffect = _SoundEffect;
    }
    public string name;
    public AudioSource source;
    public bool SoundEffect;
}
