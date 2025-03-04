using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BoolValue
{
    public BoolValue(bool _value, string _keyName)
    {
        value = _value;
        keyName = _keyName;
    }
    public bool value;
    public string keyName;
}
