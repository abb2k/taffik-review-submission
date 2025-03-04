using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class boolField : MonoBehaviour
{
    public Toggle input;
    public TextMeshProUGUI text;

    public string valueName;

    public void SetName(string name)
    {
        valueName = name;
        text.text = name + ":";
        gameObject.name = name;
    }

    public void SetValue(BoolValue val)
    {
        SetName(val.keyName);
        input.isOn = val.value;
    }

    public BoolValue getValue()
    {
        return new BoolValue(input.isOn, valueName);
    }
}
