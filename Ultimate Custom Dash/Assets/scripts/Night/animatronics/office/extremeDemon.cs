using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class extremeDemon : AnimatronicBase
{
    public AudioClip warning;

    bool blockOrKill = true;

    float killTimer = 1.4f;

    //called when animatronic gets his AILevel
    public override void AnimatronicStart()
    {
        
    }

    //called every frame after the Oppretunity calculations
    public override void AnimatronicUpdate()
    {
        if (NM.NightOngoing)
        {
            if (killTimer > 0)
            {
                killTimer -= Time.deltaTime;
            }
            else
            {
                if (!blockOrKill)
                {
                    blockOrKill = true;
                    NM.StartEffectsBlackscreen(false);
                    if (NM.bottomRightDoorClosed)
                    {
                        Blocked();
                    }
                    else
                    {
                        Jumpscare();
                    }
                }
            }
        }
    }

    //called every oppretunity
    public override void OnOppretunity()
    {
        GM.soundManager.CreateSoundEffect("warning", warning);

        blockOrKill = false;
        NM.StartEffectsBlackscreen(true);
        killTimer = 1.4f;
    }

    //called when deathcoined
    public override void OnDeathcoined()
    {
        
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
