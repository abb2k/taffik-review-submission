using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CustomValues
{
    public List<IntValue> Ints = new List<IntValue>();
    public List<FloatValue> Floats = new List<FloatValue>();
    public List<BoolValue> Bools = new List<BoolValue>();
    public List<EnumValue> Enums = new List<EnumValue>();
}
