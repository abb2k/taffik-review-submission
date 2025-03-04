using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class solarFlareSun : AnimatronicBase
{
    float progress;

    public SpriteRenderer Sun;

    bool kill;

    public float DisappearSpeed = 25;

    public float TemperatureMultiplier = 1;

    public float startingTemperature = 80;

    public override void AnimatronicGameStart()
    {
        AddCustomValue(new FloatValue(DisappearSpeed, "DisappearSpeed"));
        AddCustomValue(new FloatValue(TemperatureMultiplier, "TemperatureMultiplier"));
        AddCustomValue(new FloatValue(startingTemperature, "startingTemperature"));
    }

    //called when animatronic gets his AILevel
    public override void AnimatronicStart()
    {
        
    }

    //called every frame after the Oppretunity calculations
    public override void AnimatronicUpdate()
    {
        if (NM.NightOngoing)
        {
            if (NM.Temperature < startingTemperature)
            {
                //remove
                progress -= Time.deltaTime * DisappearSpeed;
            }
            else
            {
                //add
                progress += Time.deltaTime * AILevel * ((NM.Temperature - 70) / 50) * TemperatureMultiplier;
            }

            if (progress < 0)
            {
                progress = 0;
            }

            if (progress >= 255)
            {
                progress = 255;
                if (!kill)
                {
                    kill = true;
                    Sun.gameObject.SetActive(false);
                    Jumpscare();
                }
            }

            Sun.color = new Color32(255, 255, 255, (byte)progress);
        }
    }

    //called every oppretunity
    public override void OnOppretunity()
    {
        
    }

    //called when deathcoined
    public override void OnDeathcoined()
    {
        base.OnDeathcoined();
        Sun.color = new Color(1, 1, 1, 0);
        progress = 0;
    }

    //called when someone kills the player
    public override void OnPlayerDied()
    {
        Sun.gameObject.SetActive(false);
    }

    public override void SetCustomValue(FloatValue value)
    {
        if (value.keyName == "DisappearSpeed")
        {
            DisappearSpeed = value.value;
        }
        if (value.keyName == "TemperatureMultiplier")
        {
            TemperatureMultiplier = value.value;
        }
        if (value.keyName == "startingTemperature")
        {
            startingTemperature = value.value;
        }

        base.SetCustomValue(value);
    }

    public override void OnMirrorSummon(int AIChosen)
    {
        base.OnMirrorSummon(AIChosen);
    }
}
