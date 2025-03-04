using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;

public class intField : MonoBehaviour
{
    public TMP_InputField input;
    public TextMeshProUGUI text;

    public string valueName;

    public void SetName(string name)
    {
        valueName = name;
        text.text = name + ":";
        gameObject.name = name;
    }

    public void SetValue(IntValue val)
    {
        SetName(val.keyName);
        input.text = val.value.ToString();
    }

    public IntValue getValue()
    {
        return new IntValue(int.Parse(input.text), valueName);
    }
}
