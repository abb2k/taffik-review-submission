using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class bloodlust : AnimatronicBase
{
    ductPath ductIcon;
    [BoxGroup("Settings")]
    [ShowAssetPreview]
    public Sprite icon;
    [BoxGroup("Settings")]
    public float pushbackTimer;
    public override void AnimatronicStart()
    {
        if (ductIcon == null) ductIcon = CreateDuctPathObject();
        ductIcon.transform.localScale = Vector3.one * 0.5f;
        if (ductIcon != null)
        {
            ductIcon.SetSprite(icon);
            if (AILevel == 0)
            {
                ductIcon.gameObject.SetActive(false);
            }
            else
            {
                ductIcon.gameObject.SetActive(true);
            }
        }
    }

    public override void AnimatronicUpdate()
    {
        if (NM != null && AILevel != 0)
            
        if (NM != null && AILevel != 0)
            if (NM.CurrSpecialMode == NightManager.SpecialModes.Heater)
            {
                if (pushbackTimer > 0)
                {
                    pushbackTimer -= Time.deltaTime;

                    if (pushbackTimer <= 0)
                    {
                        ductIcon.pushBack();
                        attackState = false;
                        pushbackTimer = Random.Range(0.5f, 1.5f);
                    }
                }
            }
            else
            {
                pushbackTimer = Random.Range(0.5f, 1.5f);
            }
    }

    public override void OnDeathcoined()
    {
        base.OnDeathcoined();

        ductIcon.gameObject.SetActive(false);
        ductIcon.resetPositioin();
        attackState = false;
    }

    public override void OnOppretunity()
    {
        if (ductIcon.IsDuctNotClosedOnMe())
        {
            attackState = true;
        }
        if (ductIcon.IsAtEnd() && !ductIcon.IsDuctNotClosedOnMe())
        {
            ductIcon.pushBack(false);
            attackState = false;
        }
        else
        {
            int random = Random.Range(0, 3);
            if (random == 0)
            {
                ductIcon.Move(false);
            }
            else
            {
                ductIcon.Move(true);
            }
            if (!ductIcon.IsAtEnd())
                attackState = false;
        }
    }

    public override void OnMirrorSummon(int AIChosen)
    {
        base.OnMirrorSummon(AIChosen);

        ductIcon.gameObject.SetActive(true);
    }
}
