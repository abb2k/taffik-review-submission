using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class shsnowman : AnimatronicBase
{
    cameraSystem.Cameras prevcamera;

    bool canAppear;
    float looktimer;

    bool rollForCloseCams;

    bool jumps;

    public Transform snowman;

    public complexPosition[] positions;

    //called when animatronic gets his AILevel
    public override void AnimatronicStart()
    {
        NM.OnCamSysSwitch += onSystemSwitch;
    }

    //called every frame after the Oppretunity calculations
    public override void AnimatronicUpdate()
    {
        if (NM.NightOngoing)
        {
            if (prevcamera != NM.CamSys.CurrentCamera)
            {
                prevcamera = NM.CamSys.CurrentCamera;
                if (!canAppear)
                {
                    int randomChance = Random.Range(0, 20 + ExtraChance);
                    if (randomChance <= AILevel && AILevel != 0)
                    {
                        canAppear = true;
                        snowman.gameObject.SetActive(true);

                        looktimer = 3.5f - (0.1f * AILevel);

                        int randomPos = Random.Range(0, 3);

                        snowman = GameManager.setComplexPos(snowman, positions[randomPos]);
                    }
                }
            }
            if (!NM.CamsFullyOpened)
            {
                if (!rollForCloseCams)
                {
                    int randomChance = Random.Range(0, 20 + ExtraChance);
                    if (randomChance <= AILevel && AILevel != 0)
                    {
                        canAppear = true;
                        snowman.gameObject.SetActive(true);

                        looktimer = 3.5f - (0.1f * AILevel);

                        int randomPos = Random.Range(0, 3);

                        snowman = GameManager.setComplexPos(snowman, positions[randomPos]);
                    }
                    else
                    {
                        canAppear = false;
                        snowman.gameObject.SetActive(false);
                    }
                    rollForCloseCams = true;
                }
                jumps = false;
            }
            else
            {
                if (canAppear)
                {
                    if (looktimer > 0)
                    {
                        looktimer -= Time.deltaTime;
                    }
                    else
                    {
                        if (!jumps)
                        {
                            jumps = true;
                            Jumpscare();
                        }
                    }
                }
                rollForCloseCams = false;
            }
        }
        else
        {
            canAppear = false;
            snowman.gameObject.SetActive(false);
        }
    }

    void onSystemSwitch(NightManager.camSystems sys)
    {
        canAppear = false;
        snowman.gameObject.SetActive(false);
    }

    //called every oppretunity
    public override void OnOppretunity()
    {
        
    }

    //called when deathcoined
    public override void OnDeathcoined()
    {
        base.OnDeathcoined();
        canAppear = false;
        snowman.gameObject.SetActive(false);
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
