using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Animatronic
{
    public Animatronic(string _Name, int _AILevel)
    {
        Name = _Name;
        AILevel = _AILevel;
    }
    public string Name;
    public int AILevel;
}
