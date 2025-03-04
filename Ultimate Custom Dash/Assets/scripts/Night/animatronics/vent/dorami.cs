using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class dorami : AnimatronicBase
{
    ventPath VentIcon;
    [BoxGroup("Settings")]
    [ShowAssetPreview]
    public Sprite Icon;
    [BoxGroup("Settings")]
    public float progressAmount;

    Coroutine c;

    public override void AnimatronicStart()
    {
        GM = GameManager.get();
        NM = NightManager.inctance;
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

        if (VentIcon.isAtEnd() && c == null)
        {
            c = StartCoroutine(waitToKill());
            IEnumerator waitToKill()
            {
                float waitTimer = 5;
                while (true)
                {
                    if (NM.topDoorClosed)
                    {
                        Blocked();
                        VentIcon.resetPosition();
                        c = null;
                        break;
                    }
                    else
                    {
                        waitTimer -= Time.deltaTime;

                        if (waitTimer <= 0)
                        {
                            //Jumpscare();
                            attackState = true;
                            c = null;
                            break;
                        }
                    }

                    yield return null;
                }
            }

        }
    }

    public override void SetCustomValue(FloatValue value)
    {
        if (value.keyName == "progressAmount")
        {
            progressAmount = value.value;
        }

        base.SetCustomValue(value);
    }

    public override void OnDeathcoined()
    {
        base.OnDeathcoined();

        VentIcon.gameObject.SetActive(false);
        progressAmount = 0;
        attackState = false;
    }

    public override void OnMirrorSummon(int AIChosen)
    {
        base.OnMirrorSummon(AIChosen);

        VentIcon.gameObject.SetActive(true);
    }

    public override void OnAttackStateJumpscareCall()
    {
        if (!NM.topDoorClosed)
            base.OnAttackStateJumpscareCall();
    }
}
