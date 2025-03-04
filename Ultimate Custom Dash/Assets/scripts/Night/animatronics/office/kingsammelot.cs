using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class kingsammelot : AnimatronicBase
{
    public GameObject sammi;

    public float killtime;
    float killtimer;

    bool inRoom;

    public int AvoidStage;

    public override void AnimatronicGameStart()
    {
        AddCustomValue(new FloatValue(killtime, "killtime"));
    }

    //called when animatronic gets his AILevel
    public override void AnimatronicStart()
    {
        killtimer = killtime;
    }

    //called every frame after the Oppretunity calculations
    public override void AnimatronicUpdate()
    {
        if (NM.NightOngoing)
        {
            if (inRoom)
            {
                killtimer -= Time.deltaTime;

                if (killtimer <= 0)
                {
                    Jumpscare();
                }

                if (!NM.CamsFullyOpened && AvoidStage == 0)
                {
                    AvoidStage++;
                }
                else if (NM.CamsFullyOpened && AvoidStage == 1)
                {
                    AvoidStage = 0;
                    inRoom = false;
                    sammi.SetActive(false);
                }
            }
        }
    }

    //called every oppretunity
    public override void OnOppretunity()
    {
        if (!GM.silent && !NM.CamsFullyOpened) return;

        if (!inRoom)
        {
            inRoom = true;
            killtimer = killtime;
            sammi.SetActive(true);
            if (!NM.CamsFullyOpened)
            {
                NM.pulseBlackscreen();
            }
        }
    }

    //called when deathcoined
    public override void OnDeathcoined()
    {
        base.OnDeathcoined();
        inRoom = false;
        sammi.SetActive(false);
        AvoidStage = 0;
    }

    //called when someone kills the player
    public override void OnPlayerDied()
    {
        
    }

    public override void SetCustomValue(FloatValue value)
    {
        if (value.keyName == "killtime")
        {
            killtime = value.value;
        }

        base.SetCustomValue(value);
    }

    public override void OnMirrorSummon(int AIChosen)
    {
        base.OnMirrorSummon(AIChosen);

        killtimer = killtime;
    }
}
