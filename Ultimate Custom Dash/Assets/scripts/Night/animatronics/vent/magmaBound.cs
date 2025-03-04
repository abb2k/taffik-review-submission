using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class magmaBound : AnimatronicBase
{
    ventPath VentIcon;
    [BoxGroup("Settings")]
    [ShowAssetPreview]
    public Sprite Icon;
    [BoxGroup("Settings")]
    public float progressAmount;

    float waitTimer;
    [BoxGroup("Settings")]
    public float waitTime;
    bool inVent;

    float avoidTimer;
    [BoxGroup("Settings")]
    public float avoidTime;

    [BoxGroup("Settings")]
    public SpriteRenderer Spaceicon;
    bool makeVisi;
    [BoxGroup("Settings")]
    public float fadingSpeed;

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
            VentIcon.SetSprite(Icon);

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
                        Blocked();
                        makeVisi = false;
                        attackReady = false;
                        VentIcon.resetPosition();
                        attackState = false;
                    }
                }

                if (waitTimer <= 0 && inVent)
                {
                    attackReady = true;
                    if (!NM.topDoorClosed)
                        attackState = true;
                }
            }

            if (makeVisi)
            {
                Spaceicon.color = Color.Lerp(Spaceicon.color, Color.white, Time.deltaTime * fadingSpeed);
            }
            else
            {
                Spaceicon.color = new Color(1, 1, 1, 0);
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
            makeVisi = true;
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

    public override void OnPlayerDied()
    {
        Spaceicon.gameObject.SetActive(false);
        attackReady = false;
    }

    public override void OnDeathcoined()
    {
        base.OnDeathcoined();

        VentIcon.gameObject.SetActive(false);
        progressAmount = 0;
        Spaceicon.color = new Color(1, 1, 1, 0);
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
