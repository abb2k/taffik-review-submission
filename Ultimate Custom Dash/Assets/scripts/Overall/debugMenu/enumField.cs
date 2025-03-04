using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Windows;

public class enumField : MonoBehaviour
{
    public TMP_Dropdown input;
    public TextMeshProUGUI text;

    public string valueName;

    public void SetName(string name)
    {
        valueName = name;
        text.text = name + ":";
        gameObject.name = name;
    }

    public void SetValue(EnumValue val)
    {
        SetName(val.keyName);

        List<TMP_Dropdown.OptionData> options = new List<TMP_Dropdown.OptionData>();

        for (int i = 0; i < val.values.Count; i++)
        {
            TMP_Dropdown.OptionData option = new TMP_Dropdown.OptionData();
            option.text = val.values[i];
            options.Add(option);
        }

        input.ClearOptions();
        input.AddOptions(options);
        input.SetValueWithoutNotify(val.SelectedOne);
        input.RefreshShownValue();
    }

    public EnumValue getValue()
    {
        List<string> names = new List<string>();
        for (int i = 0; i < input.options.Count; i++)
        {
            names.Add(input.options[i].text);
        }

        return new EnumValue(names, valueName, input.value);
    }
}
