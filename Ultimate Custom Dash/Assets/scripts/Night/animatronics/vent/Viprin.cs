using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Viprin : AnimatronicBase
{
    ventPath VentIcon;
    [BoxGroup("Settings")]
    [ShowAssetPreview]
    public Sprite Icon;
    [BoxGroup("Settings")]
    public float progressAmount;
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
            VentIcon.transform.localScale = Vector3.one * 0.45f;

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

    public override void OnOppretunity()
    {
        float progressAmountReal = progressAmount;
        if (NM.CurrSpecialMode == NightManager.SpecialModes.SilentVent)
        {
            progressAmountReal = progressAmountReal / 1.5f;
        }

        if (VentIcon.isTouchingAVentSnare(false))
        {
            progressAmountReal = progressAmountReal / 3;
        }

        VentIcon.AddProgress(progressAmountReal);

        if (VentIcon.isTouchingAVentSnare(true))
        {
            Blocked();
            VentIcon.resetPosition();
        }

        if (VentIcon.isAtEnd() && !NM.topDoorClosed)
        {
            attackState = true;
        }
    }

    void doorChanged(NightManager.doors door, bool isClosed)
    {
        if (door != NightManager.doors.top || !VentIcon.isAtEnd()) return;

        if (isClosed)
            attackState = false;
        else
            attackState = true;
    }

    public override void OnDeathcoined()
    {
        base.OnDeathcoined();

        VentIcon.gameObject.SetActive(false);
        progressAmount = 0;
        attackState = false;
    }

    public override void SetCustomValue(FloatValue value)
    {
        if (value.keyName == "progressAmount")
        {
            progressAmount = value.value;
        }

        base.SetCustomValue(value);
    }

    public override void OnMirrorSummon(int AIChosen)
    {
        base.OnMirrorSummon(AIChosen);

        VentIcon.gameObject.SetActive(true);
    }
}
