using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class loochi : AnimatronicBase
{
    cameraSystem.Cameras prevcamera;

    bool canAppear;
    float looktimer;

    bool rollForCloseCams;

    bool jumps;

    bool inJumpscare;

    public Transform looch;

    GameObject scare;
    bool lockScareReset = true;

    //called when animatronic gets his AILevel
    public override void AnimatronicStart()
    {
        NM.CamSys.onCameraChanged += onCamChanged;
    }

    //called every frame after the Oppretunity calculations
    public override void AnimatronicUpdate()
    {
        if (!inJumpscare && NM.NightOngoing)
        {
            if (prevcamera != NM.CamSys.CurrentCamera)
            {
                prevcamera = NM.CamSys.CurrentCamera;
                if (canAppear)
                {
                    canAppear = false;
                    looch.gameObject.SetActive(false);
                }
                else
                {
                    int randomChance = Random.Range(0, 20 + ExtraChance);
                    if (randomChance <= AILevel && AILevel != 0)
                    {
                        canAppear = true;
                        looch.gameObject.SetActive(true);

                        looktimer = 3 - (0.1f * AILevel);

                        int randomPos = Random.Range(0, 3);

                        if (randomPos == 0)
                        {
                            looch.localPosition = new Vector3(-1096.1f, -115, 0);
                            looch.eulerAngles = new Vector3(0,0, -20.41f);
                        }
                        else if (randomPos == 1)
                        {
                            looch.localPosition = new Vector3(-162.6f, 837.8f, 0);
                            looch.eulerAngles = new Vector3(0, 0, 157.7f);
                        }
                        else if (randomPos == 2)
                        {
                            looch.localPosition = new Vector3(1173.1f, -380, 0);
                            looch.eulerAngles = new Vector3(0, 0, 80.5f);
                        }
                    }
                }
            }
            if (!NM.CamsFullyOpened && !inJumpscare)
            {
                if (!rollForCloseCams)
                {
                    int randomChance = Random.Range(0, 20 + ExtraChance);
                    if (randomChance <= AILevel && AILevel != 0)
                    {
                        canAppear = true;
                        looch.gameObject.SetActive(true);

                        looktimer = 3 - (0.1f * AILevel);

                        int randomPos = Random.Range(0, 3);

                        if (randomPos == 0)
                        {
                            looch.localPosition = new Vector3(-1096.1f, -115, 0);
                            looch.eulerAngles = new Vector3(0, 0, -20.41f);
                        }
                        else if (randomPos == 1)
                        {
                            looch.localPosition = new Vector3(-162.6f, 837.8f, 0);
                            looch.eulerAngles = new Vector3(0, 0, 157.7f);
                        }
                        else if (randomPos == 2)
                        {
                            looch.localPosition = new Vector3(1173.1f, -380, 0);
                            looch.eulerAngles = new Vector3(0, 0, 80.5f);
                        }
                    }
                    else
                    {
                        canAppear = false;
                        looch.gameObject.SetActive(false);
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
                            scare = NM.Jumpscare(Animation, JumpscareScream, IsLethal, TakeDownCams, TakeDownMask, Voicelines, Name, null, 8, "", 0.2f);
                            inJumpscare = true;

                            scare.transform.localScale = Vector3.one * 4;
                            scare.transform.localPosition = new Vector3(-536.4f, -137, 0);

                            NM.addToNoise(1);
                        }
                    }
                }
                rollForCloseCams = false;
            }
            if (NM.currSys != NightManager.camSystems.cameraSys)
            {
                canAppear = false;
                looch.gameObject.SetActive(false);
            }
        }
        else
        {
            canAppear = false;
            looch.gameObject.SetActive(false);
        }

        if (scare == null)
        {
            if (!lockScareReset)
            {
                lockScareReset = true;
                FinishedJumpscare();
            }
        }
        else
        {
            lockScareReset = false;
        }
    }

    void onCamChanged(cameraSystem.Cameras camera)
    {
        if (camera != NM.CamSys.CurrentCamera) return;
        canAppear = false;
        looch.gameObject.SetActive(false);
    }

    //called every oppretunity
    public override void OnOppretunity()
    {
        
    }

    //called when deathcoined
    public override void OnDeathcoined()
    {
        base.OnDeathcoined();
        inJumpscare = false;
        canAppear = false;
        looch.gameObject.SetActive(false);
    }

    //called when someone kills the player
    public override void OnPlayerDied()
    {
        
    }

    void FinishedJumpscare()
    {
        inJumpscare = false;
        jumps = false;
        NM.addToNoise(-1);
        rollForCloseCams = false;
        if (NM.CamsFullyOpened)
        {
            canAppear = false;
        }
        prevcamera = NM.CamSys.CurrentCamera;
    }

    public override void OnMirrorSummon(int AIChosen)
    {
        base.OnMirrorSummon(AIChosen);
    }
}
