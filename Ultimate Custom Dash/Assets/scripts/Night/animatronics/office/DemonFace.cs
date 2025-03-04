using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DemonFace : AnimatronicBase
{
    public GameObject demonFace;
    public GameObject demonFaceOffice;
    bool InVent;
    bool InRoom;
    bool oneTimeCamCheck;
    float RoomEnterTimer;
    float EscapeTimer;
    float RoomStayTimer;
    //called when animatronic gets his AILevel
    public override void AnimatronicStart()
    {
        RoomEnterTimer = 7;
        RoomStayTimer = 12;
    }

    //called every frame after the Oppretunity calculations
    public override void AnimatronicUpdate()
    {
        if (NM.NightOngoing)
        {
            if (NM.CamsFullyOpened)
            {
                if (!oneTimeCamCheck)
                {
                    oneTimeCamCheck = true;

                    int randomChance = Random.Range(0, 20 + ExtraChance);
                    if (randomChance <= AILevel && AILevel != 0)
                    {
                        if (!InVent && !InRoom && !NM.bottomRightDoorClosed)
                        {
                            InVent = true;
                            demonFace.SetActive(true);
                            RoomEnterTimer = 7;
                            EscapeTimer = 0.3f;
                            attackState = false;
                        }
                    }
                }
            }
            else
            {
                oneTimeCamCheck = false;
            }

            if (RoomEnterTimer > 0 && InVent)
            {
                RoomEnterTimer -= Time.deltaTime;

                if (RoomEnterTimer <= 0)
                {
                    attackState = true;
                }
            }

            if (NM.bottomRightDoorClosed && InVent)
            {
                if (EscapeTimer > 0)
                {
                    EscapeTimer -= Time.deltaTime;

                    if (EscapeTimer <= 0)
                    {
                        demonFace.SetActive(false);
                        InVent = false;
                        attackState = false;
                        Blocked();
                    }
                }
            }

            if (InRoom)
            {
                RoomStayTimer -= Time.deltaTime;

                if (RoomStayTimer <= 0)
                {
                    RoomStayTimer = 12;

                    InRoom = false;
                    demonFaceOffice.SetActive(false);
                    GameObject poof = PlayPoof();
                    poof.transform.position = demonFaceOffice.transform.position;
                    NM.flashlightDisabled(false);
                    GM.soundManager.CreateSoundEffect("demonLeft", GM.soundManager.GetSoundFromList("popSharp"));
                }
            }
        }
        
    }

    //called when deathcoined
    public override void OnDeathcoined()
    {
        base.OnDeathcoined();
        RoomEnterTimer = 7;
        RoomStayTimer = 12;
        if (InRoom)
        {
            InRoom = false;
            NM.flashlightDisabled(false);
        }
        InVent = false;
        demonFace.SetActive(false);
        demonFaceOffice.SetActive(false);
        attackState = false;
    }

    //called when someone kills the player
    public override void OnPlayerDied()
    {
        
    }

    public override void OnMirrorSummon(int AIChosen)
    {
        base.OnMirrorSummon(AIChosen);

        RoomEnterTimer = 7;
        RoomStayTimer = 12;
    }

    public override void OnAttackStateJumpscareCall()
    {
        InVent = false;
        InRoom = true;
        NM.flashlightDisabled(true);
        NM.pulseBlackscreen();
        demonFace.SetActive(false);
        demonFaceOffice.SetActive(true);
    }
}
