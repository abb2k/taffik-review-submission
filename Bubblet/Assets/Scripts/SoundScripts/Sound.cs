using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public string SoundName;
    public AudioClip Clip;
    [Range(0,1)]
    public float VolumeBalance = 1;
}
