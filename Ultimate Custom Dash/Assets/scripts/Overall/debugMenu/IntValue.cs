using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class IntValue
{
    public IntValue(int _value, string _keyName)
    {
        value = _value;
        keyName = _keyName;
    }
    public int value;
    public string keyName;
}
