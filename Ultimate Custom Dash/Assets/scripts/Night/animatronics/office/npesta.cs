using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class npesta : AnimatronicBase
{
    public VideoClip vid;

    bool oneTimeCamCheck;

    bool inOffice;
    float jumpscareTime;
    bool killp;

    public GameObject icon;

    //called when animatronic gets his AILevel
    public override void AnimatronicStart()
    {
        
    }

    //called every frame after the Oppretunity calculations
    public override void AnimatronicUpdate()
    {
        if (NM.NightOngoing)
        {
            if (NM.CamsFullyOpened)
            {
                if (!oneTimeCamCheck)
                {
                    oneTimeCamCheck = true;

                    int randomChance = Random.Range(0, 20 + ExtraChance);
                    if (randomChance <= AILevel && AILevel != 0)
                    {
                        inOffice = true;
                        jumpscareTime = 6.5f - (0.2f * AILevel);
                        icon.SetActive(true);
                    }
                    else
                    {
                        inOffice = false;
                        icon.SetActive(false);
                    }
                }
            }
            else
            {
                oneTimeCamCheck = false;
            }
            if (NM.InMask)
            {
                inOffice = false;
                icon.SetActive(false);
            }
            if (inOffice && !NM.CamsFullyOpened)
            {
                if (jumpscareTime > 0)
                {
                    jumpscareTime -= Time.deltaTime;
                }
                else
                {
                    if (!killp)
                    {
                        killp = true;
                        NM.deathscreen.storytime = true;
                        NM.Jumpscare(vid, JumpscareScream, IsLethal, TakeDownCams, TakeDownMask, Voicelines, Name);
                        icon.SetActive(false);
                    }
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
        icon.SetActive(false);
        inOffice = false;
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
