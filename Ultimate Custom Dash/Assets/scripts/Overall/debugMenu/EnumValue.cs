using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class EnumValue
{
    public EnumValue(List<string> _values, string _keyName, int _SelectedOne)
    {
        values = _values;
        keyName = _keyName;
        SelectedOne = _SelectedOne;
    }

    public List<string> values;
    public string keyName;
    public int SelectedOne;

    public static List<string> EnumValuesToStrings(Type enumType)
    {
        return System.Enum.GetNames(enumType).ToList();
    }
}
