using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class wulzy : AnimatronicBase
{
    bool onRight;

    bool Attaking;

    float avoidTimer;

    public float avoidTime = 0.5f;

    float killTiemr;
    public float killTime;
    public float killTimeNormal;

    public override void AnimatronicGameStart()
    {
        if (!GM) return;

        GM.soundManager.CreateIdleSource("wulzyWoah", GM.soundManager.GetSoundFromList("woah"));
        avoidTimer = avoidTime;
        if (!GM.silent)
            killTime = killTimeNormal;
        killTiemr = killTime;

        AddCustomValue(new FloatValue(avoidTime, "avoidTime"));
        AddCustomValue(new FloatValue(killTime, "killTime"));
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
            if (Attaking)
            {
                if (onRight)
                {
                    if (NM.rightDoorClosed)
                    {
                        avoidTimer -= Time.deltaTime;
                    }
                }
                else
                {
                    if (NM.leftDoorClosed)
                    {
                        avoidTimer -= Time.deltaTime;
                    }
                }

                if (avoidTimer < 0)
                {
                    Attaking = false;
                    Blocked();
                    killTiemr = killTime;
                }

                if (killTiemr > 0)
                {
                    killTiemr -= Time.deltaTime;
                }
                else
                {
                    killTiemr = 9999;
                    Jumpscare();
                }
            }
        }
    }

    //called every oppretunity
    public override void OnOppretunity()
    {
        if (!Attaking)
        {
            Attaking = true;
            avoidTimer = avoidTime;

            int randomnum = Random.Range(0,2);

            if (randomnum == 0)
            {
                onRight = true;
                GM.soundManager.getActiveSource("wulzyWoah").panStereo = 1;
                GM.soundManager.getActiveSource("wulzyWoah").Play();
            }
            else
            {
                onRight = false;
                GM.soundManager.getActiveSource("wulzyWoah").panStereo = -1;
                GM.soundManager.getActiveSource("wulzyWoah").Play();
            }
        }
    }

    //called when deathcoined
    public override void OnDeathcoined()
    {
        base.OnDeathcoined();
        Attaking = false;
    }

    //called when someone kills the player
    public override void OnPlayerDied()
    {
        
    }

    public override void SetCustomValue(FloatValue value)
    {
        if (value.keyName == "avoidTime")
        {
            avoidTime = value.value;
        }
        if (value.keyName == "killTime")
        {
            killTime = value.value;
        }

        base.SetCustomValue(value);
    }

    public override void OnMirrorSummon(int AIChosen)
    {
        base.OnMirrorSummon(AIChosen);

        avoidTimer = avoidTime;
    }
}
