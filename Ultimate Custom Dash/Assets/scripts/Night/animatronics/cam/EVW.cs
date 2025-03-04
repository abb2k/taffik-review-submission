using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class EVW : AnimatronicBase
{
    bool killPlayer;
    float progress;
    bool pulse;

    public complexPosition[] positions;
    public Transform evw;

    //called when animatronic gets his AILevel
    public override void AnimatronicStart()
    {
        
    }

    //called every frame after the Oppretunity calculations
    public override void AnimatronicUpdate()
    {
        if (NM.NightOngoing && GM)
        {
            if (NM.CurrSpecialMode == NightManager.SpecialModes.GMB)
            {
                progress -= 7.5f * Time.deltaTime;
            }
            else
            {
                if (!GM.silent && NM.CamSys.CurrentCamera == cameraSystem.Cameras.Cam03) return;

                if (NM.NoiseLevel > 1)
                {
                    progress += ((NM.NoiseLevel - 1) * AILevel * Time.deltaTime) / 25;
                }

                if (NM.Temperature > 84)
                {
                    progress += ((NM.Temperature - 80) * AILevel * Time.deltaTime) / 140;
                }
            }

            if (progress < 0)
            {
                progress = 0;
            }
            if (progress > 0 && progress <= 25)
            {
                if (pulse)
                {
                    pulse = false;
                    if (NM.CamSys.CurrentCamera == cameraSystem.Cameras.Cam03)
                        NM.CamSys.PulseStatic();
                }
                evw.gameObject.SetActive(false);
            }
            else if (progress > 25 && progress <= 50)
            {
                if (!pulse)
                {
                    pulse = true;
                    if (NM.CamSys.CurrentCamera == cameraSystem.Cameras.Cam03)
                        NM.CamSys.PulseStatic();
                }
                evw.gameObject.SetActive(true);
                evw = GameManager.setComplexPos(evw, positions[0]);
            }
            else if (progress > 50 && progress <= 75)
            {
                if (pulse)
                {
                    pulse = false;
                    if (NM.CamSys.CurrentCamera == cameraSystem.Cameras.Cam03)
                        NM.CamSys.PulseStatic();
                }
                evw.gameObject.SetActive(true);
                evw = GameManager.setComplexPos(evw, positions[1]);
            }
            else if (progress > 75 && progress <= 100)
            {
                if (!pulse)
                {
                    pulse = true;
                    if (NM.CamSys.CurrentCamera == cameraSystem.Cameras.Cam03)
                        NM.CamSys.PulseStatic();
                }
                evw.gameObject.SetActive(true);
                evw = GameManager.setComplexPos(evw, positions[2]);
            }
            else if (progress > 100)
            {
                evw.gameObject.SetActive(false);
                if (!killPlayer)
                {
                    if (NM.CamSys.CurrentCamera == cameraSystem.Cameras.Cam03)
                        NM.CamSys.PulseStatic();
                    Jumpscare();
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
        progress = 0;
        evw.gameObject.SetActive(false);
        pulse = true;
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
