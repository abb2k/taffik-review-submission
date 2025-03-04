using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PSSkull : AnimatronicBase
{
    public GameObject skullCont;

    public bool alreadyIn;

    public float insideTimer;
    bool killp;
    public float maskForTime;

    public float TimeInside = 4;
    public float maskTime = 0.1f;

    public override void AnimatronicGameStart()
    {
        if (GM != null) GM.soundManager.CreateIdleSource("PSMusic", GM.soundManager.GetSoundFromList("pressStart"));
        AddCustomValue(new FloatValue(TimeInside, "TimeInside"));
        AddCustomValue(new FloatValue(maskTime, "maskTime"));
    }

    //called when animatronic gets his AILevel
    public override void AnimatronicStart()
    {
        if (!GM.silent)
        {
            OppretunityEvery = 20;
            OppretunityTimer = OppretunityEvery;
        }
    }

    //called every frame after the Oppretunity calculations
    public override void AnimatronicUpdate()
    {
        if (NM.NightOngoing)
        {
            if (alreadyIn)
            {
                if (NM.InMask)
                {
                    if (maskForTime > 0)
                    {
                        maskForTime -= Time.deltaTime;
                    }
                    else
                    {
                        alreadyIn = false;
                        GM.soundManager.getActiveSource("PSMusic").Stop();
                        skullCont.SetActive(false);
                        NM.pulseBlackscreen();
                        NM.StartEffectsBlackscreen(false);
                    }
                }
                if (insideTimer > 0)
                {
                    insideTimer -= Time.deltaTime;
                }
                else
                {
                    if (!killp)
                    {
                        killp = true;
                        NM.Jumpscare(Animation, JumpscareScream, IsLethal, TakeDownCams, TakeDownMask, Voicelines, Name, null, 8, "PSMusic");
                    }
                }
            }
        }
    }

    //called every oppretunity
    public override void OnOppretunity()
    {
        if (!GM.silent && !NM.CamsFullyOpened) return;

        if (!alreadyIn)
        {
            alreadyIn = true;
            skullCont.SetActive(true);
            GM.soundManager.getActiveSource("PSMusic").Play();
            insideTimer = TimeInside;
            maskForTime = maskTime * AILevel;
            if (!NM.CamsFullyOpened)
            {
                NM.pulseBlackscreen();
            }
            NM.StartEffectsBlackscreen(true);
        }
    }

    //called when deathcoined
    public override void OnDeathcoined()
    {
        base.OnDeathcoined();
        alreadyIn = false;
        skullCont.SetActive(false);
        GM.soundManager.getActiveSource("PSMusic").Stop();
        NM.StartEffectsBlackscreen(false);
    }

    //called when someone kills the player
    public override void OnPlayerDied()
    {
        skullCont.SetActive(false);
    }

    public override void SetCustomValue(FloatValue value)
    {
        if (value.keyName == "TimeInside")
        {
            TimeInside = value.value;
        }
        if (value.keyName == "maskTime")
        {
            maskTime = value.value;
        }

        base.SetCustomValue(value);
    }

    public override void OnMirrorSummon(int AIChosen)
    {
        base.OnMirrorSummon(AIChosen);
    }
}
