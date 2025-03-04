using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class nSwish : AnimatronicBase
{
    float anger;

    float clickTimer;

    //called when animatronic gets his AILevel
    public override void AnimatronicStart()
    {
        
    }

    //called every frame after the Oppretunity calculations
    public override void AnimatronicUpdate()
    {
        if (NM.NightOngoing)
        {
            anger += Time.deltaTime * (NM.NoiseLevel / 4.5f) * (AILevel / 4);

            if (NM.NoiseLevel == 0)
            {
                if (anger > 0)
                {
                    anger -= Time.deltaTime * 3;

                    if (anger <= 0)
                    {
                        anger = 0;
                    }
                }
            }

            if (anger >= 100)
            {
                anger = 0;
                Jumpscare();
            }

            if (anger > 20)
            {
                if (clickTimer > 0)
                {
                    clickTimer -= Time.deltaTime;
                }
                else
                {
                    if (anger < 90)
                    {
                        clickTimer = (100 - anger) / 26.666666666666666666666666666667f;
                    }
                    else
                    {
                        clickTimer = 10 / 26.666666666666666666666666666667f;
                    }
                    GM.soundManager.CreateSoundEffect("nSwishClick", GM.soundManager.GetSoundFromList("nSwishClick"));
                }
            }
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
        anger = 0;
    }

    //called when someone kills the player
    public override void OnPlayerDied()
    {
        
    }

    public override void OnMirrorSummon(int AIChosen)
    {
        base.OnMirrorSummon(AIChosen);
    }
}
