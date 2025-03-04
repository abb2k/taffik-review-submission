using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class zoink : AnimatronicBase
{
    public Transform spike;
    SpriteRenderer SpikeRenderer;

    public complexPosition[] positions;
    //called when animatronic gets his AILevel
    public override void AnimatronicStart()
    {
        SpikeRenderer = spike.GetComponent<SpriteRenderer>();

        if (SpikeRenderer.color.a <= 0)
        {
            int randomPos = Random.Range(0, positions.Length);

            spike = GameManager.setComplexPos(spike, positions[randomPos]);
        }

        if (AILevel == 0)
        {
            spike.gameObject.SetActive(false);
        }
        else
        {
            spike.gameObject.SetActive(true);
        }
    }

    //called every frame after the Oppretunity calculations
    public override void AnimatronicUpdate()
    {
        if (NM.NightOngoing)
        {
            if (SpikeRenderer == null) return;
            if (GameManager.DetectHoverOutside(spike.gameObject, false))
            {
                if (SpikeRenderer.color.a < 1 && !NM.CamsFullyOpened)
                {
                    SpikeRenderer.color += new Color(0, 0, 0, Time.deltaTime * AILevel * 20 / 1000);

                    if (SpikeRenderer.color.a >= 1)
                    {
                        Jumpscare();
                    }
                }
            }
            else
            {
                if (SpikeRenderer.color.a > 0)
                {
                    SpikeRenderer.color -= new Color(0, 0, 0, Time.deltaTime * 30 / 300);
                    if (SpikeRenderer.color.a < 0)
                    {
                        SpikeRenderer.color = GameManager.setColorAlpha(SpikeRenderer.color, 0);
                    }
                }
            }
        }
    }

    //called every oppretunity
    public override void OnOppretunity()
    {
        if (SpikeRenderer.color.a <= 0)
        {
            int randomPos = Random.Range(0, positions.Length);

            spike = GameManager.setComplexPos(spike, positions[randomPos]);
        }
    }

    //called when deathcoined
    public override void OnDeathcoined()
    {
        base.OnDeathcoined();
        spike.gameObject.SetActive(false);
    }

    //called when someone kills the player
    public override void OnPlayerDied()
    {
        spike.gameObject.SetActive(false);
    }

    public override void OnMirrorSummon(int AIChosen)
    {
        base.OnMirrorSummon(AIChosen);

        spike.gameObject.SetActive(true);
    }
}
