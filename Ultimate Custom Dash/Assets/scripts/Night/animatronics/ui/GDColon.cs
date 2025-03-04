using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GDColon : AnimatronicBase
{
    public Image mask;
    public Image Colmask;

    public float Anger;

    public float AngerMultiplier;

    public override void AnimatronicGameStart()
    {
        AddCustomValue(new FloatValue(AngerMultiplier, "AngerMultiplier"));
    }


    //called when animatronic gets his AILevel
    public override void AnimatronicStart()
    {
        if (AILevel == 0)
        {
            mask.color = Color.white;
            Colmask.color = new Color(1, 1, 1, 0);
        }
        else
        {
            mask.color = new Color(1,1,1,0);
            Colmask.color = Color.white;
        }
    }

    //called every frame after the Oppretunity calculations
    public override void AnimatronicUpdate()
    {
        if (NM.NightOngoing)
        {
            if (NM.InMask)
            {
                Anger += Time.deltaTime * AngerMultiplier * AILevel;

                if (Anger >= 100)
                {
                    Jumpscare();
                }
            }
            else
            {
                if (Anger > 0) Anger -= Time.deltaTime * AngerMultiplier * 12;
                else Anger = 0;
            }

            if (AILevel > 0) Colmask.color = new Color(1, 1 - (Anger / 100), 1 - (Anger / 100), 1);
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
        mask.color = Color.white;
        Colmask.color = new Color(1, 1, 1, 0);
    }

    //called when someone kills the player
    public override void OnPlayerDied()
    {
        
    }

    public override void SetCustomValue(FloatValue value)
    {
        if (value.keyName == "AngerMultiplier")
        {
            AngerMultiplier = value.value;
        }

        base.SetCustomValue(value);
    }

    public override void OnMirrorSummon(int AIChosen)
    {
        base.OnMirrorSummon(AIChosen);

        mask.color = new Color(1, 1, 1, 0);
        Colmask.color = Color.white;
    }
}
