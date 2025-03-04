using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class timeMachineMonster : AnimatronicBase
{
    cameraSystem.Cameras cam;

    public GameObject dinasour;

    bool appeared = false;

    //called when animatronic gets his AILevel
    public override void AnimatronicStart()
    {
        if (!GM.silent)
        {
            var col = dinasour.GetComponent<Image>().color;
            col.a = 200.0f / 255;
            dinasour.GetComponent<Image>().color = col;
        }
    }

    //called every frame after the Oppretunity calculations
    public override void AnimatronicUpdate()
    {
        if (NM.CamSys.CurrentCamera != cam)
        {
            dinasour.SetActive(false);
            appeared = false;
        }
    }

    //called every oppretunity
    public override void OnOppretunity()
    {
        if (!appeared)
        {
            appeared = true;
            cam = NM.CamSys.CurrentCamera;
            NM.CamSys.PulseStatic();
            dinasour.SetActive(true);
        }
    }

    //called when deathcoined
    public override void OnDeathcoined()
    {
        base.OnDeathcoined();
        appeared = false;
        dinasour.SetActive(false);
        NM.CamSys.PulseStatic();
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
