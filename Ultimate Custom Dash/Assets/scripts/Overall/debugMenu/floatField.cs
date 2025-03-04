using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class floatField : MonoBehaviour
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

    public void SetValue(FloatValue val)
    {
        SetName(val.keyName);
        input.text = val.value.ToString();
    }

    public FloatValue getValue()
    {
        return new FloatValue(float.Parse(input.text, CultureInfo.InvariantCulture.NumberFormat), valueName);
    }
}
