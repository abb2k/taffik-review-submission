using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class grandpaDemon : AnimatronicBase
{
    public float pauseTime;
    public bool Onleft;

    public GameObject faceLeft;
    public GameObject faceRight;

    //called when animatronic gets his AILevel
    public override void AnimatronicStart()
    {
        NM = NightManager.inctance;
        pauseTime = 20.5f / (AILevel / 20.5f);

        int randomside = Random.Range(0, 2);

        if (randomside == 0)
        {
            move(true);
        }
        else if (randomside == 1)
        {
            move(false);
        }

        if (AILevel == 0)
        {
            faceRight.SetActive(false);
            faceLeft.SetActive(false);
        }
    }

    //called every frame after the Oppretunity calculations
    public override void AnimatronicUpdate()
    {
        if (NM.NightOngoing)
        {
            if (pauseTime > 0)
            {
                pauseTime -= Time.deltaTime;
            }

            if (pauseTime <= 0)
            {
                if (Onleft)
                {
                    if (!NM.signOnRight || !GM.silent && NM.leftDoorClosed)
                    {
                        block();
                    }
                    else
                    {
                        if (!GM.silent && NM.CamSys.CurrentCamera == cameraSystem.Cameras.Cam01) return;
                        attackState = true;
                    }
                }
                // on right
                else
                {
                    if (NM.signOnRight || !GM.silent && NM.rightDoorClosed)
                    {
                        block();
                    }
                    else
                    {
                        if (!GM.silent && NM.CamSys.CurrentCamera == cameraSystem.Cameras.Cam02) return;
                        attackState = true;
                    }
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
        faceRight.SetActive(false);
        faceLeft.SetActive(false);
        attackState = false;
    }

    //called when someone kills the player
    public override void OnPlayerDied()
    {
        
    }

    void block()
    {
        int randomside = Random.Range(0, 2);

        pauseTime = 20.5f / (AILevel / 20.5f);
        attackState = false;


        if (randomside == 0)
        {
            if (!Onleft)
            {
                move(true);
            }
        }
        else if (randomside == 1)
        {
            if (Onleft)
            {
                move(false);
            }
        }
    }

    void move(bool toLeft)
    {
        if (NM.CamSys.CurrentCamera == cameraSystem.Cameras.Cam01 || NM.CamSys.CurrentCamera == cameraSystem.Cameras.Cam02)
        {
            NM.CamSys.PulseStatic();
        }
        if (toLeft)
        {
            Onleft = true;
            faceRight.SetActive(false);
            faceLeft.SetActive(true);
        }
        else
        {
            Onleft = false;
            faceRight.SetActive(true);
            faceLeft.SetActive(false);
        }
    }

    public override void OnMirrorSummon(int AIChosen)
    {
        base.OnMirrorSummon(AIChosen);

        pauseTime = 20.5f / (AILevel / 20.5f);

        int randomside = Random.Range(0, 2);

        if (randomside == 0)
        {
            move(true);
        }
        else if (randomside == 1)
        {
            move(false);
        }
    }
}
