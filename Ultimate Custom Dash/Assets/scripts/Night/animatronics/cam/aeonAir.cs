using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class aeonAir : AnimatronicBase
{
    float progress;

    public complexPosition[] positions;

    public Transform aeon;

    bool pulse;

    float avoidTimer = 1.5f;

    //called when animatronic gets his AILevel
    public override void AnimatronicStart()
    {
        avoidTimer = 1.5f;
    }

    //called every frame after the Oppretunity calculations
    public override void AnimatronicUpdate()
    {
        if (NM.NightOngoing)
        {
            if (progress >= 0 && progress <= 100)
            {
                if (pulse)
                {
                    pulse = false;
                    if (NM.CamSys.CurrentCamera == cameraSystem.Cameras.Cam01)
                        NM.CamSys.PulseStatic();
                }
                aeon.gameObject.SetActive(false);
            }
            else if (progress > 100 && progress <= 200)
            {
                if (!pulse)
                {
                    pulse = true;
                    if (NM.CamSys.CurrentCamera == cameraSystem.Cameras.Cam01)
                        NM.CamSys.PulseStatic();
                }
                aeon.gameObject.SetActive(true);
                aeon = GameManager.setComplexPos(aeon, positions[0]);
            }
            else if (progress > 200 && progress <= 300)
            {
                if (pulse)
                {
                    pulse = false;
                    if (NM.CamSys.CurrentCamera == cameraSystem.Cameras.Cam01)
                        NM.CamSys.PulseStatic();
                }
                aeon.gameObject.SetActive(true);
                aeon = GameManager.setComplexPos(aeon, positions[1]);
            }
            else if (progress > 300 && progress <= 400)
            {
                if (!pulse)
                {
                    pulse = true;
                    if (NM.CamSys.CurrentCamera == cameraSystem.Cameras.Cam01)
                        NM.CamSys.PulseStatic();
                }
                aeon.gameObject.SetActive(true);
                aeon = GameManager.setComplexPos(aeon, positions[2]);
            }
            else if (progress > 400 && progress <= 500)
            {
                if (pulse)
                {
                    pulse = false;
                    if (NM.CamSys.CurrentCamera == cameraSystem.Cameras.Cam01)
                        NM.CamSys.PulseStatic();
                }
                aeon.gameObject.SetActive(true);
                aeon = GameManager.setComplexPos(aeon, positions[3]);

                if (NM.leftDoorClosed)
                {
                    if (avoidTimer > 0)
                    {
                        avoidTimer -= Time.deltaTime;
                    }

                    if (avoidTimer <= 0)
                    {
                        progress = 0;
                        Blocked();
                        avoidTimer = 1.5f;
                        pulse = true;
                        if (NM.CamSys.CurrentCamera == cameraSystem.Cameras.Cam01)
                            NM.CamSys.PulseStatic();
                    }
                }
            }
            else if (progress > 500)
            {
                if (!pulse)
                {
                    pulse = true;
                    if (NM.CamSys.CurrentCamera == cameraSystem.Cameras.Cam01)
                        NM.CamSys.PulseStatic();
                }
                aeon.gameObject.SetActive(false);
                if (NM.leftDoorClosed)
                {
                    progress = 0;
                    Blocked();
                    attackState = false;
                    avoidTimer = 1.5f;
                }
                else
                {
                    progress = 0;
                    attackState = true;
                }
            }
        }
    }

    //called every oppretunity
    public override void OnOppretunity()
    {
        if (NM.Temperature < 79)
        {
            progress += 0.5f;
        }
        else
        {
            progress += 0.5f * ((NM.Temperature - 79) / 10);
        }
    }

    //called when deathcoined
    public override void OnDeathcoined()
    {
        base.OnDeathcoined();
        progress = 0;
        pulse = false;
        aeon.gameObject.SetActive(false);
        if (NM.CamSys.CurrentCamera == cameraSystem.Cameras.Cam01)
            NM.CamSys.PulseStatic();
        attackState = false;
    }

    //called when someone kills the player
    public override void OnPlayerDied()
    {
        
    }

    public override void OnMirrorSummon(int AIChosen)
    {
        base.OnMirrorSummon(AIChosen);
    }

    public override GameObject Jumpscare()
    {
        return NM.Jumpscare(Animation, JumpscareScream, IsLethal, TakeDownCams, TakeDownMask, Voicelines, Name, null, 10);
    }
}
