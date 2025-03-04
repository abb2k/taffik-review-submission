using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class bloodbath : AnimatronicBase
{
    public SpriteRenderer Face;

    float anger;

    //called when animatronic gets his AILevel
    public override void AnimatronicStart()
    {
        if (AILevel == 0)
        {
            Face.gameObject.SetActive(false);
        }
        else
        {
            Face.gameObject.SetActive(true);
        }
        Face.color = new Color32(255, 255, 255, (byte)(anger * 2.55f));
    }

    //called every frame after the Oppretunity calculations
    public override void AnimatronicUpdate()
    {
        
    }

    //called every oppretunity
    public override void OnOppretunity()
    {
        if (NM.CurrSpecialMode == NightManager.SpecialModes.SilentVent)
        {
            if (anger > 0)
            {
                anger -= 0.05f * 20;
                if (anger <= 0)
                {
                    anger = 0;
                }
            }
        }
        if (NM.CamsFullyOpened)
        {
            if (anger < 100)
            {
                anger += 0.025f * AILevel;

                if (anger >= 100)
                {
                    anger = 100;
                    Jumpscare();
                }
            }
        }

        Face.color = new Color32(255, 255, 255, (byte)(anger * 2.55f));
    }

    //called when deathcoined
    public override void OnDeathcoined()
    {
        base.OnDeathcoined();
        anger = 0;
        Face.color = new Color32(255, 255, 255, (byte)(anger * 2.55f));
        Face.gameObject.SetActive(false);
    }

    //called when someone kills the player
    public override void OnPlayerDied()
    {
        Face.gameObject.SetActive(false);
    }

    public override void OnMirrorSummon(int AIChosen)
    {
        base.OnMirrorSummon(AIChosen);

        Face.gameObject.SetActive(true);
    }
}
