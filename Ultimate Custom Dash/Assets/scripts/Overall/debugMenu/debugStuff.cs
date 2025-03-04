using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class debugStuff : MonoBehaviour
{
    //overall
    public enum Modes { night, animatronic };
    [BoxGroup("Overall")]
    public Modes Mode;

    NightManager NM;

    //night mode
    [BoxGroup("Night Mode")]
    public GameObject nightPage;
    [BoxGroup("Night Mode")]
    public bool isHoveringOn;
    [BoxGroup("Night Mode")]
    public Transform nightCont;

    //animatronic mode
    [BoxGroup("Animatronic Mode")]
    public GameObject AnimatronicPage;
    [BoxGroup("Animatronic Mode")]
    public TMP_Dropdown animatronicSelect;
    [BoxGroup("Animatronic Mode")]
    public Transform FieldsCont;

    AnimatronicBase currAnimat;
    [BoxGroup("Animatronic Mode")]
    public GameObject
        intFieldPrefab,
        floatFieldPrefab,
        boolFieldPrefab,
        enumFieldPrefab
    ;

    int lastSelectedAnimat;

    // Start is called before the first frame update
    void Awake()
    {
        
    }

    public void OpenMenu()
    {
        if (gameObject.activeSelf)
        {
            gameObject.SetActive(false);
        }
        else
        {
            OnModeChanged((int)Mode);

            NM = NightManager.inctance;
            gameObject.SetActive(true);
            List<TMP_Dropdown.OptionData> options = new List<TMP_Dropdown.OptionData>();
            for (int i = 0; i < NM.Animatronics.Count; i++)
            {
                TMP_Dropdown.OptionData option = new TMP_Dropdown.OptionData();
                option.text = NM.Animatronics[i].Name;
                options.Add(option);
            }

            animatronicSelect.ClearOptions();
            animatronicSelect.AddOptions(options);
            animatronicSelect.value = lastSelectedAnimat;
            OnAnimatronicSelectedChanged(lastSelectedAnimat);
            animatronicSelect.RefreshShownValue();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnModeChanged(Int32 number)
    {
        Mode = (Modes)number;

        switch (Mode)
        {
            case Modes.night:
                nightPage.SetActive(true);
                AnimatronicPage.SetActive(false);
                break;
            case Modes.animatronic:
                nightPage.SetActive(false);
                AnimatronicPage.SetActive(true);
                break;
        }
    }

    
    //animatronic stuff
    public void OnAnimatronicSelectedChanged(Int32 value)
    {
        //getting animatronic
        AnimatronicBase AnimBase = null;

        string SelectedName = "";

        for (int i = 0; i < animatronicSelect.options.Count; i++)
        {
            if (i == value)
            {
                SelectedName = animatronicSelect.options[i].text;
            }
        }

        if (SelectedName == "") return;

        for (int i = 0; i < NM.Animatronics.Count; i++)
        {
            if (NM.Animatronics[i].Name == SelectedName)
            {
                AnimBase = NM.Animatronics[i];
            }
        }

        if (AnimBase == null) return;

        currAnimat = AnimBase;

        lastSelectedAnimat = value;

        //add in custom vals

        for (int i = 0; i < FieldsCont.childCount; i++)
        {
            if (i >= 8)
            {
                Destroy(FieldsCont.GetChild(i).gameObject);
                
            }
        }

        for (int i = 0; i < AnimBase.customVals.Ints.Count; i++)
        {
            if (AnimBase.customVals.Ints[i].keyName != "AILevel")
            {
                if (AnimBase.customVals.Ints[i].keyName != "ExtraChance")
                {
                    Instantiate(intFieldPrefab, FieldsCont).GetComponent<intField>().SetValue(AnimBase.customVals.Ints[i]);
                }
            }
        }

        for (int i = 0; i < AnimBase.customVals.Floats.Count; i++)
        {
            if (AnimBase.customVals.Floats[i].keyName != "Oppretunity")
            {
                Instantiate(floatFieldPrefab, FieldsCont).GetComponent<floatField>().SetValue(AnimBase.customVals.Floats[i]);
            }
        }

        for (int i = 0; i < AnimBase.customVals.Bools.Count; i++)
        {
            if (AnimBase.customVals.Bools[i].keyName != "IsLethal")
            {
                if (AnimBase.customVals.Bools[i].keyName != "TakeDownCams")
                {
                    if (AnimBase.customVals.Bools[i].keyName != "TakeDownMask")
                    {
                        Instantiate(boolFieldPrefab, FieldsCont).GetComponent<boolField>().SetValue(AnimBase.customVals.Bools[i]);
                    }
                }
            }
        }

        for (int i = 0; i < AnimBase.customVals.Enums.Count; i++)
        {
            Instantiate(enumFieldPrefab, FieldsCont).GetComponent<enumField>().SetValue(AnimBase.customVals.Enums[i]);
        }

        //getting animatronic values

        foreach (Transform child in FieldsCont)
        {
            //if its an int
            if (child.TryGetComponent(out intField intF))
            {
                for (int i = 0; i < AnimBase.customVals.Ints.Count; i++)
                {
                    if (AnimBase.customVals.Ints[i].keyName == intF.valueName)
                    {
                        intF.SetValue(AnimBase.customVals.Ints[i]);
                    }
                }
            }

            //if its a Float
            if (child.TryGetComponent(out floatField floatF))
            {
                for (int i = 0; i < AnimBase.customVals.Floats.Count; i++)
                {
                    if (AnimBase.customVals.Floats[i].keyName == floatF.valueName)
                    {
                        floatF.SetValue(AnimBase.customVals.Floats[i]);
                    }
                }
            }

            //if its a Bool
            if (child.TryGetComponent(out boolField boolF))
            {
                for (int i = 0; i < AnimBase.customVals.Bools.Count; i++)
                {
                    if (AnimBase.customVals.Bools[i].keyName == boolF.valueName)
                    {
                        boolF.SetValue(AnimBase.customVals.Bools[i]);
                    }
                }
            }

            //if its a Enum
            if (child.TryGetComponent(out enumField enumF))
            {
                for (int i = 0; i < AnimBase.customVals.Enums.Count; i++)
                {
                    if (AnimBase.customVals.Enums[i].keyName == enumF.valueName)
                    {
                        enumF.SetValue(AnimBase.customVals.Enums[i]);
                    }
                }
            }

        }
    }

    public void applyChanges()
    {
        foreach (Transform child in FieldsCont)
        {
            //if its an int
            if (child.TryGetComponent(out intField intF))
            {
                for (int i = 0; i < currAnimat.customVals.Ints.Count; i++)
                {
                    if (currAnimat.customVals.Ints[i].keyName == intF.valueName)
                    {
                        currAnimat.customVals.Ints[i] = intF.getValue();

                        currAnimat.SetCustomValue(intF.getValue());
                    }
                }
            }

            //if its a Float
            if (child.TryGetComponent(out floatField floatF))
            {
                for (int i = 0; i < currAnimat.customVals.Floats.Count; i++)
                {
                    if (currAnimat.customVals.Floats[i].keyName == floatF.valueName)
                    {
                        currAnimat.customVals.Floats[i] = floatF.getValue();

                        currAnimat.SetCustomValue(floatF.getValue());
                    }
                }
            }

            //if its a Bool
            if (child.TryGetComponent(out boolField boolF))
            {
                for (int i = 0; i < currAnimat.customVals.Bools.Count; i++)
                {
                    if (currAnimat.customVals.Bools[i].keyName == boolF.valueName)
                    {
                        currAnimat.customVals.Bools[i] = boolF.getValue();

                        currAnimat.SetCustomValue(boolF.getValue());
                    }
                }
            }

            //if its a Enum
            if (child.TryGetComponent(out enumField enumF))
            {
                for (int i = 0; i < currAnimat.customVals.Enums.Count; i++)
                {
                    if (currAnimat.customVals.Enums[i].keyName == enumF.valueName)
                    {
                        currAnimat.customVals.Enums[i] = enumF.getValue();

                        currAnimat.SetCustomValue(enumF.getValue());
                    }
                }
            }

        }
    }

    public void UpdateNightVariables(float power, int usage, int noise, int fazCoins, float temper, cameraSystem.Cameras camera, NightManager.SpecialModes Smode)
    {
        if (!isHoveringOn)
        {
            foreach (Transform child in nightCont)
            {
                if (child.TryGetComponent(out intField intF))
                {
                    switch (intF.valueName)
                    {
                        case "Usage":
                            intF.SetValue(new IntValue(usage, "Usage"));
                            break;
                        case "Noise":
                            intF.SetValue(new IntValue(noise, "Noise"));
                            break;
                        case "FazCoins":
                            intF.SetValue(new IntValue(fazCoins, "FazCoins"));
                            break;
                    }
                }

                if (child.TryGetComponent(out floatField floatF))
                {
                    switch (floatF.valueName)
                    {
                        case "Power":
                            floatF.SetValue(new FloatValue(power, "Power"));
                            break;
                        case "tempreture":
                            floatF.SetValue(new FloatValue(temper, "tempreture"));
                            break;
                    }
                }
                if (child.TryGetComponent(out enumField EF))
                {
                    switch (EF.valueName)
                    {
                        case "curr camera":
                            EF.SetValue(new EnumValue(EnumValue.EnumValuesToStrings(typeof(cameraSystem.Cameras)), "curr camera", (int)camera));
                            break;
                        case "CurrSpecialMode":
                            EF.SetValue(new EnumValue(EnumValue.EnumValuesToStrings(typeof(NightManager.SpecialModes)), "CurrSpecialMode", (int)Smode));
                            break;
                    }
                }
            }
        }
    }

    public void changeHoverState(bool b)
    {
        isHoveringOn = b;
    }
}
