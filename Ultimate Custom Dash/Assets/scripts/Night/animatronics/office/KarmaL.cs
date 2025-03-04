using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using UnityEngine;
using UnityEngine.UI;

public class KarmaL : AnimatronicBase
{
    bool canKill;
    bool awake;
    bool rollWakeChance;
    bool oneFlashLock;
    bool CamLockGuy;

    public SpriteRenderer karma;
    [ShowAssetPreview]
    public Sprite Awake;
    [ShowAssetPreview]
    public Sprite asleep;


    //called when animatronic gets his AILevel
    public override void AnimatronicStart()
    {
        if (AILevel == 0)
        {
            karma.gameObject.SetActive(false);
        }
        else
        {
            karma.gameObject.SetActive(true);
        }
    }

    //called every frame after the Oppretunity calculations
    public override void AnimatronicUpdate()
    {
        if (NM.NightOngoing)
        {
            if (AILevel != 0)
            {
                if (NM.CamsFullyOpened)
                {
                    CamLockGuy = false;
                    if (canKill)
                    {
                        Jumpscare();
                    }
                    if (!rollWakeChance)
                    {
                        rollWakeChance = true;
                        int randomChance = Random.Range(0, 20 + ExtraChance);
                        if (randomChance <= AILevel && AILevel != 0)
                        {
                            awake = true;
                            karma.sprite = Awake;
                        }
                    }
                }
                else
                {
                    rollWakeChance = false;
                    if (awake)
                    {
                        canKill = true;
                    }
                }
                

                if (awake)
                {
                    if (NM.FlashlightOn && !NM.lookingAtLeft() && !NM.CamsFullyOpened)
                    {
                        awake = false;
                        karma.sprite = asleep;
                        canKill = false;
                        oneFlashLock = true;
                        CamLockGuy = true;
                    }
                }
                else if (!oneFlashLock)
                {
                    if (NM.FlashlightOn && !NM.lookingAtLeft() && !CamLockGuy && !NM.CamsFullyOpened)
                    {
                        Jumpscare();
                    }

                }
                if (!NM.FlashlightOn)
                {
                    oneFlashLock = false;
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
        karma.gameObject.SetActive(false);
        awake = false;
    }

    //called when someone kills the player
    public override void OnPlayerDied()
    {
        
    }

    public override void OnMirrorSummon(int AIChosen)
    {
        base.OnMirrorSummon(AIChosen);

        karma.gameObject.SetActive(true);
    }
}
