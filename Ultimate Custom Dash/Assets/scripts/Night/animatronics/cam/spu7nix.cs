using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class spu7nix : AnimatronicBase
{
    public GameObject cam05Sput;
    public GameObject cam06Sput;

    bool onCam05;
    bool isOn;

    float escapeTimer;
    public float escapeTime;
    public override void AnimatronicGameStart()
    {
        AddCustomValue(new FloatValue(escapeTime, "escapeTime"));
    }

    //called when animatronic gets his AILevel
    public override void AnimatronicStart()
    {
        if (!GM.silent)
        {
            OppretunityEvery = 32;
            OppretunityTimer = OppretunityEvery;
        }
    }

    //called every frame after the Oppretunity calculations
    public override void AnimatronicUpdate()
    {
        if (isOn)
        {
            if (onCam05)
            {
                if (NM.CamSys.CurrentCamera == cameraSystem.Cameras.Cam05 && NM.CamsFullyOpened)
                {
                    escapeTimer -= Time.deltaTime;

                    if (escapeTimer <= 0)
                    {
                        isOn = false;
                        cam05Sput.SetActive(false);
                        NM.CamSys.PulseStatic();
                    }
                }
            }
            else
            {
                if (NM.CamSys.CurrentCamera == cameraSystem.Cameras.Cam06 && NM.CamsFullyOpened)
                {
                    escapeTimer -= Time.deltaTime;

                    if (escapeTimer <= 0)
                    {
                        isOn = false;
                        cam06Sput.SetActive(false);
                        NM.CamSys.PulseStatic();
                    }
                }
            }
        }
    }

    //called every oppretunity
    public override void OnOppretunity()
    {
        if (!isOn)
        {
            cam05Sput.SetActive(false);
            cam06Sput.SetActive(false);
            isOn = true;
            onCam05 = randomBool();
            escapeTimer = escapeTime;

            if (onCam05)
            {
                cam05Sput.SetActive(true);

                if (NM.CamSys.CurrentCamera == cameraSystem.Cameras.Cam05)
                {
                    NM.CamSys.PulseStatic();
                }
            }
            else
            {
                cam06Sput.SetActive(true);

                if (NM.CamSys.CurrentCamera == cameraSystem.Cameras.Cam06)
                {
                    NM.CamSys.PulseStatic();
                }
            }

            
        }
        else
        {
            Jumpscare();
        }
    }

    bool randomBool() { return Random.value > 0.5f; }

    //called when deathcoined
    public override void OnDeathcoined()
    {
        base.OnDeathcoined();
        isOn = false;
        cam06Sput.SetActive(false);
        cam05Sput.SetActive(false);
        NM.CamSys.PulseStatic();
    }

    //called when someone kills the player
    public override void OnPlayerDied()
    {
        
    }

    public override void SetCustomValue(FloatValue value)
    {
        if (value.keyName == "escapeTime")
        {
            escapeTime = value.value;
        }

        base.SetCustomValue(value);
    }

    public override void OnMirrorSummon(int AIChosen)
    {
        base.OnMirrorSummon(AIChosen);
    }
}
