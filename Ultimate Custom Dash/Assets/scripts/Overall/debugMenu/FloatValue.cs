using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FloatValue
{
    public FloatValue(float _value, string _keyName)
    {
        value = _value;
        keyName = _keyName;
    }
    public float value;
    public string keyName;
}
