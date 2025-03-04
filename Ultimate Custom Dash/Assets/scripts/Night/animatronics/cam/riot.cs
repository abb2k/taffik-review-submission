using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class riot : AnimatronicBase
{
    bool pulse = false;
    bool killp;

    public Image cameraThing;

    [ShowAssetPreview]
    public Sprite Cam06S0;
    [ShowAssetPreview]
    public Sprite Cam06S1;
    [ShowAssetPreview]
    public Sprite Cam06S2;
    [ShowAssetPreview]
    public Sprite Cam06S3;

    public complexPosition[] positions;

    public Transform _riot;

    float progression = 0;

    //called when animatronic gets his AILevel
    public override void AnimatronicStart()
    {
        
    }

    //called every frame after the Oppretunity calculations
    public override void AnimatronicUpdate()
    {
        if (NM.NightOngoing)
        {
            if (progression < 100)
            {
                if (!pulse)
                {
                    pulse = true;
                    if (NM.CamSys.CurrentCamera == cameraSystem.Cameras.Cam06)
                    {
                        NM.CamSys.PulseStatic();
                    }
                }
                cameraThing.sprite = Cam06S0;
                _riot.SetAsFirstSibling();
                _riot.gameObject.SetActive(false);
            }
            else if (progression >= 100 && progression < 200)
            {
                if (pulse)
                {
                    pulse = false;
                    if (NM.CamSys.CurrentCamera == cameraSystem.Cameras.Cam06)
                    {
                        NM.CamSys.PulseStatic();
                    }
                }
                cameraThing.sprite = Cam06S1;
                _riot.gameObject.SetActive(true);
                _riot.SetAsFirstSibling();
                _riot = GameManager.setComplexPos(_riot, positions[0]);
            }
            else if (progression >= 200 && progression < 300)
            {
                if (!pulse)
                {
                    pulse = true;
                    if (NM.CamSys.CurrentCamera == cameraSystem.Cameras.Cam06)
                    {
                        NM.CamSys.PulseStatic();
                    }
                }
                cameraThing.sprite = Cam06S2;
                _riot.gameObject.SetActive(true);
                _riot.SetAsFirstSibling();
                _riot = GameManager.setComplexPos(_riot, positions[1]);
            }
            else if (progression >= 300 && progression < 400)
            {
                if (pulse)
                {
                    pulse = false;
                    if (NM.CamSys.CurrentCamera == cameraSystem.Cameras.Cam06)
                    {
                        NM.CamSys.PulseStatic();
                    }
                }
                cameraThing.sprite = Cam06S3;
                _riot.gameObject.SetActive(true);
                _riot.SetAsLastSibling();
                _riot = GameManager.setComplexPos(_riot, positions[2]);
            }
            else if (progression >= 400)
            {
                if (!pulse)
                {
                    pulse = true;
                    if (NM.CamSys.CurrentCamera == cameraSystem.Cameras.Cam06)
                    {
                        NM.CamSys.PulseStatic();
                    }
                    Jumpscare();
                }
               _riot.gameObject.SetActive(false);
            }
        }
    }

    //called every oppretunity
    public override void OnOppretunity()
    {
        if (!GM.silent && NM.CamSys.CurrentCamera == cameraSystem.Cameras.Cam06) return;

        progression++;
    }

    //called when deathcoined
    public override void OnDeathcoined()
    {
        base.OnDeathcoined();
        _riot.gameObject.SetActive(false);
        progression = 0;
        pulse = false;
        cameraThing.sprite = Cam06S0;
    }

    //called when someone kills the player
    public override void OnPlayerDied()
    {
        _riot.gameObject.SetActive(false);
    }

    public virtual void OnCloseCurtains()
    {
        if (progression < 100)
        {
            GM.soundManager.CreateSoundEffect("blip", GM.soundManager.GetSoundFromList("blip"));
            if (AILevel != 0)
            {
                Jumpscare();
            }
        }
        else
        {
            progression = 0;
            pulse = false;
            GM.soundManager.CreateSoundEffect("blip", GM.soundManager.GetSoundFromList("blip"));
        }
    }

    public override void OnMirrorSummon(int AIChosen)
    {
        base.OnMirrorSummon(AIChosen);


    }
}
