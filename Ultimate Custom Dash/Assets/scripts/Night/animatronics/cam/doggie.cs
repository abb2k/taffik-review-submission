using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class doggie : AnimatronicBase
{
    public complexPosition[] positions;

    public float progression;
    public bool pulse;
    public float doorWaitTime;
    private float reachDoorTimer;
    public float reachDoorTimeSilent;
    public float reachDoorTimeNormal;
    public bool knock;
    public bool killp;

    public Image mycamera;
    [ShowAssetPreview]
    public Sprite St0;
    [ShowAssetPreview]
    public Sprite St1;
    [ShowAssetPreview]
    public Sprite St2;
    public Transform Doggie;

    public override void AnimatronicStart()
    {
        reachDoorTimer = GM.silent ? reachDoorTimeSilent : reachDoorTimeNormal;
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
                    if (NM.CamSys.CurrentCamera == cameraSystem.Cameras.Cam05)
                    {
                        NM.CamSys.PulseStatic();
                    }
                }
                mycamera.sprite = St0;
                Doggie.gameObject.SetActive(false);
            }
            else if (progression >= 100 && progression < 200)
            {
                if (pulse)
                {
                    pulse = false;
                    if (NM.CamSys.CurrentCamera == cameraSystem.Cameras.Cam05)
                    {
                        NM.CamSys.PulseStatic();
                    }
                }
                mycamera.sprite = St1;
                Doggie.gameObject.SetActive(true);
                Doggie.SetAsFirstSibling();

                Doggie = GameManager.setComplexPos(Doggie, positions[0]);
            }
            else if (progression >= 200 && progression < 300)
            {
                if (!pulse)
                {
                    pulse = true;
                    if (NM.CamSys.CurrentCamera == cameraSystem.Cameras.Cam05)
                    {
                        NM.CamSys.PulseStatic();
                    }
                }
                mycamera.sprite = St2;
                Doggie.gameObject.SetActive(true);
                Doggie.SetAsFirstSibling();

                Doggie = GameManager.setComplexPos(Doggie, positions[1]);
            }
            else if (progression >= 300 && progression < 400)
            {
                if (pulse)
                {
                    pulse = false;
                    if (NM.CamSys.CurrentCamera == cameraSystem.Cameras.Cam05)
                    {
                        NM.CamSys.PulseStatic();
                    }
                }

                mycamera.sprite = St2;
                Doggie.gameObject.SetActive(true);
                Doggie.SetAsLastSibling();

                Doggie = GameManager.setComplexPos(Doggie, positions[2]);
            }
            else if (progression >= 400)
            {
                if (!pulse)
                {
                    pulse = true;
                    if (NM.CamSys.CurrentCamera == cameraSystem.Cameras.Cam05)
                    {
                        NM.CamSys.PulseStatic();
                    }
                    GM.soundManager.CreateSoundEffect("running", GM.soundManager.GetSoundFromList("running")).pitch = 1 / reachDoorTimer;
                }
                mycamera.sprite = St2;
                Doggie.gameObject.SetActive(false);
                if (reachDoorTimer > 0)
                {
                    reachDoorTimer -= Time.deltaTime;
                }
                else
                {

                    if (!knock)
                    {
                        knock = true;
                        GM.soundManager.CreateSoundEffect("doggieKnock", GM.soundManager.GetSoundFromList("doorPound"));
                    }

                    if (doorWaitTime > 0)
                    {
                        doorWaitTime -= Time.deltaTime;
                        if (!NM.leftDoorClosed)
                        {
                            if (!killp)
                            {
                                killp = true;
                                if (GM.soundManager.getActiveSource("doggieKnock", out NamedAudioSource gotten) != null)
                                {
                                    GM.soundManager.DeleteSource(gotten);
                                }
                                Jumpscare();
                            }
                        }

                        if (!GM.silent)
                            doorWaitTime = 0;
                    }
                    else
                    {
                        progression = 0;
                        doorWaitTime = 3;
                        pulse = false;
                        if (GM.soundManager.getActiveSource("doggieKnock", out NamedAudioSource gotten) != null)
                        {
                            GM.soundManager.DeleteSource(gotten);
                        }
                        knock = false;

                        reachDoorTimer = GM.silent ? reachDoorTimeSilent : reachDoorTimeNormal;
                        Blocked();
                    }
                }
            }
        }
    }

    //called every oppretunity
    public override void OnOppretunity()
    {
        if (NM.CamSys.CurrentCamera != cameraSystem.Cameras.Cam05)
        {
            progression++;
        }
        else
        {
            if (!NM.CamsFullyOpened)
            {
                progression++;
            }
        }
    }

    //called when deathcoined
    public override void OnDeathcoined()
    {
        base.OnDeathcoined();
        progression = 0;
        pulse = false;
        Doggie.gameObject.SetActive(false);
    }

    public override void OnMirrorSummon(int AIChosen)
    {
        base.OnMirrorSummon(AIChosen);
    }
}
