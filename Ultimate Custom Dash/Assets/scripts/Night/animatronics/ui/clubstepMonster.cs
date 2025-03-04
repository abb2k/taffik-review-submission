using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class clubstepMonster : AnimatronicBase
{
    bool going;
    [BoxGroup("Settings")]
    public Transform top;
    [BoxGroup("Settings")]
    public Transform bottom;
    [BoxGroup("Settings")]
    public float progressLevel;
    [BoxGroup("Settings")]
    public float oppEveryNormal;
    //called when animatronic gets his AILevel
    public override void AnimatronicStart()
    {
        if (!GM.silent)
        {
            OppretunityEvery = oppEveryNormal;
        }
    }

    //called every frame after the Oppretunity calculations
    public override void AnimatronicUpdate()
    {
        if (!NM.NightOngoing) return;

        if (going)
        {
            if (NM.CurrSpecialMode == NightManager.SpecialModes.PowerAC)
            {
                progressLevel -= 1200 * Time.deltaTime;

                if (progressLevel < 0)
                {
                    progressLevel = 0;
                    going = false;
                }
            }
            else
            {
                progressLevel += 300 * Time.deltaTime;

                if (progressLevel >= 2965)
                {
                    Jumpscare();
                    going = false;
                }
            }
        }

        var tPos = top.localPosition;
        tPos.y = 4256 - progressLevel;
        top.localPosition = tPos;

        var bPos = bottom.localPosition;
        bPos.y = -5477 + progressLevel; 
        bottom.localPosition = bPos;
    }

    //called every oppretunity
    public override void OnOppretunity()
    {
        if (!NM.isDead)
            going = true;
    }

    //called when deathcoined
    public override void OnDeathcoined()
    {
        base.OnDeathcoined();
        progressLevel = 0;
        var tPos = top.localPosition;
        tPos.y = 4256 - progressLevel;
        top.localPosition = tPos;

        var bPos = bottom.localPosition;
        bPos.y = -5477 + progressLevel;
        bottom.localPosition = bPos;
        going = false;
    }

    //called when someone kills the player
    public override void OnPlayerDied()
    {
        bottom.gameObject.SetActive(false);
        top.gameObject.SetActive(false);
    }

    public override void OnMirrorSummon(int AIChosen)
    {
        base.OnMirrorSummon(AIChosen);
    }
}
