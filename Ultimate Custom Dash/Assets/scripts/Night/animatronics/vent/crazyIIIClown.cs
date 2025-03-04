using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class crazyIIIClown : AnimatronicBase
{
    ventPath VentIcon;
    [BoxGroup("Settings")]
    public float progressAmount;

    float waitTimer;
    [BoxGroup("Settings")]
    public float waitTime;
    bool inVent;

    float avoidTimer;
    [BoxGroup("Settings")]
    public float avoidTime;

    bool attackReady;

    public override void AnimatronicGameStart()
    {
        AddCustomValue(new FloatValue(waitTime, "waitTime"));
        AddCustomValue(new FloatValue(avoidTime, "avoidTime"));
    }
    public override void AnimatronicStart()
    {
        GM = GameManager.get();
        NM = NightManager.inctance;
        NM.OnDoorStateChanged += doorChanged;
        if (VentIcon == null)
        {
            VentIcon = CreateVentPathObject();
            AddCustomValue(new FloatValue(progressAmount, "progressAmount"));
        }
        if (VentIcon != null)
        {
            VentIcon.SetColor(new Color(0,0,0,0));

            if (AILevel == 0)
            {
                VentIcon.gameObject.SetActive(false);
            }
            else
            {
                VentIcon.gameObject.SetActive(true);
            }
        }

    }

    public override void AnimatronicUpdate()
    {
        if (NM.NightOngoing)
        {
            if (inVent)
            {
                waitTimer -= Time.deltaTime;

                if (NM.topDoorClosed)
                {
                    avoidTimer -= Time.deltaTime;

                    if (avoidTimer <= 0)
                    {
                        inVent = false;
                        attackReady = false;
                        Blocked();
                        VentIcon.resetPosition();
                    }
                }

                if (waitTimer <= 0 && inVent)
                {
                    attackReady = true;
                    if (!NM.topDoorClosed)
                        attackState = true;
                }
            }
        }
    }

    void doorChanged(NightManager.doors door, bool isClosed)
    {
        if (door != NightManager.doors.top || !attackReady) return;

        if (isClosed)
            attackState = false;
        else
            attackState = true;
    }

    public override void OnOppretunity()
    {
        float progressAmountReal = progressAmount;
        if (NM.CurrSpecialMode == NightManager.SpecialModes.SilentVent)
        {
            progressAmountReal = progressAmountReal / 1.5f;
        }
        else if (NM.CurrSpecialMode == NightManager.SpecialModes.PowerAC)
        {
            progressAmountReal = progressAmountReal * 1.5f;
        }

        if (VentIcon.isTouchingAVentSnare(false))
        {
            progressAmountReal = progressAmountReal / 3;
        }

        VentIcon.AddProgress(progressAmountReal);

        if (VentIcon.isAtEnd() && !inVent)
        {
            waitTimer = waitTime;
            inVent = true;
            avoidTimer = avoidTime;
            GM.soundManager.CreateSoundEffect("clownAppear", GM.soundManager.GetSoundFromList("clownAppear"));
        }
    }
    
    public override void SetCustomValue(FloatValue value)
    {
        if (value.keyName == "progressAmount")
        {
            progressAmount = value.value;
        }
        if (value.keyName == "waitTime")
        {
            waitTime = value.value;
        }
        if (value.keyName == "avoidTime")
        {
            avoidTime = value.value;
        }

        base.SetCustomValue(value);
    }

    public override void OnDeathcoined()
    {
        base.OnDeathcoined();

        VentIcon.gameObject.SetActive(false);
        progressAmount = 0;
        inVent = false;
        attackState = false;
        attackReady = false;
    }

    public override void OnMirrorSummon(int AIChosen)
    {
        base.OnMirrorSummon(AIChosen);

        VentIcon.gameObject.SetActive(true);
    }
}
